using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JBOFarmersMkt.Context;
using JBOFarmersMkt.Models;
using System.IO;
using CsvHelper;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Entity.Core;
using JBOFarmersMkt.ViewModels;

namespace JBOFarmersMkt.Controllers
{
    public class ImportController : Controller
    {

        JBOContext context = new JBOContext();

        //
        // GET: /Import/

        public ActionResult Index()
        {
            // Change this to return lastModifiedDate for product and sales
            // to save a round trip from the client later
            //return View(context.Imports
            //    .ToList());

            // Get last 5 product hashes
            var productHashes = context.Imports
                .Where(i => i.type == ImportCategories.Products)
                .OrderByDescending(i => i.CreatedAt)
                .Take(5)
                .Select(i => i.contentHash);

            // Get the last 5 sales hashes
            var salesHashes = context.Imports
                .Where(i => i.type == ImportCategories.Sales)
                .OrderByDescending(i => i.CreatedAt)
                .Take(5)
                .Select(i => i.contentHash);

            // Get the date of the most recent products import
            var lastProductsImportDate = context.Imports
                .Where(i => i.type == ImportCategories.Products)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => i.CreatedAt)
                .DefaultIfEmpty()
                .First();

            // Get the date of the most recent sales import
            var lastSalesImportDate = context.Imports
                .Where(i => i.type == ImportCategories.Sales)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => i.CreatedAt)
                .DefaultIfEmpty()
                .First();

            // Send this data to the view for client-side validations
            ViewBag.hashes = productHashes.Concat(salesHashes).ToArray();
            ViewBag.lastProductsImportDate = lastProductsImportDate.ToString("s");
            ViewBag.lastSalesImportDate = lastSalesImportDate.ToString("s");

