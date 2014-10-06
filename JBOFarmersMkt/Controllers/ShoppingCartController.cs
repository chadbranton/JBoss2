using System.Linq;
using System.Web.Mvc;
using JBOFarmersMkt.Models;
using JBOFarmersMkt.ViewModels;
using JBOFarmersMkt.Context;

namespace JBOFarmersMkt.Controllers
{
    public class ShoppingCartController : Controller
    {
        JBOContext context = new JBOContext();

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            var cart = ShoppingCart.getCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                cartItems = cart.GetCartItems(),
                cartTotal = cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public ActionResult AddToCart(int id)
        {

            // Retrieve the product from the database
            var addedProduct = context.Products
                .Single(product => product.productId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.getCart(this.HttpContext);

            cart.addToCart(addedProduct);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.getCart(this.HttpContext);

            // Get the name of the product to display confirmation
            string productName = context.Carts
                .Single(item => item.RecordId == id).product.description;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.",
                cartTotal = cart.GetTotal(),
                cartCount = cart.GetCount(),
                itemCount = itemCount,
                deleteId = id
            };

            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.getCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }
    }
}
