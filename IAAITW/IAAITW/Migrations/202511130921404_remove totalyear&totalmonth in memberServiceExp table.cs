namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removetotalyeartotalmonthinmemberServiceExptable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MemberServiceExps", "TotalYears");
            DropColumn("dbo.MemberServiceExps", "TotalMonths");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberServiceExps", "TotalMonths", c => c.Int(nullable: false));
            AddColumn("dbo.MemberServiceExps", "TotalYears", c => c.Int(nullable: false));
        }
    }
}
