namespace swop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imgUpload : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "ApartmentPicture", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "ApartmentPicture", c => c.String(nullable: false));
        }
    }
}
