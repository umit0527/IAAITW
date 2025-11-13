namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeforeignkeymemberaccountinmemberServiceExptable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MemberServiceExps", "MemberId", "dbo.MemberAccounts");
            DropIndex("dbo.MemberServiceExps", new[] { "MemberId" });
            DropColumn("dbo.MemberServiceExps", "MemberId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberServiceExps", "MemberId", c => c.Int(nullable: false));
            CreateIndex("dbo.MemberServiceExps", "MemberId");
            AddForeignKey("dbo.MemberServiceExps", "MemberId", "dbo.MemberAccounts", "Id", cascadeDelete: true);
        }
    }
}
