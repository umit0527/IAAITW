namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmemberDiscussiontables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberDiscussionPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Content = c.String(nullable: false),
                        PosterId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberAccounts", t => t.PosterId, cascadeDelete: false)
                .Index(t => t.PosterId);
            
            CreateTable(
                "dbo.MemberDiscussionReplies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        Content = c.String(nullable: false),
                        ReplierId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberAccounts", t => t.ReplierId, cascadeDelete: false)
                .ForeignKey("dbo.MemberDiscussionPosts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.ReplierId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberDiscussionReplies", "PostId", "dbo.MemberDiscussionPosts");
            DropForeignKey("dbo.MemberDiscussionReplies", "ReplierId", "dbo.MemberAccounts");
            DropForeignKey("dbo.MemberDiscussionPosts", "PosterId", "dbo.MemberAccounts");
            DropIndex("dbo.MemberDiscussionReplies", new[] { "ReplierId" });
            DropIndex("dbo.MemberDiscussionReplies", new[] { "PostId" });
            DropIndex("dbo.MemberDiscussionPosts", new[] { "PosterId" });
            DropTable("dbo.MemberDiscussionReplies");
            DropTable("dbo.MemberDiscussionPosts");
        }
    }
}
