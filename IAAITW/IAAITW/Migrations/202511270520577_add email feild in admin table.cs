namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addemailfeildinadmintable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "Email", c => c.String());
            AddColumn("dbo.Admins", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Admins", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Admins", "CreateDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Admins", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Admins", "Name", c => c.String());
            DropColumn("dbo.Admins", "CreatedDate");
            DropColumn("dbo.Admins", "Email");
        }
    }
}
