namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfile", "Customer_customerId", "dbo.Customers");
            DropIndex("dbo.UserProfile", new[] { "Customer_customerId" });
            AddColumn("dbo.Customers", "username", c => c.String());
            DropColumn("dbo.UserProfile", "Customer_customerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfile", "Customer_customerId", c => c.Int());
            DropColumn("dbo.Customers", "username");
            CreateIndex("dbo.UserProfile", "Customer_customerId");
            AddForeignKey("dbo.UserProfile", "Customer_customerId", "dbo.Customers", "customerId");
        }
    }
}
