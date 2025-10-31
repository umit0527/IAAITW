namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfeildinadmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Admins", "CreateDate");
        }
    }
}
