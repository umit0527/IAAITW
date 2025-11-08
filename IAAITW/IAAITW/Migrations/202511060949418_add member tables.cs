namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmembertables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 255),
                        Salt = c.String(nullable: false, maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MemberInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Gender = c.Int(nullable: false),
                        BirthDate = c.DateTime(),
                        MembershipType = c.Int(nullable: false),
                        Phone = c.String(maxLength: 20),
                        Mobile = c.String(maxLength: 20),
                        Address = c.String(maxLength: 200),
                        Email = c.String(nullable: false, maxLength: 100),
                        InternationalMember = c.Boolean(nullable: false),
                        CurrentCompany = c.String(maxLength: 100),
                        JobTitle = c.String(maxLength: 50),
                        HighestEducation = c.String(maxLength: 50),
                        MemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberAccounts", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.MemberServiceExps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company = c.String(maxLength: 100),
                        JobTitle = c.String(maxLength: 50),
                        StartYear = c.Int(nullable: false),
                        StartMonth = c.Int(nullable: false),
                        EndYear = c.Int(),
                        EndMonth = c.Int(),
                        MemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberAccounts", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberServiceExps", "MemberId", "dbo.MemberAccounts");
            DropForeignKey("dbo.MemberInfoes", "MemberId", "dbo.MemberAccounts");
            DropIndex("dbo.MemberServiceExps", new[] { "MemberId" });
            DropIndex("dbo.MemberInfoes", new[] { "MemberId" });
            DropTable("dbo.MemberServiceExps");
            DropTable("dbo.MemberInfoes");
            DropTable("dbo.MemberAccounts");
        }
    }
}
