namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedPaymentnametoWeeklyPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Days", "WeeklyPayment", c => c.Single(nullable: false));
            DropColumn("dbo.Days", "Payment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Days", "Payment", c => c.Single(nullable: false));
            DropColumn("dbo.Days", "WeeklyPayment");
        }
    }
}
