namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImageToIdeity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserImageURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserImageURL");
        }
    }
}
