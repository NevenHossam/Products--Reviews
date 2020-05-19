namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(maxLength: 11),
                        UserImageUrl = c.String(),
                        Active = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.People");
        }
    }
}
