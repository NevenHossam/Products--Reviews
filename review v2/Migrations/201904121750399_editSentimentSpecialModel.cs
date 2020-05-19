
namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editSentimentSpecialModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SentimentSpecials", "IsBefore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SentimentSpecials", "IsBefore");
        }
    }
}
