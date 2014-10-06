using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JBOFarmersMkt.Models;
using JBOFarmersMkt.Context;

namespace JBOFarmersMkt.Controllers
{
    public class SupplierController : Controller
    {
        private JBOContext db = new JBOContext();

        //
        // GET: /Supplier/

        public ActionResult Index()
        {
            return View(db.Suppliers.ToList());
        }

        //
        // GET: /Supplier/Details/5

        public ActionResult Details(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);

            var users = supplier.users.Select(u => u.UserName);

            ViewBag.users = users.ToList();
            
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // GET: /Supplier/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Supplier/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        //
        // GET: /Supplier/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // POST: /Supplier/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        public ActionResult Users(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);
            var users = from u in db.UserProfiles where u.Supplier.supplierID == id select u;
            TempData["supplier"] = supplier.name;

            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users.ToList());
        }

        public ActionResult addUser(int id)
        {

            Supplier supplier = db.Suppliers.Find(id);
            TempData["supplier"] = supplier.name;
            return View(db.UserProfiles.ToList());
        }

        public ActionResult addAssignment(string supp)
        {
            Supplier supplier = db.Suppliers.Where(s => s.name == supp).Single();
            TempData["supplier"] = supplier.name;

            var users = from u in db.UserProfiles select u;

            return View(users.ToList());
        }

        
        public ActionResult completeAssignment(string supp, int? id)
        {
            Supplier supplier = db.Suppliers.Where(s => s.name == supp).Single();

            if (supplier == null)
            {
                return HttpNotFound();
            }


            var userProfile = db.UserProfiles.Where(u => u.UserId == id).Single();
            supplier.users.Add(userProfile);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult deleteAssignment(int id, string supp)
        {
            Supplier supplier = db.Suppliers.Where(s => s.name == supp).Single();

            if (supplier == null)
            {
                return HttpNotFound();
            }

            var userProfile = db.UserProfiles.Where(u => u.UserId == id).Single();
            supplier.users.Remove(userProfile);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: /Supplier/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // POST: /Supplier/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
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