namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Imports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        filename = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Imports");
        }
    }
}
