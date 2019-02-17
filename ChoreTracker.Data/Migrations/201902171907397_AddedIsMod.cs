namespace ChoreTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsMod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupMemberEntity", "IsMod", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupMemberEntity", "IsMod");
        }
    }
}
