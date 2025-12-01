namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameupdaterIdfeildinnewstable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.News", name: "LastUpdaterId", newName: "UpdaterId");
            RenameIndex(table: "dbo.News", name: "IX_LastUpdaterId", newName: "IX_UpdaterId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.News", name: "IX_UpdaterId", newName: "IX_LastUpdaterId");
            RenameColumn(table: "dbo.News", name: "UpdaterId", newName: "LastUpdaterId");
        }
    }
}
