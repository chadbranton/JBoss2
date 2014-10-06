﻿using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Models
{
    public class Product
    {
        [ScaffoldColumn(false)]
        public int productId { get; set; }

        [DisplayName("Item No.")]
        public int productCode { get; set; }

        [DisplayName("Item")]
        public string description { get; set; }
        public string department { get; set; }
        public string category { get; set; }
        public string upc { get; set; }
        public string storeCode { get; set; }

        [DisplayName("Price")]        
        public decimal unitPrice { get; set; }

        public Boolean discountable { get; set; }

        public Boolean taxable { get; set; }

        public string inventoryMethod { get; set; }       

        [DisplayName("Qty")]
        public double quantity { get; set; }

        public int orderTrigger { get; set; }

        public int recommendedOrder { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public string lastSoldDate { get; set; }

        [DisplayName("Supplier")]
        public string supplier { get; set; }

        public string liabilityItem { get; set; }

        public string LRT { get; set; }

        [DisplayName("Product Image URL")]
        [StringLength(1024)]
        public string ProductImageUrl { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }

    }

    public class ProductView
    {
        // public string productId { get; set; }        
        public string productCode { get; set; }
        public string description { get; set; }
        public string department { get; set; }
        public string category { get; set; }
        public string upc { get; set; }
        public string storeCode { get; set; }
        public string unitPrice { get; set; }
        public string discountable { get; set; }
        public string taxable { get; set; }
        public string inventoryMethod { get; set; }        
        public string assignedCost { get; set; }
        public string quantity { get; set; }
        public string orderTrigger { get; set; }
        public string recommendedOrder { get; set; }
        public string lastSoldDate { get; set; }
        public string supplier { get; set; }
        public string liabilityItem { get; set; }
        public string LRT { get; set; }
    }
  
    public sealed class ProductClassMap : CsvClassMap<ProductView>
    {
        public ProductClassMap()
        {
            Map(m => m.productCode).Index(0);
            Map(m => m.description).Index(1);
            Map(m => m.department).Index(2);
            Map(m => m.category).Index(3);
            Map(m => m.upc).Index(4);
            Map(m => m.storeCode).Index(5);
            Map(m => m.unitPrice).Index(6);
            Map(m => m.discountable).Index(7);
            Map(m => m.taxable).Index(8);
            Map(m => m.inventoryMethod).Index(9);                      
            Map(m => m.quantity).Index(12);
            Map(m => m.orderTrigger).Index(13);
            Map(m => m.recommendedOrder).Index(14);
            Map(m => m.lastSoldDate).Index(15);
            Map(m => m.supplier).Index(16);
            Map(m => m.liabilityItem).Index(17);
            Map(m => m.LRT).Index(18);
        }
    }
}

