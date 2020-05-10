namespace swop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cycles",
                c => new
                    {
                        CycleId = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CycleId);
            
            CreateTable(
                "dbo.UserCycles",
                c => new
                    {
                        UserCycleId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CycleId = c.Int(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserCycleId)
                .ForeignKey("dbo.Cycles", t => t.CycleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CycleId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        From = c.String(),
                        To = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCycles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Requests", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserCycles", "CycleId", "dbo.Cycles");
            DropIndex("dbo.Requests", new[] { "UserId" });
            DropIndex("dbo.UserCycles", new[] { "CycleId" });
            DropIndex("dbo.UserCycles", new[] { "UserId" });
            DropTable("dbo.Requests");
            DropTable("dbo.UserCycles");
            DropTable("dbo.Cycles");
        }
    }
}
