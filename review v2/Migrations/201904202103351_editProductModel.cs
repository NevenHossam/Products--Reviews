namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editProductModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "ProductCategoryId");
            AddForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories", "Id", cascadeDelete: true);
           // DropColumn("dbo.Products", "TypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "TypeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories");
            DropIndex("dbo.Products", new[] { "ProductCategoryId" });
            DropColumn("dbo.Products", "ProductCategoryId");
        }
    }
}
