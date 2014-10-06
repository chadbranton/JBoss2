using JBOFarmersMkt.Context;
using JBOFarmersMkt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JBOFarmersMkt.Controllers
{
    public class StoreController : Controller
    {
        JBOContext context = new JBOContext();
        //
        // GET: /Store/

        public ActionResult Index()
        {
            var departments = from d in context.Departments select d;
            return View(departments);
        }

        [HttpGet]
        public ActionResult Browse(string dept)
        {
            ViewBag.Department = dept;
            var products = from p in context.Products where p.department == dept select p;
            return View(products);
        }

        public ActionResult Details(int id = 0)
        {
            Product product = context.Products.Find(id);
            return View(product);
        }

        [ChildActionOnly]
        public ActionResult DeptMenu()
        {
            var depts = context.Departments.ToList();

            return PartialView(depts);
        }

    }
}
