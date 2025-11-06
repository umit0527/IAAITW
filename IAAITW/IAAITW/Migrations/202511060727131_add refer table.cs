namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrefertable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Refers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        AdminId = c.Int(nullable: false),
                        UpdatedAdminId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId, cascadeDelete: false)
                .ForeignKey("dbo.Admins", t => t.UpdatedAdminId, cascadeDelete: false)
                .Index(t => t.AdminId)
                .Index(t => t.UpdatedAdminId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Refers", "UpdatedAdminId", "dbo.Admins");
            DropForeignKey("dbo.Refers", "AdminId", "dbo.Admins");
            DropIndex("dbo.Refers", new[] { "UpdatedAdminId" });
            DropIndex("dbo.Refers", new[] { "AdminId" });
            DropTable("dbo.Refers");
        }
    }
}
