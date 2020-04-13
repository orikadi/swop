namespace swop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        Balance = c.Double(nullable: false),
                        Password = c.String(nullable: false),
                        UserPicture = c.String(),
                        UserType = c.Int(nullable: false),
                        Country = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        ApartmentPicture = c.String(nullable: false),
                        ApartmentDescription = c.String(nullable: false),
                        ApartmentPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Email);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
