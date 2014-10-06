using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JBOFarmersMkt.Context;
using JBOFarmersMkt.Models;

namespace JBOFarmersMkt.Controllers
{
    public class CheckoutController : Controller
    {
        JBOContext context = new JBOContext();
        const string PromoCode = "FREE";

        //
        // GET: /Checkout/

        [Authorize]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();

            var customer = context.Customers.SingleOrDefault(c => c.username == User.Identity.Name);
                  

            if (customer != null)
            {
                order.orderDate = DateTime.Now;
                order.customer = customer;

                try
                {
                    if (string.Equals(values["PromoCode"], PromoCode,
                        StringComparison.OrdinalIgnoreCase) == false)
                    {
                        return View(order);
                    }
                    else
                    {
                        //order.user.UserName = User.Identity.Name;
                        order.orderDate = DateTime.Now;

                        //save order
                        context.Orders.Add(order);
                        context.SaveChanges();

                        //process order
                        var cart = ShoppingCart.getCart(this.HttpContext);


                        var products = from i in cart.GetCartItems()
                                       select i.productId;

                        foreach (var i in products)
                        {
                            var item = cart.getSale(i);
                            context.Sales.Add(item);
                            context.SaveChanges();
                        }

                        cart.CreateOrder(order);

                        return RedirectToAction("Complete",
                            new { id = order.orderId });
                    }

                }

                catch
                {
                    return View(order);
                }
            }
            else
            {
                TryUpdateModel(order);

                try
                {
                    if (string.Equals(values["PromoCode"], PromoCode,
                        StringComparison.OrdinalIgnoreCase) == false)
                    {
                        return View(order);
                    }
                    else
                    {
                        order.orderDate = DateTime.Now;


                        //save order
                        context.Orders.Add(order);
                        context.SaveChanges();

                        //process order
                        var cart = ShoppingCart.getCart(this.HttpContext);


                        var products = from i in cart.GetCartItems()
                                       select i.productId;

                        foreach (var i in products)
                        {
                            var item = cart.getSale(i);
                            context.Sales.Add(item);
                            context.SaveChanges();
                        }

                        cart.CreateOrder(order);

                        return RedirectToAction("Complete",
                            new { id = order.orderId });

                    }
                }
                catch
                {
                    return View(order);
                }
                return View();
                
            }
        }

        public ActionResult Complete(int id)
        {
            //validate customer that owns this order
            bool isValid = context.Orders.Any(
                o => o.orderId == id &&
                o.customer.username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

    }
}
