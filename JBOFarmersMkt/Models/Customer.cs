using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int customerId { get; set; }

        [DisplayName("POS Code")]
        public int customerCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string account { get; set; }
        public string notes { get; set; }
        public string username { get; set; }
       
    }
}