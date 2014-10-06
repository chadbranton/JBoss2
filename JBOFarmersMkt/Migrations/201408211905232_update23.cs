namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update23 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transactions", "modifiers");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "modifiers", c => c.Int(nullable: false));
        }
    }
}
