namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustfeildincontacttable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Contacts", "Content", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "Content", c => c.String(maxLength: 500));
            AlterColumn("dbo.Contacts", "Title", c => c.String(maxLength: 100));
        }
    }
}
