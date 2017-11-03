namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedVacationEndandVacationStartcolumns : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pickups", "VacationStart");
            DropColumn("dbo.Pickups", "VacationEnd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pickups", "VacationEnd", c => c.DateTime());
            AddColumn("dbo.Pickups", "VacationStart", c => c.DateTime());
        }
    }
}
