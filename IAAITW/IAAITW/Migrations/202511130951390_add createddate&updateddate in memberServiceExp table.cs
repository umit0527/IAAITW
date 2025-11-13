namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcreateddateupdateddateinmemberServiceExptable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberServiceExps", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.MemberServiceExps", "UpdatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberServiceExps", "UpdatedDate");
            DropColumn("dbo.MemberServiceExps", "CreatedDate");
        }
    }
}
