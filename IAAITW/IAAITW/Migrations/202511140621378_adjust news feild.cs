namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustnewsfeild : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "CoverImage", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "CoverImage", c => c.String(maxLength: 300));
        }
    }
}
