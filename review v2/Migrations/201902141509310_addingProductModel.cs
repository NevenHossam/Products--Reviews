namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingProductModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        TotalPercentageRate = c.Single(nullable: false),
                        Price = c.Single(nullable: false),
                        Features = c.String(nullable: false),
                        ProductImageUrl = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
        }
    }
}
