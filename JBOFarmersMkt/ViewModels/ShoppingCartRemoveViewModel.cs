using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.ViewModels
{
    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public decimal cartTotal { get; set; }
        public int cartCount { get; set; }
        public int itemCount { get; set; }
        public int deleteId { get; set; }

    }
}