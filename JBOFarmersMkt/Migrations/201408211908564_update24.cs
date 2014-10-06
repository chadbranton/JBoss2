namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update24 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transactions", "date", c => c.DateTime(nullable: false));
        }
    }
}
