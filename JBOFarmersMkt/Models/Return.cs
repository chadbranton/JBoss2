using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Models
{
    public class Return
    {
        public int returnId { get; set; }
        public DateTime returnDate { get; set; }
        public int custId { get; set; }
        public string description { get; set; }
        public string department { get; set; }
        public string category { get; set; }
        public string upc { get; set; }
        public string storeCode { get; set; }
        public decimal unitPrice { get; set; }
        public double quantity { get; set; }
        public decimal totalPrice { get; set; }
        public decimal discount { get; set; }
        public decimal total { get; set; }
        public decimal cost { get; set; }
        public int register { get; set; }
        public string supplier { get; set; }

    }

    public class ReturnView
    {
        public string returnId { get; set; }
        public string returnDate { get; set; }
        public string custId { get; set; }
        public string description { get; set; }
        public string department { get; set; }
        public string category { get; set; }
        public string upc { get; set; }
        public string storeCode { get; set; }
        public string unitPrice { get; set; }
        public string quantity { get; set; }
        public string totalPrice { get; set; }
        public string discount { get; set; }
        public string total { get; set; }
        public string cost { get; set; }
        public string register { get; set; }
        public string supplier { get; set; }
    }

    public sealed class ReturnClassMap : CsvClassMap<ReturnView>
    {
        public ReturnClassMap()
        {
            Map(m => m.returnId).Index(0);
            Map(m => m.returnDate).Index(1);
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