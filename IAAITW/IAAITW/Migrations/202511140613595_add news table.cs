namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewstable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Content = c.String(nullable: false),
                        CoverImage = c.String(maxLength: 300),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        PublisherId = c.Int(nullable: false),
                        LastUpdaterId = c.Int(nullable: false),
                        IsPinned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberInfoes", t => t.LastUpdaterId, cascadeDelete: false)
                .ForeignKey("dbo.MemberInfoes", t => t.PublisherId, cascadeDelete: false)
                .Index(t => t.PublisherId)
                .Index(t => t.LastUpdaterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "PublisherId", "dbo.MemberInfoes");
            DropForeignKey("dbo.News", "LastUpdaterId", "dbo.MemberInfoes");
            DropIndex("dbo.News", new[] { "LastUpdaterId" });
            DropIndex("dbo.News", new[] { "PublisherId" });
            DropTable("dbo.News");
        }
    }
}
