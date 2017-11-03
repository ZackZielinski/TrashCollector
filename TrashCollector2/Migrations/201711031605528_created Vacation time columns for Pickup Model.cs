namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdVacationtimecolumnsforPickupModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pickups", "VacationStart", c => c.String());
            AddColumn("dbo.Pickups", "VacationEnd", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pickups", "VacationEnd");
            DropColumn("dbo.Pickups", "VacationStart");
        }
    }
}
