namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecustomeraddUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "profile_UserId", c => c.Int());
            CreateIndex("dbo.Customers", "profile_UserId");
            AddForeignKey("dbo.Customers", "profile_UserId", "dbo.UserProfile", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "profile_UserId", "dbo.UserProfile");
            DropIndex("dbo.Customers", new[] { "profile_UserId" });
            DropColumn("dbo.Customers", "profile_UserId");
        }
    }
}
