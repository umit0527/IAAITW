namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustfeildcontentinjobtable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "Content", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}
