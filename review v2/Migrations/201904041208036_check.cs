namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AspNetUsers", "Username", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "UserNameIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            AlterColumn("dbo.AspNetUsers", "Username", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "UserNameIndex");
        }
    }
}
