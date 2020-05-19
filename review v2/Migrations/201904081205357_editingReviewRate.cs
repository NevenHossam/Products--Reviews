namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editingReviewRate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CompanyName", c => c.String(nullable: false));
            AddColumn("dbo.Reviews", "Ratio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Reviews", "Category", c => c.String());
            AlterColumn("dbo.Products", "TotalPercentageRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Reviews", "Rate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reviews", "Rate", c => c.Single(nullable: false));
            AlterColumn("dbo.Products", "TotalPercentageRate", c => c.Single(nullable: false));
            DropColumn("dbo.Reviews", "Category");
            DropColumn("dbo.Reviews", "Ratio");
            DropColumn("dbo.Products", "CompanyName");
        }
    }
}
