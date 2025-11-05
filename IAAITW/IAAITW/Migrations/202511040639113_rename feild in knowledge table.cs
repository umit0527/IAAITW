namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamefeildinknowledgetable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Knowledges", name: "UploadUserId", newName: "AdminId");
            RenameIndex(table: "dbo.Knowledges", name: "IX_UploadUserId", newName: "IX_AdminId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Knowledges", name: "IX_AdminId", newName: "IX_UploadUserId");
            RenameColumn(table: "dbo.Knowledges", name: "AdminId", newName: "UploadUserId");
        }
    }
}
