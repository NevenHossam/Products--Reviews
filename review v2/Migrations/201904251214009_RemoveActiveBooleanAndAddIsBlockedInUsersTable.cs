namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveActiveBooleanAndAddIsBlockedInUsersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsBlocked", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Active", c => c.Boolean());
            DropColumn("dbo.AspNetUsers", "IsBlocked");
        }
    }
}
