namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteNickNameToRegister : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "NickName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "NickName", c => c.String(nullable: false, maxLength: 225));
        }
    }
}
