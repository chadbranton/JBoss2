using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string cartId { get; set; }
        public int productId { get; set; }
        public int count { get; set; }
        public DateTime dateCreated { get; set; }
        public virtual Product product { get; set; }
    }
}