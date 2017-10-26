namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdcustomeremployeeanddaymodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Userid = c.String(maxLength: 128),
                        DayId = c.Int(nullable: false),
                        OnVacation = c.Boolean(nullable: false),
                        Location = c.String(),
                        ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Days", t => t.DayId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Userid)
                .Index(t => t.Userid)
                .Index(t => t.DayId);
            
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayName = c.String(),
                        Payment = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Userid = c.String(maxLength: 128),
                        AssignedZipCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Userid)
                .Index(t => t.Userid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "Userid", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "Userid", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "DayId", "dbo.Days");
            DropIndex("dbo.Employees", new[] { "Userid" });
            DropIndex("dbo.Customers", new[] { "DayId" });
            DropIndex("dbo.Customers", new[] { "Userid" });
            DropTable("dbo.Employees");
            DropTable("dbo.Days");
            DropTable("dbo.Customers");
        }
    }
}
