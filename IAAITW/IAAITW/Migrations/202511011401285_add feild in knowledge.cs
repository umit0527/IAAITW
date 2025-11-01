namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfeildinknowledge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Knowledges", "IsTop", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Knowledges", "IsTop");
        }
    }
}
