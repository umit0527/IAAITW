namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnableCascadeDeleteOnReplies : DbMigration
    {
        public override void Up()
        {
            // 先刪掉舊的 FK
            DropForeignKey("dbo.MemberDiscussionReplies", "PostId", "dbo.MemberDiscussionPosts");

            // 再新增 FK 並啟用 Cascade
            AddForeignKey("dbo.MemberDiscussionReplies", "PostId", "dbo.MemberDiscussionPosts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberDiscussionReplies", "PostId", "dbo.MemberDiscussionPosts");

            AddForeignKey("dbo.MemberDiscussionReplies", "PostId", "dbo.MemberDiscussionPosts", "Id", cascadeDelete: false);
        }
    }
}
