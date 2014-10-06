namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserProfile", name: "Customer_customerId", newName: "customerId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.UserProfile", name: "customerId", newName: "Customer_customerId");
        }
    }
}
