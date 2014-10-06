namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "user_UserId", "dbo.UserProfile");
            DropIndex("dbo.Orders", new[] { "user_UserId" });
            DropColumn("dbo.Orders", "user_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "user_UserId", c => c.Int());
            CreateIndex("dbo.Orders", "user_UserId");
            AddForeignKey("dbo.Orders", "user_UserId", "dbo.UserProfile", "UserId");
        }
    }
}
