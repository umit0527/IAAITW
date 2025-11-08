namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renametable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberInfoes", "FilePath", c => c.String(maxLength: 255));
            AddColumn("dbo.MemberInfoes", "CurrentJobTitle", c => c.String(maxLength: 50));
            DropColumn("dbo.MemberInfoes", "JobTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberInfoes", "JobTitle", c => c.String(maxLength: 50));
            DropColumn("dbo.MemberInfoes", "CurrentJobTitle");
            DropColumn("dbo.MemberInfoes", "FilePath");
        }
    }
}
