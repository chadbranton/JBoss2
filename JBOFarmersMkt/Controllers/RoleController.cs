using JBOFarmersMkt.Context;
using JBOFarmersMkt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JBOFarmersMkt.Controllers
{
    public class RoleController : Controller
    {
        //
        // GET: /Role/
        JBOContext context = new JBOContext();
        public ActionResult Index()
        {
            var roles = System.Web.Security.Roles.GetAllRoles();
            ViewBag.Roles = roles;
            return View(roles);
        }

        [HttpPost]
        public ActionResult Index(string roleName)
        {
            System.Web.Security.Roles.CreateRole(roleName);
            return RedirectToAction("index");
        }

        public ActionResult AddUserRole(string user)
        {
            System.Web.Security.Roles.AddUserToRole(user, "Admin");
            return View("Index");
        }

        public ActionResult GetUsers()
        {
            var users = from i in context.Customers select i;
            return View(users);
        }

    }
}
