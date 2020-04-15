namespace swop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Users");
            AddColumn("dbo.Users", "UserId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Users", "UserId");
            AddPrimaryKey("dbo.Users", "Email");
        }
    }
}
