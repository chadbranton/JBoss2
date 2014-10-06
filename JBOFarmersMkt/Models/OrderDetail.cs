using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Models
{
    public class OrderDetail
    {
        public int orderDetailId { get; set; }
        public int orderId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
        public Product product { get; set; }
        public Order order { get; set; }

    }
}