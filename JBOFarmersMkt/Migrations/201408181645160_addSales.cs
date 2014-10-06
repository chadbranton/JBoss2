namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        saleId = c.Int(nullable: false, identity: true),
                        transCode = c.Int(nullable: false),
                        date = c.DateTime(),
                        custId = c.Int(nullable: false),
                        description = c.String(),
                        department = c.String(),
                        category = c.String(),
                        upc = c.String(),
                        storeCode = c.String(),
                        unitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        quantity = c.Double(nullable: false),
                        totalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        register = c.Int(nullable: false),
                        supplier = c.String(),
                    })
                .PrimaryKey(t => t.saleId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sales");
        }
    }
}
