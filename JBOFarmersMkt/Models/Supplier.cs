using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Models
{
    public class Supplier
    {
        public int supplierID { get; set; }

        [Required]
        [Display(Name = "Supplier Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "City")]
        public string city { get; set; }

        [Required]
        [Display(Name = "State")]
        public string state { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string phone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }

        public virtual ICollection<UserProfile> users { get; set; }

        public virtual ICollection<Product> products { get; set; }      

       

    }
}