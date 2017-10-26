namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class placedbackcustomerforeignkeys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Userid", c => c.String(maxLength: 128));
            AddColumn("dbo.Customers", "DayId", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "Userid");
            CreateIndex("dbo.Customers", "DayId");
            AddForeignKey("dbo.Customers", "DayId", "dbo.Days", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Customers", "Userid", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "Userid", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "DayId", "dbo.Days");
            DropIndex("dbo.Customers", new[] { "DayId" });
            DropIndex("dbo.Customers", new[] { "Userid" });
            DropColumn("dbo.Customers", "DayId");
            DropColumn("dbo.Customers", "Userid");
        }
    }
}
