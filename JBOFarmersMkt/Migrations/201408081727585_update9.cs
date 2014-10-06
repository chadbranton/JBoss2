namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update9 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "cost", c => c.Double());
        }
    }
}
