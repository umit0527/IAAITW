namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustfeildintablememberInfoes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberInfoes", "IsInternationalMember", c => c.Boolean(nullable: false));
            DropColumn("dbo.MemberInfoes", "InternationalMember");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberInfoes", "InternationalMember", c => c.Boolean(nullable: false));
            DropColumn("dbo.MemberInfoes", "IsInternationalMember");
        }
    }
}
