namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfeildupdateduseridinalltable : DbMigration
    {
        public override void Up()
        {
            // 先處理原本 AdminId FK
            DropForeignKey("dbo.Jobs", "AdminId", "dbo.Admins");
            AddForeignKey("dbo.Jobs", "AdminId", "dbo.Admins", "Id", cascadeDelete: false);

            // 新增 UpdatedAdminId 欄位
            AddColumn("dbo.Jobs", "UpdatedAdminId", c => c.Int(nullable: false));
            AddColumn("dbo.Knowledges", "UpdatedAdminId", c => c.Int(nullable: false));

            // 建立索引
            CreateIndex("dbo.Jobs", "UpdatedAdminId");
            CreateIndex("dbo.Knowledges", "UpdatedAdminId");

            // 新增 UpdatedAdminId FK
            AddForeignKey("dbo.Jobs", "UpdatedAdminId", "dbo.Admins", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Knowledges", "UpdatedAdminId", "dbo.Admins", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            // 刪除 UpdatedAdminId FK
            DropForeignKey("dbo.Knowledges", "UpdatedAdminId", "dbo.Admins");
            DropForeignKey("dbo.Jobs", "UpdatedAdminId", "dbo.Admins");

            // 刪除索引
            DropIndex("dbo.Knowledges", new[] { "UpdatedAdminId" });
            DropIndex("dbo.Jobs", new[] { "UpdatedAdminId" });

            // 刪除欄位
            DropColumn("dbo.Knowledges", "UpdatedAdminId");
            DropColumn("dbo.Jobs", "UpdatedAdminId");

            // 還原 AdminId FK (如果原本 cascadeDelete = true)
            DropForeignKey("dbo.Jobs", "AdminId", "dbo.Admins");
            AddForeignKey("dbo.Jobs", "AdminId", "dbo.Admins", "Id", cascadeDelete: true); // 或 false 看你原本需求
        }

    }
}
