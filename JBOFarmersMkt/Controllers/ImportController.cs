﻿using JBOFarmersMkt.Models;
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
using PagedList;
using PagedList.Mvc;

namespace JBOFarmersMkt.Controllers
{
    public class ImportController : Controller
    {

        JBOContext context = new JBOContext();

        //
        // GET: /Import/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult importProducts(int? page)
        {
            return View(context.Imports
                .ToList().ToPagedList(page ?? 1,10));
        }

        [HttpPost]
        public ActionResult importProducts(HttpPostedFileBase file)
        {
            string path = null;

            List<Product> display = new List<Product>();
            JBOContext context = new JBOContext();
            try
            {
                if (file.ContentLength > 0)
                {
                    JBODatabase db = new JBODatabase();

                    var fileName = Path.GetFileName(file.FileName);
                    path = AppDomain.CurrentDomain.BaseDirectory + "upload\\" + fileName;
                    file.SaveAs(path);

                    //var checkFileName = context.Import.Where(i => i.filename == fileName);

                    var check = (from i in context.Imports where i.filename == fileName select i.filename).Count();

                    if (check == 0)
                    {

                        StreamReader files = new StreamReader(path);
                        var csv = new CsvReader(files);
                        csv.Configuration.RegisterClassMap<ProductClassMap>();

                        var productList = csv.GetRecords<ProductView>().ToList();


                        foreach (var s in productList)
                        {
                            Product productDisplay = new Product();

                            productDisplay.productCode = int.Parse(s.productCode);
                            productDisplay.description = s.description;
                            productDisplay.department = s.department;
                            productDisplay.category = s.category;
                            productDisplay.upc = s.upc;
                            productDisplay.storeCode = s.storeCode;
                            productDisplay.unitPrice = decimal.Parse(s.unitPrice);
                            productDisplay.discountable = Boolean.Parse(s.discountable);
                            productDisplay.taxable = Boolean.Parse(s.taxable);
                            productDisplay.inventoryMethod = s.inventoryMethod;                           
                            productDisplay.quantity = double.Parse(s.quantity);
                            productDisplay.orderTrigger = int.Parse(s.orderTrigger);
                            productDisplay.recommendedOrder = int.Parse(s.recommendedOrder);
                            productDisplay.lastSoldDate = s.lastSoldDate;
                            productDisplay.supplier = s.supplier;
                            productDisplay.liabilityItem = s.liabilityItem;
                            productDisplay.LRT = s.LRT;

                            display.Add(productDisplay);

                        }

                        db.addImportedFile(fileName);
                        List<Product> updated = new List<Product>();
                        List<Product> newItems = new List<Product>();

                        using (JBOContext ctx = new JBOContext())
                        {
                            var products = from p in context.Products select p.productCode;
                            var update = from p in display where products.Contains(p.productCode) select p;
                            var newItem = from p in display where !products.Contains(p.productCode) select p;
                            foreach (var i in update)
                            {
                                updated.Add(i);
                            }

                            foreach (var i in newItem)
                            {
                                context.Products.Add(i);
                                context.SaveChanges();
                            }

                        }

                        using (JBOContext cont = new JBOContext())
                        {
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
                                try
                                {
                                    context.SaveChanges();
                                }
                                catch (EntityException ex)
                                {

                                }
                            }

                        }
                    }



                }
                else
                {
                    ViewBag.Error = TempData["error"] = "This File has already been imported!";
                    ViewBag.Message = TempData["Message"] = "If you wish to re-upload this file, try deleting it first then retry.";
                    return View();

                }

            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return RedirectToAction("../Product/index");


        }

        public ActionResult importSales(int? page)
        {
            return View(context.Imports
                .ToList().ToPagedList(page ?? 1, 10));
        }

