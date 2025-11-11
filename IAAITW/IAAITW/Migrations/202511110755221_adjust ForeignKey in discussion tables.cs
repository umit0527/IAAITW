namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustForeignKeyindiscussiontables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MemberDiscussionPosts", "PosterId", "dbo.MemberAccounts");
            DropForeignKey("dbo.MemberDiscussionReplies", "ReplierId", "dbo.MemberAccounts");
            //AddForeignKey("dbo.MemberDiscussionPosts", "PosterId", "dbo.MemberInfoes", "Id", cascadeDelete: false);
            //AddForeignKey("dbo.MemberDiscussionReplies", "ReplierId", "dbo.MemberInfoes", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.MemberDiscussionReplies", "ReplierId", "dbo.MemberInfoes");
            //DropForeignKey("dbo.MemberDiscussionPosts", "PosterId", "dbo.MemberInfoes");
            AddForeignKey("dbo.MemberDiscussionReplies", "ReplierId", "dbo.MemberAccounts", "Id", cascadeDelete: false);
            AddForeignKey("dbo.MemberDiscussionPosts", "PosterId", "dbo.MemberAccounts", "Id", cascadeDelete: false);
        }
    }
}
