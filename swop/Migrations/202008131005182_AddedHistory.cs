namespace swop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        HistoryId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Guest_UserId = c.Int(),
                        Host_UserId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.HistoryId)
                .ForeignKey("dbo.Users", t => t.Guest_UserId)
                .ForeignKey("dbo.Users", t => t.Host_UserId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.UserId)
                .Index(t => t.Guest_UserId)
                .Index(t => t.Host_UserId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Histories", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Histories", "UserId", "dbo.Users");
            DropForeignKey("dbo.Histories", "Host_UserId", "dbo.Users");
            DropForeignKey("dbo.Histories", "Guest_UserId", "dbo.Users");
            DropIndex("dbo.Histories", new[] { "User_UserId" });
            DropIndex("dbo.Histories", new[] { "Host_UserId" });
            DropIndex("dbo.Histories", new[] { "Guest_UserId" });
            DropIndex("dbo.Histories", new[] { "UserId" });
            DropTable("dbo.Histories");
        }
    }
}
