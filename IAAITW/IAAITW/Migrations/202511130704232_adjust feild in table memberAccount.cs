namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustfeildintablememberAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberAccounts", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.MemberAccounts", "UpdatedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.MemberAccounts", "CreatedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberAccounts", "CreatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.MemberAccounts", "UpdatedDate");
            DropColumn("dbo.MemberAccounts", "CreatedDate");
        }
    }
}
