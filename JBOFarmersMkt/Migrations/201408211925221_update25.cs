namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update25 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "transactionCode", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transactions", "transactionCode", c => c.Int(nullable: false));
        }
    }
}
