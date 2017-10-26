namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedemployeeforeignkeystocreatecontroller : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "Userid", "dbo.AspNetUsers");
            DropIndex("dbo.Employees", new[] { "Userid" });
            DropColumn("dbo.Employees", "Userid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Userid", c => c.String(maxLength: 128));
            CreateIndex("dbo.Employees", "Userid");
            AddForeignKey("dbo.Employees", "Userid", "dbo.AspNetUsers", "Id");
        }
    }
}
