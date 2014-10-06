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
    public class ReturnController : Controller
    {
        private JBOContext db = new JBOContext();

        //
        // GET: /Return/
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

                    var returns = from r in db.Returns
                               where supplier.Contains(r.supplier)
                               select r;


                    ///<summary>
                    ///
                    /// return is passed to salesQuery as a Queryable object 
                    /// 
                    ///</summary>
                    ///<remarks>
                    /// None
                    ///</remarks>
                    ///

                    var returnQuery = returns.AsQueryable();

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
                            returnQuery = returnQuery.OrderByDescending(x => x.returnId);
                            break;

                        case "description desc": //order by description descending
                            returnQuery = returnQuery.OrderByDescending(x => x.description);
                            break;

                        case "description": //order by description ascending
                            returnQuery = returnQuery.OrderBy(x => x.description);
                            break;

                        default: //default order by supplier ascending
                            returnQuery = returnQuery.OrderBy(x => x.supplier);
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

                    if (search != null && returnQuery != null)
                    {

                        return View(returns.Where(x => x.returnId == search).ToList().ToPagedList(page ?? 1, 10));

                    }

                    if (searchName != null)
                    {

                        return View(returns.Where(p => p.description.ToUpper().Contains(searchName.ToUpper())).ToList().ToPagedList(page ?? 1, 10));
                    }

                    if (returnQuery != null)
                    {
                        return View(returnQuery.ToList().ToPagedList(page ?? 1, 20));
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

                var retrieve = from r in db.Returns select r;
                if (retrieve.Any())
                {

                }
                else
                {
                    ViewBag.Message = "There are no return items to display!";
                }

                var returns = from r in db.Returns select r;
                var saleQuery = returns.AsQueryable();

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
        // GET: /Return/Details/5

        public ActionResult Details(int id = 0)
        {
            Return returns = db.Returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            return View(returns);
        }

        //
        // GET: /Return/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Return/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Return returns)
        {
            if (ModelState.IsValid)
            {
                db.Returns.Add(returns);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(returns);
        }

        //
        // GET: /Return/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Return returns = db.Returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            return View(returns);
        }

        //
        // POST: /Return/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Return returns)
        {
            if (ModelState.IsValid)
            {
                db.Entry(returns).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(returns);
        }

        //
        // GET: /Return/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Return returns = db.Returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            return View(returns);
        }

        //
        // POST: /Return/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Return returns = db.Returns.Find(id);
            db.Returns.Remove(returns);
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