namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdanumberofassignedandactivepickupsforcustomers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "NumberOfAssignedPickups", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "NumberOfActivePickups", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "NumberOfActivePickups");
            DropColumn("dbo.Customers", "NumberOfAssignedPickups");
        }
    }
}
