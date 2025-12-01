namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlisencestable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lisences",
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
                .ForeignKey("dbo.Admins", t => t.AdminId, cascadeDelete: true)
                .ForeignKey("dbo.Admins", t => t.UpdatedAdminId, cascadeDelete: false)
                .Index(t => t.AdminId)
                .Index(t => t.UpdatedAdminId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lisences", "UpdatedAdminId", "dbo.Admins");
            DropForeignKey("dbo.Lisences", "AdminId", "dbo.Admins");
            DropIndex("dbo.Lisences", new[] { "UpdatedAdminId" });
            DropIndex("dbo.Lisences", new[] { "AdminId" });
            DropTable("dbo.Lisences");
        }
    }
}
