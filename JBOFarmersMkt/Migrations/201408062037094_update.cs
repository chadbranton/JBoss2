namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "Profile_UserId", "dbo.UserProfile");
            DropIndex("dbo.Customers", new[] { "Profile_UserId" });
            AddColumn("dbo.UserProfile", "Customer_customerId", c => c.Int());
            AddForeignKey("dbo.UserProfile", "Customer_customerId", "dbo.Customers", "customerId");
            CreateIndex("dbo.UserProfile", "Customer_customerId");
            DropColumn("dbo.Customers", "Profile_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Profile_UserId", c => c.Int());
            DropIndex("dbo.UserProfile", new[] { "Customer_customerId" });
            DropForeignKey("dbo.UserProfile", "Customer_customerId", "dbo.Customers");
            DropColumn("dbo.UserProfile", "Customer_customerId");
            CreateIndex("dbo.Customers", "Profile_UserId");
            AddForeignKey("dbo.Customers", "Profile_UserId", "dbo.UserProfile", "UserId");
        }
    }
}
