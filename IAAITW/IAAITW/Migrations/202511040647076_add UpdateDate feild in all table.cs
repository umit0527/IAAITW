namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUpdateDatefeildinalltable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contacts", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Jobs", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Knowledges", "UpdatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Knowledges", "UpdatedDate");
            DropColumn("dbo.Jobs", "UpdatedDate");
            DropColumn("dbo.Contacts", "UpdatedDate");
            DropColumn("dbo.Admins", "UpdatedDate");
        }
    }
}
