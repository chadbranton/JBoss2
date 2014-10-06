namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSupplier : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        supplierID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        address = c.String(nullable: false),
                        city = c.String(nullable: false),
                        state = c.String(nullable: false),
                        phone = c.String(nullable: false),
                        email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.supplierID);
            
            AddColumn("dbo.UserProfile", "Supplier_supplierID", c => c.Int());
            AddColumn("dbo.Products", "Supplier_supplierID", c => c.Int());
            AddForeignKey("dbo.UserProfile", "Supplier_supplierID", "dbo.Suppliers", "supplierID");
            AddForeignKey("dbo.Products", "Supplier_supplierID", "dbo.Suppliers", "supplierID");
            CreateIndex("dbo.UserProfile", "Supplier_supplierID");
            CreateIndex("dbo.Products", "Supplier_supplierID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "Supplier_supplierID" });
            DropIndex("dbo.UserProfile", new[] { "Supplier_supplierID" });
            DropForeignKey("dbo.Products", "Supplier_supplierID", "dbo.Suppliers");
            DropForeignKey("dbo.UserProfile", "Supplier_supplierID", "dbo.Suppliers");
            DropColumn("dbo.Products", "Supplier_supplierID");
            DropColumn("dbo.UserProfile", "Supplier_supplierID");
            DropTable("dbo.Suppliers");
        }
    }
}
