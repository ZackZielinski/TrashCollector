namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedNumberOfPickupscolumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customers", "NumberOfPickups");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "NumberOfPickups", c => c.Int(nullable: false));
        }
    }
}
