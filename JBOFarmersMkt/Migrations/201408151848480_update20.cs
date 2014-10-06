namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update20 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfile", "UserProfile_UserId", "dbo.UserProfile");
            DropIndex("dbo.UserProfile", new[] { "UserProfile_UserId" });
            DropColumn("dbo.UserProfile", "UserProfile_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfile", "UserProfile_UserId", c => c.Int());
            CreateIndex("dbo.UserProfile", "UserProfile_UserId");
            AddForeignKey("dbo.UserProfile", "UserProfile_UserId", "dbo.UserProfile", "UserId");
        }
    }
}
