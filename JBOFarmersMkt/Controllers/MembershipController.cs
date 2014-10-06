using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JBOFarmersMkt.Context
{
    public class MembershipController : Controller
    {
        // GET: Membership
        //JBOContext context = new JBOContext();
        private JBOContext db = new JBOContext();
        //public MembershipController(MembershipContext memdb)
        //{
        //    this.db = memdb;
        //}

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubmitForm(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                //var customeremail = context.Customers.SingleOrDefault(c => c.email == User.Identity.email);


                int memberID = Convert.ToInt32(collection["memberID"]);
                //string email = customeremail;
                string email = collection["email"].ToString();
                int amount = Convert.ToInt32(collection["amount"]);
                string cardnumber = collection["cardnumber"].ToString();
                int expirymonth = Convert.ToInt32(collection["expirymonth"]);
                int expiryyear = Convert.ToInt32(collection["expiryyear"]);
                int securitycode = Convert.ToInt32(collection["securitycode"]);
                string cardholdersname = collection["cardholdersname"].ToString();
                DateTime startdate = Convert.ToDateTime(collection["startdate"]);
                DateTime enddate = Convert.ToDateTime(collection["enddate"]);

                if (collection.AllKeys.Contains("email"))
                {
                    ViewData["contains"] = email;
                }

                if (collection.AllKeys.Contains("startdate") && collection.AllKeys.Contains("enddate"))
                {
                    TimeSpan days = enddate - startdate;
                    ViewData["daysLeft"] = days.TotalDays;
                }

                return View();
            }
            else
            {
                ///And if entries are not valid then we render that view again.
                return View("Membership", Index());
            }
        }

        //
        // GET: /Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Membership member)
        {
            if (ModelState.IsValid)
            {
                db.Memberships.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}