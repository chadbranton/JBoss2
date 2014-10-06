namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update6 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserProfile", name: "customerId", newName: "Customer_customerId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.UserProfile", name: "Customer_customerId", newName: "customerId");
        }
    }
}
