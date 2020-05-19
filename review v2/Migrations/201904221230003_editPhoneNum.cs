namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editPhoneNum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PhoneNum", c => c.String(maxLength: 11));
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String(maxLength: 11));
            DropColumn("dbo.AspNetUsers", "PhoneNum");
        }
    }
}
