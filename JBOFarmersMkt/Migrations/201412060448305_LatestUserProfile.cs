namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LatestUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "profile_UserId", c => c.Int());
            AddColumn("dbo.UserProfile", "firstName", c => c.String());
            AddColumn("dbo.UserProfile", "lastName", c => c.String());
            AddColumn("dbo.UserProfile", "address", c => c.String());
            AddColumn("dbo.UserProfile", "city", c => c.String());
            AddColumn("dbo.UserProfile", "state", c => c.String());
            AddColumn("dbo.UserProfile", "zip", c => c.String());
            AddColumn("dbo.UserProfile", "email", c => c.String());
            AddColumn("dbo.UserProfile", "phone", c => c.String());
            CreateIndex("dbo.Customers", "profile_UserId");
            AddForeignKey("dbo.Customers", "profile_UserId", "dbo.UserProfile", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "profile_UserId", "dbo.UserProfile");
            DropIndex("dbo.Customers", new[] { "profile_UserId" });
            DropColumn("dbo.UserProfile", "phone");
            DropColumn("dbo.UserProfile", "email");
            DropColumn("dbo.UserProfile", "zip");
            DropColumn("dbo.UserProfile", "state");
            DropColumn("dbo.UserProfile", "city");
            DropColumn("dbo.UserProfile", "address");
            DropColumn("dbo.UserProfile", "lastName");
            DropColumn("dbo.UserProfile", "firstName");
            DropColumn("dbo.Customers", "profile_UserId");
        }
    }
}
