namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustfeildinknowledge : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Knowledges", "FileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Knowledges", "FileName", c => c.String());
        }
    }
}
