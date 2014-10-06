namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateReturns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Returns", "totalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Returns", "discount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Returns", "discount");
            DropColumn("dbo.Returns", "totalPrice");
        }
    }
}
