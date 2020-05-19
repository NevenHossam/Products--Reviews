namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingAdminModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(maxLength: 11),
                        AdminImageUrl = c.String(),
                        AdminImage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Admins");
        }
    }
}
