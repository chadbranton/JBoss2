using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.ViewModels
{
    /// <summary>
    /// Models a specific upload for error handling.
    /// </summary>
    /// <example> 
    /// Failing products import
    /// <code>
    ///     // Products is the id of the input and capitalized so that it will
    ///     // look nice in the success message.
    ///     ImportUploadStatusViewModel p = new ImportUploadStatusViewModel { name = "Products" };
    ///     p.success = false;
    ///     p.dbErrors.Add("The database did not approve because...");
    /// </code>
    /// </example>
    /// <example>
    /// Successful sales import
    /// <code>
    ///     ImportUploadStatusViewModel s = new ImportUploadStatusViewModel { name = "Sales" };
    ///     s.success = true;
    ///     s.message = "Successfully imported sales. 120,000 records updated. 12,000 records created.";
    /// </code>
    /// </example>
    public class ImportUploadStatusViewModel
    {
        public bool success { get; set; }
        // Errors that occur during import
        public List<string> dbErrors { get; set; }
        public string name { get; set; }
        // Message on successful import.
        public string message { get; set; }

        public ImportUploadStatusViewModel()
        {
            success = false;
            dbErrors = new List<string>();
        }

        /// <summary>
        /// Sets up the success message.
        /// </summary>
        /// <param name="results">A Tuple containing the items updated and created.</param>
        public void FormatSuccessMessage(Tuple<int, int> results, bool insertOnly = false) {
            string format = "{0} import complete. {1} items updated. {2} items created.";

            if (insertOnly)
            {
                format = "{0} import complete. {2} items created.";
            }

            message = String.Format(format, name, results.Item1, results.Item2);
        }
    }
}