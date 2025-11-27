namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustpasswordSaltrequierinadmintable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Admins", "PasswordSalt", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Admins", "PasswordSalt", c => c.String(nullable: false));
        }
    }
}
