namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update22 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transactions", "cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
