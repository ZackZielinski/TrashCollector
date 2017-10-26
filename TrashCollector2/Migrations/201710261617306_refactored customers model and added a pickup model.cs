namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactoredcustomersmodelandaddedapickupmodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "DayId", "dbo.Days");
            DropIndex("dbo.Customers", new[] { "DayId" });
            CreateTable(
                "dbo.Pickups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Location = c.String(),
                        ZipCode = c.String(),
                        ActiveStatus = c.Boolean(nullable: false),
                        DayId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Days", t => t.DayId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.DayId);
            
            DropColumn("dbo.Customers", "OnVacation");
            DropColumn("dbo.Customers", "Location");
            DropColumn("dbo.Customers", "ZipCode");
            DropColumn("dbo.Customers", "DayId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "DayId", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "ZipCode", c => c.String());
            AddColumn("dbo.Customers", "Location", c => c.String());
            AddColumn("dbo.Customers", "OnVacation", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Pickups", "DayId", "dbo.Days");
            DropForeignKey("dbo.Pickups", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Pickups", new[] { "DayId" });
            DropIndex("dbo.Pickups", new[] { "CustomerId" });
            DropTable("dbo.Pickups");
            CreateIndex("dbo.Customers", "DayId");
            AddForeignKey("dbo.Customers", "DayId", "dbo.Days", "Id", cascadeDelete: true);
        }
    }
}
