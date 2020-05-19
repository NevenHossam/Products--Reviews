namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editsentimentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SentimentDbs", "Word", c => c.String());
            AddColumn("dbo.SentimentDbs", "Value", c => c.Int(nullable: false));
            DropColumn("dbo.SentimentDbs", "PositiveWord");
            DropColumn("dbo.SentimentDbs", "PositiveValue");
            DropColumn("dbo.SentimentDbs", "NegativeWord");
            DropColumn("dbo.SentimentDbs", "NegativeValue");
            DropColumn("dbo.SentimentDbs", "SpecialWord");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SentimentDbs", "SpecialWord", c => c.String());
            AddColumn("dbo.SentimentDbs", "NegativeValue", c => c.Int(nullable: false));
            AddColumn("dbo.SentimentDbs", "NegativeWord", c => c.String());
            AddColumn("dbo.SentimentDbs", "PositiveValue", c => c.Int(nullable: false));
            AddColumn("dbo.SentimentDbs", "PositiveWord", c => c.String());
            DropColumn("dbo.SentimentDbs", "Value");
            DropColumn("dbo.SentimentDbs", "Word");
        }
    }
}
