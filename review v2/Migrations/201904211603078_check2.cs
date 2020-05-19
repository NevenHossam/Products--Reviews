namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String());
            //DropColumn("dbo.AspNetUsers", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Active", c => c.Boolean());
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String(maxLength: 11));
        }
    }
}
