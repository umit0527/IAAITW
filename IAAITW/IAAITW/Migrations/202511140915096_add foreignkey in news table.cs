namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addforeignkeyinnewstable : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.News", "PublisherId");
            CreateIndex("dbo.News", "LastUpdaterId");
            AddForeignKey("dbo.News", "LastUpdaterId", "dbo.Admins", "Id", cascadeDelete: false);
            AddForeignKey("dbo.News", "PublisherId", "dbo.Admins", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "PublisherId", "dbo.Admins");
            DropForeignKey("dbo.News", "LastUpdaterId", "dbo.Admins");
            DropIndex("dbo.News", new[] { "LastUpdaterId" });
            DropIndex("dbo.News", new[] { "PublisherId" });
        }
    }
}
