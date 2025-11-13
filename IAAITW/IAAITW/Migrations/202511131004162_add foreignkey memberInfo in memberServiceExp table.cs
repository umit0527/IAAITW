namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addforeignkeymemberInfoinmemberServiceExptable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MemberServiceExps", "MemberInfo_Id", "dbo.MemberInfoes");
            DropIndex("dbo.MemberServiceExps", new[] { "MemberInfo_Id" });
            RenameColumn(table: "dbo.MemberServiceExps", name: "MemberInfo_Id", newName: "MemberId");
            AlterColumn("dbo.MemberServiceExps", "MemberId", c => c.Int(nullable: false));
            CreateIndex("dbo.MemberServiceExps", "MemberId");
            AddForeignKey("dbo.MemberServiceExps", "MemberId", "dbo.MemberInfoes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberServiceExps", "MemberId", "dbo.MemberInfoes");
            DropIndex("dbo.MemberServiceExps", new[] { "MemberId" });
            AlterColumn("dbo.MemberServiceExps", "MemberId", c => c.Int());
            RenameColumn(table: "dbo.MemberServiceExps", name: "MemberId", newName: "MemberInfo_Id");
            CreateIndex("dbo.MemberServiceExps", "MemberInfo_Id");
            AddForeignKey("dbo.MemberServiceExps", "MemberInfo_Id", "dbo.MemberInfoes", "Id");
        }
    }
}
