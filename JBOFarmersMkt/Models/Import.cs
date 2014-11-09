using CsvHelper;
using JBOFarmersMkt.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Models
{
    public enum ImportCategories
    {
        Sales,
        Products
    }

    public class Import : IAuditedEntity
    {
        public int Id { get; set; }
        public string filename { get; set; }
        public ImportCategories type { get; set; }
        public string contentHash { get; set; }

        [DisplayName("Imported By")]
        public string CreatedBy
        {
            get;
            set;
        }

        [DisplayName("Imported At")]
        public DateTime CreatedAt
        {
            get;
            set;
        }

        public string LastModifiedBy
        {
            get;
            set;
        }

        public DateTime LastModifiedAt
        {
            get;
            set;
        }

        public static Import FindByContentHash(string h)
        {
            using (JBOContext context = new JBOContext())
            {
                return context.Imports.Where(i => i.contentHash == h).DefaultIfEmpty(null).FirstOrDefault(i => i.contentHash == h);
            }
        }

        public static bool IsUniqueContentHash(string h)
        {
            if (FindByContentHash(h) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Imports the given category from the given CSV stream.
        /// </summary>
        /// <param name="type">The type of import.</param>
        /// <param name="file">The csv file.</param>
        /// <param name="contentHash">The hash of the csv content.</param>
        public static Tuple<int, int> FromCSV(ImportCategories type, HttpPostedFileBase csv, string contentHash)
        {
            bool error = false;
            Tuple<int, int> results = null;

            using (var context = new JBOContext())
            {
                switch (type)
                {
                    case ImportCategories.Sales:
                        results = SalesFromCSV(csv.InputStream, context);
                        break;
                    case ImportCategories.Products:
                        results = ProductsFromCSV(csv.InputStream, context);
                        break;
                    default:
                        error = true;
                        break;
                }

                if (error)
                {
                    // I'm pretty sure C# does this automatically, but just in case
                    // until I can find out for sure...
                    throw new InvalidEnumArgumentException("Invalid import type.");
                }
                else
                {
                    // Something should've been imported.
                    context.Imports.Add(
                        new Import
                        {
                            contentHash = contentHash,
                            type = type,
                            filename = csv.FileName
                        });

                    // All done. Let's save the changes.
                    context.SaveChanges();

                    return results;
                }
            }
        }

        /// <summary>
        /// Imports products from a csv stream into the given context.
        /// The caller is responsible for calling SaveChanges().
        /// </summary>
        /// <param name="csv">The CSV stream</param>
        /// <param name="context">The DB Context</param>
        private static Tuple<int, int> ProductsFromCSV(Stream csv, JBOContext context)
        {
            List<Product> allImportedProducts = new List<Product>();

            if (csv != null && csv.Length > 0)
            {
                var csvReader = new CsvReader(new StreamReader(csv));
                csvReader.Configuration.RegisterClassMap<ProductClassMap>();

                allImportedProducts = csvReader.GetRecords<Product>().ToList();

                List<Product> updated = new List<Product>();
                List<Product> newItems = new List<Product>();

                var products = from p in context.Products select p.productCode;
                updated = (from p in allImportedProducts where products.Contains(p.productCode) select p).ToList();
                newItems = (from p in allImportedProducts where !products.Contains(p.productCode) select p).ToList();

                // Add the new items to the context so they will be saved
                // shortly.
                context.Products.AddRange(newItems);

                foreach (Product i in updated)
                {
                    Product prod = context.Products.Single(p => p.productCode == i.productCode);
                    prod.productCode = i.productCode;
                    prod.description = i.description;
                    prod.department = i.department;
                    prod.category = i.category;
                    prod.upc = i.upc;
                    prod.storeCode = i.storeCode;
                    prod.unitPrice = i.unitPrice;
                    prod.discountable = i.discountable;
                    prod.taxable = i.taxable;
                    prod.inventoryMethod = i.inventoryMethod;
                    prod.quantity = i.quantity;
                    prod.orderTrigger = i.orderTrigger;
                    prod.recommendedOrder = i.recommendedOrder;
                    prod.lastSoldDate = i.lastSoldDate;
                    prod.supplier = i.supplier;
                    prod.liabilityItem = i.liabilityItem;
                    prod.LRT = i.LRT;
                }

                return new Tuple<int, int>(updated.Count, newItems.Count);
            }

            return new Tuple<int, int>(0, 0);
        }

        /// <summary>
        /// Imports sales from a csv stream into the given context.
        /// The caller is responsible for calling SaveChanges().
        /// </summary>
        /// <param name="csv">The CSV stream</param>
        /// <param name="context">The DB Context</param>
        private static Tuple<int, int> SalesFromCSV(Stream csv, JBOContext context)
        {
            List<Sale> allImportedSales = new List<Sale>();

            if (csv != null && csv.Length > 0)
            {
                var csvReader = new CsvReader(new StreamReader(csv));
                csvReader.Configuration.RegisterClassMap<SalesClassMap>();

                allImportedSales = csvReader.GetRecords<Sale>().ToList();

                //List<Sale> updated = new List<Sale>();
                List<Sale> newItems = new List<Sale>();

                var salesCodes = context.Sales.Select(s => s.transCode);

                // Don't update sales like we did in products.
                // Transaction codes are unique to a transaction.
                // A transaction could have 30 items sold.
                // The system is currently not designed to distinguish which of these items
                // it may need to update within a given transaction.
                // It is also likely that ShopKeep does not allow changing this data, and we won't
                // receive transactions that have been changed anyway.
                //updated = (from s in allImportedSales where salesCodes.Contains(s.transCode) select s).ToList();
                newItems = (from s in allImportedSales where !salesCodes.Contains(s.transCode) select s).ToList();

                // Add the new items to the context so they will be saved
                // shortly.
                context.Sales.AddRange(newItems);

                return new Tuple<int, int>(0, newItems.Count);
            }
            return new Tuple<int, int>(0, 0);
        }
    }
}