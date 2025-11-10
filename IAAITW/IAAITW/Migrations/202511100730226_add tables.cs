namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Experts",
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
            
            CreateTable(
                "dbo.Histories",
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
            
            CreateTable(
                "dbo.Organizations",
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
            DropForeignKey("dbo.Organizations", "UpdatedAdminId", "dbo.Admins");
            DropForeignKey("dbo.Organizations", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.Histories", "UpdatedAdminId", "dbo.Admins");
            DropForeignKey("dbo.Histories", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.Experts", "UpdatedAdminId", "dbo.Admins");
            DropForeignKey("dbo.Experts", "AdminId", "dbo.Admins");
            DropIndex("dbo.Organizations", new[] { "UpdatedAdminId" });
            DropIndex("dbo.Organizations", new[] { "AdminId" });
            DropIndex("dbo.Histories", new[] { "UpdatedAdminId" });
            DropIndex("dbo.Histories", new[] { "AdminId" });
            DropIndex("dbo.Experts", new[] { "UpdatedAdminId" });
            DropIndex("dbo.Experts", new[] { "AdminId" });
            DropTable("dbo.Organizations");
            DropTable("dbo.Histories");
            DropTable("dbo.Experts");
        }
    }
}
