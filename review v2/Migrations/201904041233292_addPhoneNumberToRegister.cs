namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPhoneNumberToRegister : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String(maxLength: 11));
            CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "UserNameIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String());
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false, maxLength: 225));
            CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "UserNameIndex");
        }
    }
}
