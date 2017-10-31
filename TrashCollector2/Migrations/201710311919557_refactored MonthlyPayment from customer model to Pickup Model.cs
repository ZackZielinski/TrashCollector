namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactoredMonthlyPaymentfromcustomermodeltoPickupModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pickups", "MonthlyPayment", c => c.Single(nullable: false));
            DropColumn("dbo.Customers", "MonthlyPayment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "MonthlyPayment", c => c.Single(nullable: false));
            DropColumn("dbo.Pickups", "MonthlyPayment");
        }
    }
}
