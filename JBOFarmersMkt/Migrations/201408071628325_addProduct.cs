namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        productId = c.Int(nullable: false, identity: true),
                        productCode = c.Int(nullable: false),
                        description = c.String(),
                        department = c.String(),
                        category = c.String(),
                        upc = c.String(),
                        storeCode = c.String(),
                        unitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        discountable = c.Boolean(nullable: false),
                        taxable = c.Boolean(nullable: false),
                        inventoryMethod = c.String(),
                        cost = c.Decimal(precision: 18, scale: 2),
                        quantity = c.Double(nullable: false),
                        orderTrigger = c.Int(nullable: false),
                        recommendedOrder = c.Int(nullable: false),
                        lastSoldDate = c.String(),
                        supplier = c.String(),
                        liabilityItem = c.String(),
                        LRT = c.String(),
                        ProductImageUrl = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.productId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
        }
    }
}
