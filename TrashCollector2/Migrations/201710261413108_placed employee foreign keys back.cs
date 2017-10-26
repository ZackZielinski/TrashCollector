namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class placedemployeeforeignkeysback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Userid", c => c.String(maxLength: 128));
            CreateIndex("dbo.Employees", "Userid");
            AddForeignKey("dbo.Employees", "Userid", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "Userid", "dbo.AspNetUsers");
            DropIndex("dbo.Employees", new[] { "Userid" });
            DropColumn("dbo.Employees", "Userid");
        }
    }
}
