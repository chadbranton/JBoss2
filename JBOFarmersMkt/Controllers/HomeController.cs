using JBOFarmersMkt.Context;
using JBOFarmersMkt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JBOFarmersMkt.Controllers
{
    public class HomeController : Controller
    {
        JBOContext context = new JBOContext();

        public ActionResult Index()
        {
            ViewBag.Message = "View Sales for a specific Grower or Supplier.";
            var products = GetTopSellingProducts(5);
            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private List<Product> GetTopSellingProducts(int count)
        {
            return context.Products
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }
    }
}
