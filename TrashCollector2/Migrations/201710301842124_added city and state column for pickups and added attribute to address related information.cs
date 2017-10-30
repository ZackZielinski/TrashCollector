namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcityandstatecolumnforpickupsandaddedattributetoaddressrelatedinformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pickups", "City", c => c.String(nullable: false));
            AddColumn("dbo.Pickups", "State", c => c.String(nullable: false, maxLength: 2));
            AlterColumn("dbo.Pickups", "Location", c => c.String(nullable: false));
            AlterColumn("dbo.Pickups", "ZipCode", c => c.String(nullable: false, maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pickups", "ZipCode", c => c.String());
            AlterColumn("dbo.Pickups", "Location", c => c.String());
            DropColumn("dbo.Pickups", "State");
            DropColumn("dbo.Pickups", "City");
        }
    }
}
