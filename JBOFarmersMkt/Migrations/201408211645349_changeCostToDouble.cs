namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeCostToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Returns", "quantity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Returns", "quantity", c => c.Int(nullable: false));
        }
    }
}
