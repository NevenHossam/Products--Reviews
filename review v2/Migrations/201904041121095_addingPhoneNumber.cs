namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingPhoneNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String(maxLength: 11));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String());
        }
    }
}
