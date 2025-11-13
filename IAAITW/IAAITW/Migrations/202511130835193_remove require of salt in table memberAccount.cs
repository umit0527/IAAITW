namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removerequireofsaltintablememberAccount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MemberAccounts", "Salt", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MemberAccounts", "Salt", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
