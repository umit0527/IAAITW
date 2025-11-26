namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustemailrequirefeildinadmintable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Admins", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Admins", "Email", c => c.String());
        }
    }
}
