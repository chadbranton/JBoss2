namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "lastSoldDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "lastSoldDate", c => c.String());
        }
    }
}
