namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReturns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Returns",
                c => new
                    {
                        returnId = c.Int(nullable: false, identity: true),
                        returnDate = c.DateTime(nullable: false),
                        custId = c.Int(nullable: false),
                        description = c.String(),
                        department = c.String(),
                        category = c.String(),
                        upc = c.String(),
                        storeCode = c.String(),
                        unitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        quantity = c.Int(nullable: false),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        register = c.Int(nullable: false),
                        supplier = c.String(),
                    })
                .PrimaryKey(t => t.returnId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Returns");
        }
    }
}