            return View(new ImportViewModel());
        }

        [HttpPost]
        public ActionResult Upload(ImportViewModel model)
        {
            bool allImportsFailed = true;

            ImportUploadStatusViewModel p = new ImportUploadStatusViewModel { name = "products" };
            ImportUploadStatusViewModel s = new ImportUploadStatusViewModel { name = "sales" };

            // Process both fields separately so that if one fails, the user
            // doesn't have to redo the successful one as well.
            if (model.products != null && ModelState.IsValidField("products"))
            {
                // Do import
                try
                {
                    var results = Import.FromCSV(ImportCategories.Products, model.products, model.productsHash);

                    p.FormatSuccessMessage(results);
                    p.success = true;

                    allImportsFailed = false;
                }
                catch (EntityException)
                {
                    // Something happened with the database.
                    // The best we can do is tell the user the import failed.
                    // This should be logged as well if that ever gets implemented.
                    p.dbErrors.Add("Database Error: Couldn't import products. Please try a different file.");
                    //throw;
                }
            }

            if (model.sales != null && ModelState.IsValidField("sales"))
            {
                // Do import
                try
                {
                    var results = Import.FromCSV(ImportCategories.Sales, model.sales, model.salesHash);

                    s.FormatSuccessMessage(results);
                    s.success = true;

                    allImportsFailed = false;
                }
                catch (EntityException)
                {
                    // Same as above...
                    s.dbErrors.Add("Database Error: Couldn't import sales. Please try a different file.");
                    //throw;
                }
            }

            if (allImportsFailed)
            {
                // The submission failed validation.
                // See here for the ModelState errors incantation:
                // http://stackoverflow.com/questions/1352948/how-to-get-all-errors-from-asp-net-mvc-modelstate#comment33109172_4934712
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)),
                    details = new List<object> { p, s }
                });
            }

            // At least one of the imports succeeded.
            return Json(new
            {
                success = true,
                details = new List<object> { p, s }
            });
        }

        //    public ActionResult importReturns(int? page)
        //    {
        //        return View(context.Imports
        //            .ToList().ToPagedList(page ?? 1, 10));
        //    }

        //    [HttpPost]
        //    public ActionResult importReturns(HttpPostedFileBase file)
        //    {
        //        string path = null;

        //        List<Return> display = new List<Return>();
        //        JBOContext context = new JBOContext();
        //        try
        //        {
        //            if (file.ContentLength > 0)
        //            {
        //                JBODatabase db = new JBODatabase();

        //                var fileName = Path.GetFileName(file.FileName);
        //                path = AppDomain.CurrentDomain.BaseDirectory + "upload\\" + fileName;
        //                file.SaveAs(path);

        //                //var checkFileName = context.Import.Where(i => i.filename == fileName);

        //                var check = (from i in context.Imports where i.filename == fileName select i.filename).Count();

        //                if (check == 0)
        //                {

        //                    StreamReader files = new StreamReader(path);
        //                    var csv = new CsvReader(files);
        //                    csv.Configuration.RegisterClassMap<ReturnClassMap>();

        //                    var productList = csv.GetRecords<ReturnView>().ToList();


        //                    foreach (var s in productList)
        //                    {
        //                        Return returnDisplay = new Return();

        //                        returnDisplay.returnId = int.Parse(s.returnId);
        //                        returnDisplay.returnDate = DateTime.Parse(s.returnDate);
        //                        returnDisplay.custId = int.Parse(s.custId);
        //                        returnDisplay.description = s.description;
        //                        returnDisplay.department = s.department;
        //                        returnDisplay.category = s.category;
        //                        returnDisplay.upc = s.upc;
        //                        returnDisplay.storeCode = s.storeCode;
        //                        returnDisplay.unitPrice = decimal.Parse(s.unitPrice);
        //                        returnDisplay.quantity = double.Parse(s.quantity);
        //                        returnDisplay.totalPrice = decimal.Parse(s.totalPrice);
        //                        returnDisplay.discount = decimal.Parse(s.discount);
        //                        returnDisplay.total = decimal.Parse(s.total);
        //                        returnDisplay.cost = decimal.Parse(s.cost);
        //                        returnDisplay.register = int.Parse(s.register);
        //                        returnDisplay.supplier = s.supplier;

        //                        display.Add(returnDisplay);

        //                    }

        //                    db.addImportedFile(fileName);
        //                    List<Return> updated = new List<Return>();
        //                    List<Return> newItems = new List<Return>();

        //                    using (JBOContext ctx = new JBOContext())
        //                    {
        //                        var returns = from s in context.Returns select s.returnId;
        //                        var update = from s in display where returns.Contains(s.returnId) select s;
        //                        var newItem = from s in display where !returns.Contains(s.returnId) select s;
        //                        foreach (var i in update)
        //                        {
        //                            updated.Add(i);
        //                        }

        //                        foreach (var i in newItem)
        //                        {
        //                            context.Returns.Add(i);
        //                            context.SaveChanges();
        //                        }

        //                    }

        //                    using (JBOContext cont = new JBOContext())
        //                    {
        //                        foreach (Return i in updated)
        //                        {
        //                            Return returns = context.Returns.Single(s => s.returnId == i.returnId);

        //                            returns.returnId = i.returnId;
        //                            returns.returnDate = i.returnDate;
        //                            returns.custId = i.custId;
        //                            returns.description = i.description;
        //                            returns.department = i.department;
        //                            returns.category = i.category;
        //                            returns.upc = i.upc;
        //                            returns.storeCode = i.storeCode;
        //                            returns.unitPrice = i.unitPrice;
        //                            returns.quantity = i.quantity;
        //                            returns.totalPrice = i.totalPrice;
        //                            returns.discount = i.discount;
        //                            returns.total = i.total;
        //                            returns.cost = i.cost;
        //                            returns.register = i.register;
        //                            returns.supplier = i.supplier;

        //                            try
        //                            {
        //                                context.SaveChanges();
        //                            }
        //                            catch (EntityException ex)
        //                            {

        //                            }
        //                        }

        //                    }
        //                }



        //            }
        //            else
        //            {
        //                ViewBag.Error = TempData["error"] = "This File has already been imported!";
        //                ViewBag.Message = TempData["Message"] = "If you wish to re-upload this file, try deleting it first then retry.";
        //                return View();

        //            }

        //        }

        //        catch (DbEntityValidationException e)
        //        {
        //            foreach (var eve in e.EntityValidationErrors)
        //            {
        //                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                foreach (var ve in eve.ValidationErrors)
        //                {
        //                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                        ve.PropertyName, ve.ErrorMessage);
        //                }
        //            }
        //            throw;
        //        }

        //        return RedirectToAction("../Product/index");


        //    }

        //    public ActionResult importTransactions(int? page)
        //    {
        //        return View(context.Imports
        //            .ToList().ToPagedList(page ?? 1, 10));
        //    }

        //    [HttpPost]
        //    public ActionResult importTransactions(HttpPostedFileBase file)
        //    {
        //        string path = null;

        //        List<Transaction> display = new List<Transaction>();
        //        JBOContext context = new JBOContext();
        //        try
        //        {
        //            if (file.ContentLength > 0)
        //            {
        //                JBODatabase db = new JBODatabase();

        //                var fileName = Path.GetFileName(file.FileName);
        //                path = AppDomain.CurrentDomain.BaseDirectory + "upload\\" + fileName;
        //                file.SaveAs(path);

        //                //var checkFileName = context.Import.Where(i => i.filename == fileName);

        //                var check = (from i in context.Imports where i.filename == fileName select i.filename).Count();

        //                if (check == 0)
        //                {

        //                    StreamReader files = new StreamReader(path);
        //                    var csv = new CsvReader(files);
        //                    csv.Configuration.RegisterClassMap<TransactionClassMap>();

        //                    var transactionList = csv.GetRecords<TransactionView>().ToList();


        //                    foreach (var s in transactionList)
        //                    {
        //                        Transaction transactionDisplay = new Transaction();

        //                        transactionDisplay.transactionCode = int.Parse(s.transactionCode);
        //                        transactionDisplay.type = s.type;
        //                        transactionDisplay.storeCode = s.storeCode;
        //                        transactionDisplay.description = s.description;
        //                        transactionDisplay.category = s.category;
        //                        transactionDisplay.department = s.department;
        //                        transactionDisplay.supplier = s.supplier;                            
        //                        transactionDisplay.price = decimal.Parse(s.price);
        //                        transactionDisplay.quantity = double.Parse(s.quantity);                            
        //                        transactionDisplay.subtotal = decimal.Parse(s.subtotal);
        //                        transactionDisplay.tax = decimal.Parse(s.tax);
        //                        transactionDisplay.discount = decimal.Parse(s.discount);
        //                        transactionDisplay.total = decimal.Parse(s.total);
        //                        transactionDisplay.cashier = s.cashier;
        //                        transactionDisplay.date = DateTime.Parse(s.date);
        //                        transactionDisplay.register = s.register;


        //                        display.Add(transactionDisplay);

        //                    }

        //                    db.addImportedFile(fileName);
        //                    List<Transaction> updated = new List<Transaction>();
        //                    List<Transaction> newItems = new List<Transaction>();

        //                    using (JBOContext ctx = new JBOContext())
        //                    {
        //                        var transactions = from s in context.Transactions select s.transactionId;
        //                        var update = from s in display where transactions.Contains(s.transactionId) select s;
        //                        var newItem = from s in display where !transactions.Contains(s.transactionId) select s;
        //                        foreach (var i in update)
        //                        {
        //                            updated.Add(i);
        //                        }

        //                        foreach (var i in newItem)
        //                        {
        //                            context.Transactions.Add(i);
        //                            context.SaveChanges();
        //                        }

        //                    }

        //                    using (JBOContext cont = new JBOContext())
        //                    {
        //                        foreach (Transaction i in updated)
        //                        {
        //                            Transaction transactions = context.Transactions.Single(s => s.transactionId == i.transactionId);

        //                            transactions.transactionCode = i.transactionCode;
        //                            transactions.type = i.type;
        //                            transactions.storeCode = i.storeCode;
        //                            transactions.description = i.description;
        //                            transactions.category = i.category;
        //                            transactions.department = i.department;
        //                            transactions.supplier = i.supplier;                                
        //                            transactions.price = i.price;
        //                            transactions.quantity = i.quantity;

        //                            transactions.subtotal = i.subtotal;
        //                            transactions.tax = i.tax;
        //                            transactions.discount = i.discount;
        //                            transactions.total = i.total;
        //                            transactions.cashier = i.cashier;
        //                            transactions.date = i.date;
        //                            transactions.register = i.register;

        //                            try
        //                            {
        //                                context.SaveChanges();
        //                            }
        //                            catch (EntityException ex)
        //                            {

        //                            }
        //                        }

        //                    }
        //                }



        //            }
        //            else
        //            {
        //                ViewBag.Error = TempData["error"] = "This File has already been imported!";
        //                ViewBag.Message = TempData["Message"] = "If you wish to re-upload this file, try deleting it first then retry.";
        //                return View();

        //            }

        //        }

        //        catch (DbEntityValidationException e)
        //        {
        //            foreach (var eve in e.EntityValidationErrors)
        //            {
        //                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                foreach (var ve in eve.ValidationErrors)
        //                {
        //                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                        ve.PropertyName, ve.ErrorMessage);
        //                }
        //            }
        //            throw;
        //        }

        //        return RedirectToAction("../Product/index");


        //    }

    }
}
