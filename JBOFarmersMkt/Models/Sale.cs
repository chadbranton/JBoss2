﻿using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt
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
        public string department { get; set; }
        public string category { get; set; }
        public string upc { get; set; }
        public string storeCode { get; set; }

        [DataType(DataType.Currency)]
        public decimal unitPrice { get; set; }

        [DisplayName("Qty")]
        public double quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal totalPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal discount { get; set; }

        [DisplayName("Total")]

        [DataType(DataType.Currency)]
        public decimal total { get; set; }

        [DataType(DataType.Currency)]
        public decimal cost { get; set; }

        public int register { get; set; }

        [DisplayName("Supplier")]
        public string supplier { get; set; }



    }

    public class SaleView
    {
        //public string saleId { get; set; }
        public string transCode { get; set; }
        public string date { get; set; }
        public string custID { get; set; }
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

    public sealed class SalesClassMap : CsvClassMap<SaleView>
    {
        public SalesClassMap()
        {
            Map(m => m.transCode).Index(0);
            Map(m => m.date).Index(1);
            Map(m => m.custID).Index(2);
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