namespace project8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsPausePropertyToTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "IsPaused", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "IsPaused");
        }
    }
}
