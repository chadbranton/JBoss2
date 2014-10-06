using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Models
{
    public class Order
    {
        [ScaffoldColumn(false)]
        public int orderId { get; set; }
        public DateTime orderDate { get; set; }        
        public Customer customer { get; set; }
        public decimal total { get; set; }
        public List<OrderDetail> orderDetails { get; set; }

    }
}