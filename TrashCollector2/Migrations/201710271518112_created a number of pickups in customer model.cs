namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdanumberofpickupsincustomermodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "NumberOfPickups", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "NumberOfPickups");
        }
    }
}
