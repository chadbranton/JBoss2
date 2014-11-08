using JBOFarmersMkt.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}
