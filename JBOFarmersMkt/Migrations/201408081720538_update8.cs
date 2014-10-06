namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "cost", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "cost", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
