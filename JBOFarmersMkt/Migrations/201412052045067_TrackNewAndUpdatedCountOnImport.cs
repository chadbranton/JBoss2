namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackNewAndUpdatedCountOnImport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Imports", "newRecords", c => c.Int(nullable: false));
            AddColumn("dbo.Imports", "updatedRecords", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Imports", "updatedRecords");
            DropColumn("dbo.Imports", "newRecords");
        }
    }
}
