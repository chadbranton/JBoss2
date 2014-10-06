using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JBOFarmersMkt.Models;
using JBOFarmersMkt.Context;
using PagedList;
using PagedList.Mvc;

namespace JBOFarmersMkt.Controllers
{
    public class ProductController : Controller
    {
        private JBOContext db = new JBOContext();

        //
        // GET: /Product/
        [Authorize]
        public ActionResult Index(int? page, string sortBy, int? search, string searchName, string searchSupplier)
        {
            if (User.Identity.Name != "admin")
            {
                ///<summary>
                ///
                /// lambda expression to select the user object that is logged in.    
                /// 
                ///</summary>
                ///<remarks>
                /// None
                ///</remarks>
                ///

                UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name);

                ///<summary>
                ///
                /// linq statement to build a list of all suppliers that have the logged in user
                /// in the user ICollection of the suppliers.  If the UserProfile user is not null 
                /// then execute the linq query statments, else return a blank view.  If the       
                /// logged in user is not "admin" then linq will pull all of the sale items        
                /// associated with the corresponding supplier, else linq will pull all sales for  
                /// the user "admin".
                /// 
                ///</summary>
                ///<remarks>
                /// None
                ///</remarks>

                if (user != null)
                {
                    var supplier = (from s in db.Suppliers
                                    where s.users.Any(i => i.UserId == user.UserId)
                                    select s.name).ToList();

                    var product = from p in db.Products
                                  where supplier.Contains(p.supplier)
                                  select p;


                    ///<summary>
                    ///
                    /// sale is passed to salesQuery as a Queryable object 
                    /// 
                    ///</summary>
                    ///<remarks>
                    /// None
                    ///</remarks>
                    ///

                    var productQuery = product.AsQueryable();

                    ///<summary>
                    ///
                    /// Sort Parameters and query string patterns for Date, Supplier, and 
                    /// Description along with Switch control to determine what sort pattern to 
                    /// follow by.
                    /// 
                    ///</summary>
                    ///<remarks>
                    /// None
                    ///</remarks>
                    ///

                    ViewBag.SortCodeParameter = string.IsNullOrEmpty(sortBy) ? "productCode desc" : "";
                    ViewBag.SortSupplierParameter = string.IsNullOrEmpty(sortBy) ? "supplier desc" : "";
                    ViewBag.SortItemParameter = string.IsNullOrEmpty(sortBy) ? "description desc" : "";

                    switch (sortBy)
                    {

                        case "productCode desc": //order by supplier descending
                            productQuery = productQuery.OrderByDescending(x => x.productCode);
                            break;

                        case "description desc": //order by description descending
                            productQuery = productQuery.OrderByDescending(x => x.description);
                            break;

                        case "description": //order by description ascending
                            productQuery = productQuery.OrderBy(x => x.description);
                            break;

                        default: //default order by supplier ascending
                            productQuery = productQuery.OrderBy(x => x.productCode);
                            break;
                    }


                    ///<summary>
                    ///
                    /// If search is not null then return the sale items requested.
                    /// 
                    ///</summary>
                    ///<remarks>
                    /// None
                    ///</remarks>
                    ///  

                    if (search != null && productQuery != null)
                    {

                        return View(product.Where(x => x.productCode == search).ToList().ToPagedList(page ?? 1, 10));

                    }

                    if (searchName != null)
                    {
                        
                        return View(product.Where(p => p.description.ToUpper().Contains(searchName.ToUpper())).ToList().ToPagedList(page ?? 1, 10));
                    }

                    if (productQuery != null)
                    {
                        return View(productQuery.ToList().ToPagedList(page ?? 1, 20));
                    }

                }
            }
            else
            {
                ///<summary>
                ///
                /// Calculate Total Sales.  Uses linq expression to total the sales.  Then passes  
                /// the total figure to the view.
                /// 
                ///</summary>
                ///<remarks>
                /// None
                ///</remarks>
                ///  

                var retrieve = from p in db.Products select p;
                if (retrieve.Any())
                {

                }
                else
                {
                    ViewBag.Message = "There are no sale items to display!";
                }

                var products = from p in db.Products select p;
                var productQuery = products.AsQueryable();

                ///<summary>
                ///
                /// Sort Parameters and query string patterns for Date, Supplier, and 
                /// Description along with Switch control to determine what sort pattern to 
                /// follow by.
                /// 
                ///</summary>
                ///<remarks>
                /// None
                ///</remarks>
                ///

                ViewBag.SortDateParameter = string.IsNullOrEmpty(sortBy) ? "date desc" : "";
                ViewBag.SortSupplierParameter = string.IsNullOrEmpty(sortBy) ? "supplier desc" : "";
                ViewBag.SortItemParameter = string.IsNullOrEmpty(sortBy) ? "description desc" : "";

                switch (sortBy)
                {

                    case "supplier desc": //order by supplier descending
                        productQuery = productQuery.OrderByDescending(x => x.supplier);
                        break;

                    case "supplier": //order by supplier ascending
                        productQuery = productQuery.OrderBy(x => x.supplier);
                        break;

                    case "description desc": //order by description descending
                        productQuery = productQuery.OrderByDescending(x => x.description);
                        break;

                    case "description": //order by description ascending
                        productQuery = productQuery.OrderBy(x => x.description);
                        break;

                    default: //default order by supplier ascending
                        productQuery = productQuery.OrderBy(x => x.supplier);
                        break;
                }


                ///<summary>
                ///
                /// If search is not null then return the items requested. 
                /// 
                /// 
                ///</summary>
                ///<remarks>
                /// None
                ///</remarks>
                ///  
                if (search != null && productQuery != null)
                {
                    return View(db.Products.Where(x => x.productCode == search).ToList().ToPagedList(page ?? 1, 10));
                }

                if (searchName != null)
                {
                    return View(db.Products.Where(p => p.description.ToUpper().Contains(searchName.ToUpper())).ToList().ToPagedList(page ?? 1, 10));
                }

                if (searchSupplier != null)
                {
                    return View(db.Products.Where(p => p.supplier.ToUpper().Contains(searchSupplier.ToUpper())).ToList().ToPagedList(page ?? 1, 10));
                }

                else
                {
                    return View(productQuery.ToList().ToPagedList(page ?? 1, 20));
                }


            }

            ///<summary>
            ///
            /// If all above fails, then return a blank view.
            /// 
            ///</summary>
            ///<remarks>
            /// None
            ///</remarks>
            ///  

            return View();

        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                Product prod = db.Products.Single(p => p.productCode == product.productCode);
                prod.productCode = product.productCode;
                prod.description = product.description;
                prod.department = product.department;
                prod.category = product.category;
                prod.upc = product.upc;
                prod.storeCode = product.storeCode;
                prod.unitPrice = product.unitPrice;
                prod.discountable = product.discountable;
                prod.taxable = product.taxable;
                prod.inventoryMethod = product.inventoryMethod;
                prod.quantity = product.quantity;
                prod.orderTrigger = product.orderTrigger;
                prod.recommendedOrder = product.recommendedOrder;
                prod.lastSoldDate = product.lastSoldDate;
                prod.supplier = product.supplier;
                prod.liabilityItem = product.liabilityItem;
                prod.LRT = product.LRT;
                prod.ProductImageUrl = product.ProductImageUrl;
                try
                {
                    db.SaveChanges();
                }
                catch (EntityException ex)
                {

                }
                //db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}