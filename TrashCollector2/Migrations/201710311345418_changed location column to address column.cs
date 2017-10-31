namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedlocationcolumntoaddresscolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pickups", "Address", c => c.String(nullable: false));
            DropColumn("dbo.Pickups", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pickups", "Location", c => c.String(nullable: false));
            DropColumn("dbo.Pickups", "Address");
        }
    }
}
