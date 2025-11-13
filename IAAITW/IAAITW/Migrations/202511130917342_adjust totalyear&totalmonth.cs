namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjusttotalyeartotalmonth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberInfoes", "TotalExpYears", c => c.Int(nullable: false));
            AddColumn("dbo.MemberInfoes", "TotalExpMonths", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberInfoes", "TotalExpMonths");
            DropColumn("dbo.MemberInfoes", "TotalExpYears");
        }
    }
}
