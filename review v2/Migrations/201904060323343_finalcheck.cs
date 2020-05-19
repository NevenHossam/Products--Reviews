namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finalcheck : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Admins");
            DropTable("dbo.Companies");
            DropTable("dbo.Customers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(maxLength: 11),
                        CustomerImageUrl = c.String(),
                        CustomerImage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
    }
}
