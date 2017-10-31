namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdTotalPaymentcolumnincustomermodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "TotalPayment", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "TotalPayment");
        }
    }
}
