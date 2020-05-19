namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNickNameToRegister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NickName", c => c.String(nullable: false, maxLength: 225));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NickName");
        }
    }
}
