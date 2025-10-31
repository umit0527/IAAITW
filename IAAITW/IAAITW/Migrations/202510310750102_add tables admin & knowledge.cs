namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtablesadminknowledge : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                        Permission = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Knowledges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        FileName = c.String(),
                        FilePath = c.String(),
                        UploadUserId = c.Int(nullable: false),
                        UploadDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.UploadUserId, cascadeDelete: true)
                .Index(t => t.UploadUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Knowledges", "UploadUserId", "dbo.Admins");
            DropIndex("dbo.Knowledges", new[] { "UploadUserId" });
            DropTable("dbo.Knowledges");
            DropTable("dbo.Admins");
        }
    }
}
