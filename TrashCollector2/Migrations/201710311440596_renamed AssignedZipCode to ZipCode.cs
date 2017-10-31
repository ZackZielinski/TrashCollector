namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamedAssignedZipCodetoZipCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "ZipCode", c => c.String());
            DropColumn("dbo.Employees", "AssignedZipCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "AssignedZipCode", c => c.String());
            DropColumn("dbo.Employees", "ZipCode");
        }
    }
}
