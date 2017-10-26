namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingforeignkeysforcontrollercreation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "DayId", "dbo.Days");
            DropForeignKey("dbo.Customers", "Userid", "dbo.AspNetUsers");
            DropIndex("dbo.Customers", new[] { "Userid" });
            DropIndex("dbo.Customers", new[] { "DayId" });
            DropColumn("dbo.Customers", "Userid");
            DropColumn("dbo.Customers", "DayId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "DayId", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "Userid", c => c.String(maxLength: 128));
            CreateIndex("dbo.Customers", "DayId");
            CreateIndex("dbo.Customers", "Userid");
            AddForeignKey("dbo.Customers", "Userid", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Customers", "DayId", "dbo.Days", "Id", cascadeDelete: true);
        }
    }
}
