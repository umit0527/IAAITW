namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixindexpage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MemberAccounts", "Password", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MemberAccounts", "Password", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
