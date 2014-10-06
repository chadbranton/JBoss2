using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JBOFarmersMkt.Context;

namespace JBOFarmersMkt.Models
{
    public partial class ShoppingCart
    {

        JBOContext context = new JBOContext();

        [Key]
        string shoppingCartId { get; set; }

        public const string CartSessionKey = "CardId";

        public static ShoppingCart getCart(HttpContextBase con)
        {
            var cart = new ShoppingCart();
            cart.shoppingCartId = cart.getCartId(con);
            return cart;
        }

        public static ShoppingCart getCart(Controller controller)
        {
            return getCart(controller.HttpContext);
        }

        public void addToCart(Product product)
        {
            var cartItem = context.Carts.SingleOrDefault(
                c => c.cartId == shoppingCartId && c.productId == product.productId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    productId = product.productId,
                    cartId = shoppingCartId,
                    count = 1,
                    dateCreated = DateTime.Now
                };

                context.Carts.Add(cartItem);
            }
            else
            {
                cartItem.count++;
            }

            context.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            //Get Cart
            var cartItem = context.Carts.Single(
                cart => cart.cartId == shoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.count > 1)
                {
                    cartItem.count--;
                    itemCount = cartItem.count;
                }
                else
                {
                    context.Carts.Remove(cartItem);
                }

                context.SaveChanges();
            }

            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = context.Carts.Where(cart => cart.cartId == shoppingCartId);

            foreach (var cartItem in cartItems)
            {
                context.Carts.Remove(cartItem);
            }

            context.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return context.Carts.Where(cart => cart.cartId == shoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from cartItems in context.Carts
                          where cartItems.cartId == shoppingCartId
                          select (int?)cartItems.count).Sum();
            //return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in context.Carts
                              where cartItems.cartId == shoppingCartId
                              select (int?)cartItems.count * cartItems.product.unitPrice).Sum();
            return total ?? decimal.Zero;
        }

        public Sale getSale(int id)
        {

            Product prod = (context.Products.Where(p => p.productId == id))
                                .First();

            var qty = (from q in context.Carts
                       where q.cartId == shoppingCartId && q.productId == id
                       select q.count).First();

            var total = qty * prod.unitPrice;

            Sale sale = new Sale();

            sale.transCode = 0000;
            sale.date = DateTime.Now;
            sale.custId = 0;
            sale.description = prod.description;
            sale.department = prod.department;
            sale.category = null;
            sale.upc = null;
            sale.storeCode = null;
            sale.unitPrice = prod.unitPrice;
            sale.quantity = qty;
            sale.totalPrice = prod.unitPrice;
            sale.discount = 0;
            sale.total = total;
            sale.cost = 0;
            sale.register = 0;
            sale.supplier = prod.supplier;

            return sale;

        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            //iterate over items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var OrderDetail = new OrderDetail
                {
                    productId = item.productId,
                    orderId = order.orderId,
                    unitPrice = item.product.unitPrice,
                    quantity = item.count
                };

                // set the order total of the shopping cart
                orderTotal += (item.count * item.product.unitPrice);
                context.OrderDetails.Add(OrderDetail);
            }
            //set the order's total to the orderTotal count
            order.total = orderTotal;

            //save to database
            context.SaveChanges();

            //Empty the shopping cart
            EmptyCart();

            //return the order as confirmation number
            return order.orderId;
        }

        //allows access to cookies
        public string getCartId(HttpContextBase con)
        {
            if (con.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(con.User.Identity.Name))
                {
                    con.Session[CartSessionKey] = con.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    con.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return con.Session[CartSessionKey].ToString();
        }

        //When a user has logged in, migrate their shopping cart to
        //be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = context.Carts.Where(c => c.cartId == shoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.cartId = userName;
            }

            context.SaveChanges();
        }
    }

}