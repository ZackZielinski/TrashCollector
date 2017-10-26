namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedacustomerpaymentcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "MonthlyPayment", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "MonthlyPayment");
        }
    }
}
