namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixForeignKeyindiscussiontables : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.MemberDiscussionPosts", "PosterId", "dbo.MemberInfoes", "Id", cascadeDelete: false);
            AddForeignKey("dbo.MemberDiscussionReplies", "ReplierId", "dbo.MemberInfoes", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            DropForeignKey("dbo.MemberDiscussionReplies", "ReplierId", "dbo.MemberInfoes");
            DropForeignKey("dbo.MemberDiscussionPosts", "PosterId", "dbo.MemberInfoes");
        }
    }
}
