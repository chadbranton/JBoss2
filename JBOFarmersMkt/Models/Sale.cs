using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Models
{
    public class Sale
    {
        public int saleId { get; set; }

        [DisplayName("Code")]
        public int transCode { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime? date { get; set; }

        [DisplayName("Customer ID")]
        public int custId { get; set; }

        [DisplayName("Item")]
        public string description { get; set; }
        [DisplayName("Department")]
        public string department { get; set; }

        public string category { get; set; }

        [DisplayName("UPC")]
        public string upc { get; set; }
        [DisplayName("Store Code")]
        public string storeCode { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Unit Price")]
        public decimal unitPrice { get; set; }

        [DisplayName("Quantity")]
        public double quantity { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Unit Price After Discount")]
        public decimal totalPrice { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Discount")]
        public decimal discount { get; set; }

        [DisplayName("Total")]
        [DataType(DataType.Currency)]
        public decimal total { get; set; }

        [DataType(DataType.Currency)]
        public decimal cost { get; set; }

        [DisplayName("Register")]
        public int register { get; set; }

        [DisplayName("Supplier")]
        public string supplier { get; set; }



    }

    public sealed class SalesClassMap : CsvClassMap<Sale>
    {
        public SalesClassMap()
        {
            Map(m => m.transCode).Index(0);
            Map(m => m.date).Index(1);
            Map(m => m.custId).Index(2);
            Map(m => m.description).Index(3);
            Map(m => m.department).Index(4);
            Map(m => m.category).Index(5);
            Map(m => m.upc).Index(6);
            Map(m => m.storeCode).Index(7);
            Map(m => m.unitPrice).Index(8);
            Map(m => m.quantity).Index(9);
            Map(m => m.totalPrice).Index(10);
            Map(m => m.discount).Index(11);
            Map(m => m.total).Index(12);
            Map(m => m.cost).Index(13);
            Map(m => m.register).Index(14);
            Map(m => m.supplier).Index(15);
        }
    }
}