namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNavigationpropertiesinMemberInfoestable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberServiceExps", "MemberInfo_Id", c => c.Int());
            CreateIndex("dbo.MemberServiceExps", "MemberInfo_Id");
            AddForeignKey("dbo.MemberServiceExps", "MemberInfo_Id", "dbo.MemberInfoes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberServiceExps", "MemberInfo_Id", "dbo.MemberInfoes");
            DropIndex("dbo.MemberServiceExps", new[] { "MemberInfo_Id" });
            DropColumn("dbo.MemberServiceExps", "MemberInfo_Id");
        }
    }
}
