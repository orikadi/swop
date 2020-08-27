namespace swop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApartmentScore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApartmentScores",
                c => new
                    {
                        ApartmentScoreId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                        ScoreByUser_UserId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ApartmentScoreId)
                .ForeignKey("dbo.Users", t => t.ScoreByUser_UserId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.UserId)
                .Index(t => t.ScoreByUser_UserId)
                .Index(t => t.User_UserId);
            
            AddColumn("dbo.Users", "ApartmentScore", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApartmentScores", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.ApartmentScores", "UserId", "dbo.Users");
            DropForeignKey("dbo.ApartmentScores", "ScoreByUser_UserId", "dbo.Users");
            DropIndex("dbo.ApartmentScores", new[] { "User_UserId" });
            DropIndex("dbo.ApartmentScores", new[] { "ScoreByUser_UserId" });
            DropIndex("dbo.ApartmentScores", new[] { "UserId" });
            DropColumn("dbo.Users", "ApartmentScore");
            DropTable("dbo.ApartmentScores");
        }
    }
}
