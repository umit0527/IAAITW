namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtotalyearmonthfeild : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberServiceExps", "TotalYears", c => c.Int(nullable: false));
            AddColumn("dbo.MemberServiceExps", "TotalMonths", c => c.Int(nullable: false));
            AlterColumn("dbo.MemberServiceExps", "StartYear", c => c.Int());
            AlterColumn("dbo.MemberServiceExps", "StartMonth", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MemberServiceExps", "StartMonth", c => c.Int(nullable: false));
            AlterColumn("dbo.MemberServiceExps", "StartYear", c => c.Int(nullable: false));
            DropColumn("dbo.MemberServiceExps", "TotalMonths");
            DropColumn("dbo.MemberServiceExps", "TotalYears");
        }
    }
}
