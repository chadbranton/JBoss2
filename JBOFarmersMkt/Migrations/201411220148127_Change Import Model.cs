namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeImportModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Imports", "type", c => c.Int(nullable: false));
            AddColumn("dbo.Imports", "contentHash", c => c.String());
            AddColumn("dbo.Imports", "CreatedBy", c => c.String());
            AddColumn("dbo.Imports", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Imports", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Imports", "LastModifiedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Imports", "LastModifiedAt");
            DropColumn("dbo.Imports", "LastModifiedBy");
            DropColumn("dbo.Imports", "CreatedAt");
            DropColumn("dbo.Imports", "CreatedBy");
            DropColumn("dbo.Imports", "contentHash");
            DropColumn("dbo.Imports", "type");
        }
    }
}
