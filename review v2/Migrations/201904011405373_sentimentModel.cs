namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sentimentModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SentimentDbs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PositiveWord = c.String(),
                        PositiveValue = c.Int(nullable: false),
                        NegativeWord = c.String(),
                        NegativeValue = c.Int(nullable: false),
                        SpecialWord = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SentimentDbs");
        }
    }
}