        [HttpPost]
        public ActionResult importSales(HttpPostedFileBase file)
        {
            string path = null;

            List<Sale> display = new List<Sale>();
            JBOContext context = new JBOContext();
            try
            {
                if (file.ContentLength > 0)
                {
                    JBODatabase db = new JBODatabase();

                    var fileName = Path.GetFileName(file.FileName);
                    path = AppDomain.CurrentDomain.BaseDirectory + "upload\\" + fileName;
                    file.SaveAs(path);

                    //var checkFileName = context.Import.Where(i => i.filename == fileName);

                    var check = (from i in context.Imports where i.filename == fileName select i.filename).Count();

                    if (check == 0)
                    {

                        StreamReader files = new StreamReader(path);
                        var csv = new CsvReader(files);
                        csv.Configuration.RegisterClassMap<SalesClassMap>();

                        var productList = csv.GetRecords<SaleView>().ToList();


                        foreach (var s in productList)
                        {
                            Sale saleDisplay = new Sale();

                            saleDisplay.transCode = int.Parse(s.transCode);
                            saleDisplay.date = DateTime.Parse(s.date);
                            saleDisplay.custId = int.Parse(s.custID);
                            saleDisplay.description = s.description;
                            saleDisplay.department = s.department;
                            saleDisplay.category = s.category;
                            saleDisplay.upc = s.upc;
                            saleDisplay.storeCode = s.storeCode;
                            saleDisplay.unitPrice = decimal.Parse(s.unitPrice);
                            saleDisplay.quantity = double.Parse(s.quantity);
                            saleDisplay.totalPrice = decimal.Parse(s.totalPrice);
                            saleDisplay.discount = decimal.Parse(s.discount);
                            saleDisplay.total = decimal.Parse(s.total);
                            saleDisplay.cost = decimal.Parse(s.cost);
                            saleDisplay.register = int.Parse(s.register);
                            saleDisplay.supplier = s.supplier;

                            display.Add(saleDisplay);

                        }

                        db.addImportedFile(fileName);
                        List<Sale> updated = new List<Sale>();
                        List<Sale> newItems = new List<Sale>();

                        using (JBOContext ctx = new JBOContext())
                        {
                            var sales = from s in context.Sales select s.transCode;
                            var update = from s in display where sales.Contains(s.transCode) select s;
                            var newItem = from s in display where !sales.Contains(s.transCode) select s;
                            foreach (var i in update)
                            {
                                updated.Add(i);
                            }

                            foreach (var i in newItem)
                            {
                                context.Sales.Add(i);
                                context.SaveChanges();
                            }

                        }

                        using (JBOContext cont = new JBOContext())
                        {
                            foreach (Sale i in updated)
                            {
                                Sale sale = context.Sales.Single(s => s.transCode == i.transCode);

                                sale.transCode = i.transCode;
                                sale.date = i.date;
                                sale.custId = i.custId;
                                sale.description = i.description;
                                sale.department = i.department;
                                sale.category = i.category;
                                sale.upc = i.upc;
                                sale.storeCode = i.storeCode;
                                sale.unitPrice = i.unitPrice;
                                sale.quantity = i.quantity;
                                sale.totalPrice = i.totalPrice;
                                sale.discount = i.discount;
                                sale.total = i.total;
                                sale.cost = i.cost;
                                sale.register = i.register;
                                sale.supplier = i.supplier;

                                try
                                {
                                    context.SaveChanges();
                                }
                                catch (EntityException ex)
                                {

                                }
                            }

                        }
                    }



                }
                else
                {
                    ViewBag.Error = TempData["error"] = "This File has already been imported!";
                    ViewBag.Message = TempData["Message"] = "If you wish to re-upload this file, try deleting it first then retry.";
                    return View();

                }

            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return RedirectToAction("../Product/index");


        }

        public ActionResult importReturns(int? page)
        {
            return View(context.Imports
                .ToList().ToPagedList(page ?? 1, 10));
        }

        [HttpPost]
        public ActionResult importReturns(HttpPostedFileBase file)
        {
            string path = null;

            List<Return> display = new List<Return>();
            JBOContext context = new JBOContext();
            try
            {
                if (file.ContentLength > 0)
                {
                    JBODatabase db = new JBODatabase();

                    var fileName = Path.GetFileName(file.FileName);
                    path = AppDomain.CurrentDomain.BaseDirectory + "upload\\" + fileName;
                    file.SaveAs(path);

                    //var checkFileName = context.Import.Where(i => i.filename == fileName);

                    var check = (from i in context.Imports where i.filename == fileName select i.filename).Count();

                    if (check == 0)
                    {

                        StreamReader files = new StreamReader(path);
                        var csv = new CsvReader(files);
                        csv.Configuration.RegisterClassMap<ReturnClassMap>();

                        var productList = csv.GetRecords<ReturnView>().ToList();


                        foreach (var s in productList)
                        {
                            Return returnDisplay = new Return();

                            returnDisplay.returnId = int.Parse(s.returnId);
                            returnDisplay.returnDate = DateTime.Parse(s.returnDate);
                            returnDisplay.custId = int.Parse(s.custId);
                            returnDisplay.description = s.description;
                            returnDisplay.department = s.department;
                            returnDisplay.category = s.category;
                            returnDisplay.upc = s.upc;
                            returnDisplay.storeCode = s.storeCode;
                            returnDisplay.unitPrice = decimal.Parse(s.unitPrice);
                            returnDisplay.quantity = double.Parse(s.quantity);
                            returnDisplay.totalPrice = decimal.Parse(s.totalPrice);
                            returnDisplay.discount = decimal.Parse(s.discount);
                            returnDisplay.total = decimal.Parse(s.total);
                            returnDisplay.cost = decimal.Parse(s.cost);
                            returnDisplay.register = int.Parse(s.register);
                            returnDisplay.supplier = s.supplier;

                            display.Add(returnDisplay);

                        }

                        db.addImportedFile(fileName);
                        List<Return> updated = new List<Return>();
                        List<Return> newItems = new List<Return>();

                        using (JBOContext ctx = new JBOContext())
                        {
                            var returns = from s in context.Returns select s.returnId;
                            var update = from s in display where returns.Contains(s.returnId) select s;
                            var newItem = from s in display where !returns.Contains(s.returnId) select s;
                            foreach (var i in update)
                            {
                                updated.Add(i);
                            }

                            foreach (var i in newItem)
                            {
                                context.Returns.Add(i);
                                context.SaveChanges();
                            }

                        }

                        using (JBOContext cont = new JBOContext())
                        {
                            foreach (Return i in updated)
                            {
                                Return returns = context.Returns.Single(s => s.returnId == i.returnId);

                                returns.returnId = i.returnId;
                                returns.returnDate = i.returnDate;
                                returns.custId = i.custId;
                                returns.description = i.description;
                                returns.department = i.department;
                                returns.category = i.category;
                                returns.upc = i.upc;
                                returns.storeCode = i.storeCode;
                                returns.unitPrice = i.unitPrice;
                                returns.quantity = i.quantity;
                                returns.totalPrice = i.totalPrice;
                                returns.discount = i.discount;
                                returns.total = i.total;
                                returns.cost = i.cost;
                                returns.register = i.register;
                                returns.supplier = i.supplier;

                                try
                                {
                                    context.SaveChanges();
                                }
                                catch (EntityException ex)
                                {

                                }
                            }

                        }
                    }



                }
                else
                {
                    ViewBag.Error = TempData["error"] = "This File has already been imported!";
                    ViewBag.Message = TempData["Message"] = "If you wish to re-upload this file, try deleting it first then retry.";
                    return View();

                }

            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return RedirectToAction("../Product/index");


        }

        public ActionResult importTransactions(int? page)
        {
            return View(context.Imports
                .ToList().ToPagedList(page ?? 1, 10));
        }

        [HttpPost]
        public ActionResult importTransactions(HttpPostedFileBase file)
        {
            string path = null;

            List<Transaction> display = new List<Transaction>();
            JBOContext context = new JBOContext();
            try
            {
                if (file.ContentLength > 0)
                {
                    JBODatabase db = new JBODatabase();

                    var fileName = Path.GetFileName(file.FileName);
                    path = AppDomain.CurrentDomain.BaseDirectory + "upload\\" + fileName;
                    file.SaveAs(path);

                    //var checkFileName = context.Import.Where(i => i.filename == fileName);

                    var check = (from i in context.Imports where i.filename == fileName select i.filename).Count();

                    if (check == 0)
                    {

                        StreamReader files = new StreamReader(path);
                        var csv = new CsvReader(files);
                        csv.Configuration.RegisterClassMap<TransactionClassMap>();

                        var transactionList = csv.GetRecords<TransactionView>().ToList();


                        foreach (var s in transactionList)
                        {
                            Transaction transactionDisplay = new Transaction();
                            
                            transactionDisplay.transactionCode = int.Parse(s.transactionCode);
                            transactionDisplay.type = s.type;
                            transactionDisplay.storeCode = s.storeCode;
                            transactionDisplay.description = s.description;
                            transactionDisplay.category = s.category;
                            transactionDisplay.department = s.department;
                            transactionDisplay.supplier = s.supplier;                            
                            transactionDisplay.price = decimal.Parse(s.price);
                            transactionDisplay.quantity = double.Parse(s.quantity);                            
                            transactionDisplay.subtotal = decimal.Parse(s.subtotal);
                            transactionDisplay.tax = decimal.Parse(s.tax);
                            transactionDisplay.discount = decimal.Parse(s.discount);
                            transactionDisplay.total = decimal.Parse(s.total);
                            transactionDisplay.cashier = s.cashier;
                            transactionDisplay.date = DateTime.Parse(s.date);
                            transactionDisplay.register = s.register;
                            

                            display.Add(transactionDisplay);

                        }

                        db.addImportedFile(fileName);
                        List<Transaction> updated = new List<Transaction>();
                        List<Transaction> newItems = new List<Transaction>();

                        using (JBOContext ctx = new JBOContext())
                        {
                            var transactions = from s in context.Transactions select s.transactionId;
                            var update = from s in display where transactions.Contains(s.transactionId) select s;
                            var newItem = from s in display where !transactions.Contains(s.transactionId) select s;
                            foreach (var i in update)
                            {
                                updated.Add(i);
                            }

                            foreach (var i in newItem)
                            {
                                context.Transactions.Add(i);
                                context.SaveChanges();
                            }

                        }

                        using (JBOContext cont = new JBOContext())
                        {
                            foreach (Transaction i in updated)
                            {
                                Transaction transactions = context.Transactions.Single(s => s.transactionId == i.transactionId);

                                transactions.transactionCode = i.transactionCode;
                                transactions.type = i.type;
                                transactions.storeCode = i.storeCode;
                                transactions.description = i.description;
                                transactions.category = i.category;
                                transactions.department = i.department;
                                transactions.supplier = i.supplier;                                
                                transactions.price = i.price;
                                transactions.quantity = i.quantity;
                                
                                transactions.subtotal = i.subtotal;
                                transactions.tax = i.tax;
                                transactions.discount = i.discount;
                                transactions.total = i.total;
                                transactions.cashier = i.cashier;
                                transactions.date = i.date;
                                transactions.register = i.register;

                                try
                                {
                                    context.SaveChanges();
                                }
                                catch (EntityException ex)
                                {

                                }
                            }

                        }
                    }



                }
                else
                {
                    ViewBag.Error = TempData["error"] = "This File has already been imported!";
                    ViewBag.Message = TempData["Message"] = "If you wish to re-upload this file, try deleting it first then retry.";
                    return View();

                }

            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return RedirectToAction("../Product/index");


        }

    }
}
