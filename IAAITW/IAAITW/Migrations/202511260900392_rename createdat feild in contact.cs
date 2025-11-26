namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamecreatedatfeildincontact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "SentDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Contacts", "CreatedAt");
            DropColumn("dbo.Contacts", "UpdatedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contacts", "CreatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Contacts", "SentDate");
        }
    }
}
