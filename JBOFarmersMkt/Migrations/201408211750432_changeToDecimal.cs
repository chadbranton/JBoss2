namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeToDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "totalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Transactions", "discount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Transactions", "tax", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Transactions", "total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Transactions", "tendered", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Transactions", "changeReturned", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transactions", "changeReturned", c => c.Double(nullable: false));
            AlterColumn("dbo.Transactions", "tendered", c => c.Double(nullable: false));
            AlterColumn("dbo.Transactions", "total", c => c.Double(nullable: false));
            AlterColumn("dbo.Transactions", "tax", c => c.Double(nullable: false));
            AlterColumn("dbo.Transactions", "discount", c => c.Double(nullable: false));
            AlterColumn("dbo.Transactions", "totalPrice", c => c.Double(nullable: false));
        }
    }
}
