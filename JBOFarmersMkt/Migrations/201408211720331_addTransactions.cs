namespace JBOFarmersMkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTransactions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        transactionId = c.Int(nullable: false, identity: true),
                        transactionCode = c.Int(nullable: false),
                        saleDate = c.DateTime(nullable: false),
                        custId = c.Int(nullable: false),
                        register = c.Int(nullable: false),
                        totalPrice = c.Double(nullable: false),
                        discount = c.Double(nullable: false),
                        newLiability = c.Double(nullable: false),
                        tax = c.Double(nullable: false),
                        total = c.Double(nullable: false),
                        tendered = c.Double(nullable: false),
                        changeReturned = c.Double(nullable: false),
                        paymentType = c.String(),
                        cardType = c.String(),
                        nameOnCard = c.String(),
                        cardLastFour = c.String(),
                        receiptNumber = c.String(),
                        bankAuth = c.String(),
                    })
                .PrimaryKey(t => t.transactionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transactions");
        }
    }
}
