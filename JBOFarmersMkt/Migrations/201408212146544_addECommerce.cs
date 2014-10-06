namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addECommerce : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        orderId = c.Int(nullable: false, identity: true),
                        orderDate = c.DateTime(nullable: false),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        user_UserId = c.Int(),
                        customer_customerId = c.Int(),
                    })
                .PrimaryKey(t => t.orderId)
                .ForeignKey("dbo.UserProfile", t => t.user_UserId)
                .ForeignKey("dbo.Customers", t => t.customer_customerId)
                .Index(t => t.user_UserId)
                .Index(t => t.customer_customerId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        orderDetailId = c.Int(nullable: false, identity: true),
                        orderId = c.Int(nullable: false),
                        productId = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                        unitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.orderDetailId)
                .ForeignKey("dbo.Products", t => t.productId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.orderId, cascadeDelete: true)
                .Index(t => t.productId)
                .Index(t => t.orderId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        cartId = c.String(),
                        productId = c.Int(nullable: false),
                        count = c.Int(nullable: false),
                        dateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Products", t => t.productId, cascadeDelete: true)
                .Index(t => t.productId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Carts", new[] { "productId" });
            DropIndex("dbo.OrderDetails", new[] { "orderId" });
            DropIndex("dbo.OrderDetails", new[] { "productId" });
            DropIndex("dbo.Orders", new[] { "customer_customerId" });
            DropIndex("dbo.Orders", new[] { "user_UserId" });
            DropForeignKey("dbo.Carts", "productId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "orderId", "dbo.Orders");
            DropForeignKey("dbo.OrderDetails", "productId", "dbo.Products");
            DropForeignKey("dbo.Orders", "customer_customerId", "dbo.Customers");
            DropForeignKey("dbo.Orders", "user_UserId", "dbo.UserProfile");
            DropTable("dbo.Carts");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
        }
    }
}
