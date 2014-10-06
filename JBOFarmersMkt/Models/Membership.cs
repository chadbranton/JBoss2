using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JBOFarmersMkt.Context
{
    public class Membership
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Member ID")]
        public int memberID { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "Amount")]
        public int amount { get; set; }
        [Required(ErrorMessage = " Please provide your credit card number", AllowEmptyStrings = false)]
        [Display(Name = "Card Number")]
        public string cardnumber { get; set; }
        [Required(ErrorMessage = " Please provide card expiry date and year", AllowEmptyStrings = false)]
        [Display(Name = "Expires On")]
        public int expirymonth { get; set; }
        [Display(Name = "Expiry Year")]
        public int expiryyear { get; set; }
        [Required(ErrorMessage = " Please provide security code", AllowEmptyStrings = false)]
        [Display(Name = "Security Code")]
        public int securitycode { get; set; }
        [Required(ErrorMessage = " Please provide card holder's name", AllowEmptyStrings = false)]
        [Display(Name = "Card Holder's Name")]
        public string cardholdersname { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime? startdate { get; set; }
        //public string startdate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime? enddate { get; set; }
        //public string enddate { get; set; }

        //public string selectMembership { get; set; }
        public Membership()
        {
            startdate = DateTime.Today;
            enddate = DateTime.Today.AddDays(365);
        }
    }
}