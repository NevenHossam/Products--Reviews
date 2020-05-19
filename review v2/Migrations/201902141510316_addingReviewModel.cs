namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingReviewModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rate = c.Single(nullable: false),
                        Comment = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Time = c.DateTime(nullable: false),
                        ReviewOwner = c.String(nullable: false),
                        productId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.productId, cascadeDelete: true)
                .Index(t => t.productId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "productId", "dbo.Products");
            DropIndex("dbo.Reviews", new[] { "productId" });
            DropTable("dbo.Reviews");
        }
    }
}
