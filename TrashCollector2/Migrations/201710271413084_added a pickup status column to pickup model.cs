namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedapickupstatuscolumntopickupmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pickups", "VacationStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pickups", "PickupStatus", c => c.Boolean(nullable: false));
            DropColumn("dbo.Pickups", "ActiveStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pickups", "ActiveStatus", c => c.Boolean(nullable: false));
            DropColumn("dbo.Pickups", "PickupStatus");
            DropColumn("dbo.Pickups", "VacationStatus");
        }
    }
}
