namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addabouttable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abouts",
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
            DropForeignKey("dbo.Abouts", "UpdatedAdminId", "dbo.Admins");
            DropForeignKey("dbo.Abouts", "AdminId", "dbo.Admins");
            DropIndex("dbo.Abouts", new[] { "UpdatedAdminId" });
            DropIndex("dbo.Abouts", new[] { "AdminId" });
            DropTable("dbo.Abouts");
        }
    }
}
