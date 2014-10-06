namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update211 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "type", c => c.String());
            AddColumn("dbo.Transactions", "storeCode", c => c.String());
            AddColumn("dbo.Transactions", "description", c => c.String());
            AddColumn("dbo.Transactions", "category", c => c.String());
            AddColumn("dbo.Transactions", "department", c => c.String());
            AddColumn("dbo.Transactions", "supplier", c => c.String());
            AddColumn("dbo.Transactions", "cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transactions", "price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transactions", "quantity", c => c.Double(nullable: false));
            AddColumn("dbo.Transactions", "modifiers", c => c.Int(nullable: false));
            AddColumn("dbo.Transactions", "subtotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transactions", "cashier", c => c.String());
            AddColumn("dbo.Transactions", "date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Transactions", "register", c => c.String());
            DropColumn("dbo.Transactions", "saleDate");
            DropColumn("dbo.Transactions", "custId");
            DropColumn("dbo.Transactions", "totalPrice");
            DropColumn("dbo.Transactions", "newLiability");
            DropColumn("dbo.Transactions", "tendered");
            DropColumn("dbo.Transactions", "changeReturned");
            DropColumn("dbo.Transactions", "paymentType");
            DropColumn("dbo.Transactions", "cardType");
            DropColumn("dbo.Transactions", "nameOnCard");
            DropColumn("dbo.Transactions", "cardLastFour");
            DropColumn("dbo.Transactions", "receiptNumber");
            DropColumn("dbo.Transactions", "bankAuth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "bankAuth", c => c.String());
            AddColumn("dbo.Transactions", "receiptNumber", c => c.String());
            AddColumn("dbo.Transactions", "cardLastFour", c => c.String());
            AddColumn("dbo.Transactions", "nameOnCard", c => c.String());
            AddColumn("dbo.Transactions", "cardType", c => c.String());
            AddColumn("dbo.Transactions", "paymentType", c => c.String());
            AddColumn("dbo.Transactions", "changeReturned", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transactions", "tendered", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transactions", "newLiability", c => c.Double(nullable: false));
            AddColumn("dbo.Transactions", "totalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transactions", "custId", c => c.Int(nullable: false));
            AddColumn("dbo.Transactions", "saleDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Transactions", "register", c => c.Int(nullable: false));
            DropColumn("dbo.Transactions", "date");
            DropColumn("dbo.Transactions", "cashier");
            DropColumn("dbo.Transactions", "subtotal");
            DropColumn("dbo.Transactions", "modifiers");
            DropColumn("dbo.Transactions", "quantity");
            DropColumn("dbo.Transactions", "price");
            DropColumn("dbo.Transactions", "cost");
            DropColumn("dbo.Transactions", "supplier");
            DropColumn("dbo.Transactions", "department");
            DropColumn("dbo.Transactions", "category");
            DropColumn("dbo.Transactions", "description");
            DropColumn("dbo.Transactions", "storeCode");
            DropColumn("dbo.Transactions", "type");
        }
    }
}
