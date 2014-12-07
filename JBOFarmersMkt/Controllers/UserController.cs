using JBOFarmersMkt.Context;
using JBOFarmersMkt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JBOFarmersMkt.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        
        JBOContext context = new JBOContext();

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var users = from user in context.UserProfiles select user;

            return View(users);
        }

        public ActionResult assignRole(int id = 0)
        {
            
            SelectList list = new SelectList(Roles.GetAllRoles());
            ViewBag.Roles = list;            
            UserProfile user = context.UserProfiles.Find(id);
            SelectList roleList = new SelectList(Roles.GetRolesForUser(user.UserName));
            ViewBag.roleList = roleList;

            if (user == null)
            {
                return HttpNotFound();
            }           

            return View(user);
        }

        [HttpPost]
        public ActionResult assignRole(int id, FormCollection form)
        {

            UserProfile user = context.UserProfiles.Find(id);

            
            Roles.AddUserToRole(user.UserName, form["RoleListBox"].ToString());
           

            return RedirectToAction("assignRole");
        }

    }

    
}
