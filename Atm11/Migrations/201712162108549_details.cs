namespace Atm11.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class details : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "firstName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "middleName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "surname", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "add1", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "add2", c => c.String());
            AddColumn("dbo.AspNetUsers", "city", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "DOB", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DOB");
            DropColumn("dbo.AspNetUsers", "city");
            DropColumn("dbo.AspNetUsers", "add2");
            DropColumn("dbo.AspNetUsers", "add1");
            DropColumn("dbo.AspNetUsers", "surname");
            DropColumn("dbo.AspNetUsers", "middleName");
            DropColumn("dbo.AspNetUsers", "firstName");
        }
    }
}
