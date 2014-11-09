using JBOFarmersMkt.Context;
using JBOFarmersMkt.Models;
using JBOFarmersMkt.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace JBOFarmersMkt.ViewModels
{
    [CannotAllBeEmpty("products", "sales", ErrorMessage = "The products and sales files cannot both be empty.")]
    public class ImportViewModel
    {
        // Hashes are set if validation gets around to hashing.
        public string productsHash { get; private set; }
        public string salesHash { get; private set; }

        [ValidFile(@"stock_items.*\.csv$", @"productsHash")]
        public HttpPostedFileBase products { get; set; }

        // What we are referring to as sales is sold items in ShopKeep
        [ValidFile(@"sold_items_from_.+_to_.+\.csv$", @"salesHash")]
        public HttpPostedFileBase sales { get; set; }

        /// <summary>
        /// ValidFile requires that an HttpPostedFileBase has a file name matching the given regex and
        /// Ensures that the file is unique based on its hash.
        /// </summary>
        private class ValidFile : ValidationAttribute
        {
            private readonly string _r;
            private readonly string _p;

            /// <summary>
            /// ValidFile requires that an HttpPostedFileBase has a file name matching the given regex and
            /// ensures that the file is unique based on its hash.
            /// </summary>
            /// <param name="r">The regex to compare the filename to.</param>
            public ValidFile(string r)
                : base("Invalid file: {0}")
            {
                _r = r;
            }

            /// <summary>
            /// ValidFile requires that an HttpPostedFileBase has a file name matching the given regex and
            /// ensures that the file is unique based on its hash. Optionally stores the calculated hash in a
            /// property on the model.
            /// </summary>
            /// <param name="r">The regex to compare the filename to.</param>
            /// <param name="p">The name of the property that will hold the hash.</param>
            public ValidFile(string r, string p)
                : base("Invalid file: {0}")
            {
                _r = r;
                _p = p;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                HttpPostedFileBase file = value as HttpPostedFileBase;
                if (value != null)
                {
                    // Check the filename before doing the
                    // expensive hash check
                    if (!Regex.IsMatch(file.FileName, _r))
                    {
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName +
                            ". This name does is not consistent with ShopKeep's for this file.");
                        return new ValidationResult(errorMessage);
                    }
                    else
                    {
                        // The name is valid. Now we check the hash.
                        // Compute the hash.
                        string hash = StreamHasher.ComputeHash(file.InputStream);

                        // Store the hash on the model if a property was given.
                        if (_p != null)
                        {
                            var m = validationContext.ObjectInstance;
                            m.GetType().GetProperty(_p).SetValue(m, hash);
                        }

                        // Check that the file is unique
                        if (!Import.IsUniqueContentHash(hash))
                        {
                            // It isn't
                            var errorMessage = FormatErrorMessage(validationContext.DisplayName +
                                ". This file has already been uploaded.");
                            return new ValidationResult(errorMessage);
                        }
                    }
                }

                // The file checks out
                return ValidationResult.Success;
            }
        }
    }

    /// <summary>
    /// CannotAllBeEmpty ensures that at least one of the given fields is not null.
    /// </summary>
    public class CannotAllBeEmpty : ValidationAttribute
    {
        private readonly string[] _fieldNames;

        /// <summary>
        /// CannotAllBeEmpty ensures that at least one of the given fields is not null.
        /// </summary>
        /// <param name="fieldNames">Names of properties in the model of which at least one must not be null.</param>
        public CannotAllBeEmpty(params string[] fieldNames)
            : base("Some fields are missing.")
        {
            _fieldNames = fieldNames;
        }

        public override bool IsValid(object value)
        {
            var valueType = value.GetType();

            int count = 0;

            foreach (var field in _fieldNames)
            {
                // This will throw an exception if field is not defined within the given object.
                // This should be fine since we want to catch that problem as early as possible.
                if (valueType.GetProperty(field).GetValue(value) == null)
                {
                    count = count + 1;
                }
            }

            return count < _fieldNames.Length;
        }
    }
}