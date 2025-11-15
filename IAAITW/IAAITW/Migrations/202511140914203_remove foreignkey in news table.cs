namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeforeignkeyinnewstable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.News", "LastUpdaterId", "dbo.MemberInfoes");
            DropForeignKey("dbo.News", "PublisherId", "dbo.MemberInfoes");
            DropIndex("dbo.News", new[] { "PublisherId" });
            DropIndex("dbo.News", new[] { "LastUpdaterId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.News", "LastUpdaterId");
            CreateIndex("dbo.News", "PublisherId");
            AddForeignKey("dbo.News", "PublisherId", "dbo.MemberInfoes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.News", "LastUpdaterId", "dbo.MemberInfoes", "Id", cascadeDelete: true);
        }
    }
}
