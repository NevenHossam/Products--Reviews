namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStopWordsModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StopWords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Word = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StopWords");
        }
    }
}
