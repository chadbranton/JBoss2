using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JBOFarmersMkt.Context;
using JBOFarmersMkt.Models;
using PagedList;
using PagedList.Mvc;

namespace JBOFarmersMkt.Controllers
{
    public class SaleController : Controller
    {
        private JBOContext db = new JBOContext();

        //
        // GET: /Sale/
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

                    var sale = from s in db.Sales
                                  where supplier.Contains(s.supplier)
                                  select s;


                    ///<summary>
                    ///
                    /// sale is passed to salesQuery as a Queryable object 
                    /// 
                    ///</summary>
                    ///<remarks>
                    /// None
                    ///</remarks>
                    ///

                    var saleQuery = sale.AsQueryable();

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

                    ViewBag.SortCodeParameter = string.IsNullOrEmpty(sortBy) ? "transCode desc" : "";
                    ViewBag.SortSupplierParameter = string.IsNullOrEmpty(sortBy) ? "supplier desc" : "";
                    ViewBag.SortItemParameter = string.IsNullOrEmpty(sortBy) ? "description desc" : "";

                    switch (sortBy)
                    {

                        case "transCode desc": //order by supplier descending
                            saleQuery = saleQuery.OrderByDescending(x => x.transCode);
                            break;

                        case "description desc": //order by description descending
                            saleQuery = saleQuery.OrderByDescending(x => x.description);
                            break;

                        case "description": //order by description ascending
                            saleQuery = saleQuery.OrderBy(x => x.description);
                            break;

                        default: //default order by supplier ascending
                            saleQuery = saleQuery.OrderBy(x => x.supplier);
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

                    if (search != null && saleQuery != null)
                    {

                        return View(sale.Where(x => x.transCode == search).ToList().ToPagedList(page ?? 1, 10));

                    }

                    if (searchName != null)
                    {

                        return View(sale.Where(p => p.description.ToUpper().Contains(searchName.ToUpper())).ToList().ToPagedList(page ?? 1, 10));
                    }

                    if (saleQuery != null)
                    {
                        return View(saleQuery.ToList().ToPagedList(page ?? 1, 20));
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

                var retrieve = from s in db.Sales select s;
                if (retrieve.Any())
                {

                }
                else
                {
                    ViewBag.Message = "There are no sale items to display!";
                }

                var sales = from s in db.Sales select s;
                var saleQuery = sales.AsQueryable();

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
                        saleQuery = saleQuery.OrderByDescending(x => x.supplier);
                        break;

                    case "supplier": //order by supplier ascending
                        saleQuery = saleQuery.OrderBy(x => x.supplier);
                        break;

                    case "description desc": //order by description descending
                        saleQuery = saleQuery.OrderByDescending(x => x.description);
                        break;

                    case "description": //order by description ascending
                        saleQuery = saleQuery.OrderBy(x => x.description);
                        break;

                    default: //default order by supplier ascending
                        saleQuery = saleQuery.OrderBy(x => x.supplier);
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
                if (search != null && saleQuery != null)
                {
                    return View(db.Sales.Where(x => x.transCode == search).ToList().ToPagedList(page ?? 1, 10));
                }

                if (searchName != null)
                {
                    return View(db.Sales.Where(p => p.description.ToUpper().Contains(searchName.ToUpper())).ToList().ToPagedList(page ?? 1, 10));
                }

                if (searchSupplier != null)
                {
                    return View(db.Sales.Where(p => p.supplier.ToUpper().Contains(searchSupplier.ToUpper())).ToList().ToPagedList(page ?? 1, 10));
                }

                else
                {
                    return View(saleQuery.ToList().ToPagedList(page ?? 1, 20));
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
        // GET: /Sale/Details/5

        public ActionResult Details(int id = 0)
        {
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        //
        // GET: /Sale/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Sale/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Sales.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sale);
        }

        //
        // GET: /Sale/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        //
        // POST: /Sale/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sale);
        }

        //
        // GET: /Sale/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        //
        // POST: /Sale/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            db.Sales.Remove(sale);
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