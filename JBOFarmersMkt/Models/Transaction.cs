using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Models
{
    public class Transaction
    {
        public int transactionId { get; set; }
        public int? transactionCode { get; set; }
        public string type { get; set; }
        public string storeCode { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string department { get; set; }
        public string supplier { get; set; }
        
        public decimal price { get; set; }
        public double quantity { get; set; }
        
        public decimal subtotal { get; set; }
        public decimal tax { get; set; }
        public decimal discount { get; set; }
        public decimal total { get; set; }
        public string cashier { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime? date { get; set; }
        public string register { get; set; }    



    }

    public class TransactionView
    {
        public string transactionId { get; set; }
        public string transactionCode { get; set; }
        public string type { get; set; }
        public string storeCode { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string department { get; set; }
        public string supplier { get; set; }        
        public string price { get; set; }
        public string quantity { get; set; }        
        public string subtotal { get; set; }
        public string tax { get; set; }
        public string discount { get; set; }
        public string total { get; set; }
        public string cashier { get; set; }
        public string date { get; set; }
        public string register { get; set; }

    }

    public sealed class TransactionClassMap : CsvClassMap<TransactionView>
    {
        public TransactionClassMap()
        {
            Map(m => m.transactionCode).Index(0);
            Map(m => m.type).Index(1);
            Map(m => m.storeCode).Index(2);
            Map(m => m.description).Index(3);
            Map(m => m.category).Index(4);
            Map(m => m.department).Index(5);
            Map(m => m.supplier).Index(6);            
            Map(m => m.price).Index(9);
            Map(m => m.quantity).Index(10);            
            Map(m => m.subtotal).Index(12);
            Map(m => m.tax).Index(13);
            Map(m => m.discount).Index(14);
            Map(m => m.total).Index(15);
            Map(m => m.cashier).Index(16);
            Map(m => m.date).Index(17);
            Map(m => m.register).Index(18);

        }
    }
}