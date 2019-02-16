namespace ChoreTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedRewardEntities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimedRewardEntity", "ClaimedCount", c => c.Int(nullable: false));
            AddColumn("dbo.GroupMemberEntity", "EarnedPoints", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupMemberEntity", "EarnedPoints");
            DropColumn("dbo.ClaimedRewardEntity", "ClaimedCount");
        }
    }
}
