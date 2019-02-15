namespace ChoreTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rediddbsets : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GroupMember", newName: "GroupMemberEntity");
            CreateTable(
                "dbo.ClaimedRewardEntity",
                c => new
                    {
                        ClaimedRewardId = c.Int(nullable: false, identity: true),
                        OwnerId = c.Guid(nullable: false),
                        RewardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClaimedRewardId)
                .ForeignKey("dbo.RewardEntity", t => t.RewardId, cascadeDelete: true)
                .Index(t => t.RewardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClaimedRewardEntity", "RewardId", "dbo.RewardEntity");
            DropIndex("dbo.ClaimedRewardEntity", new[] { "RewardId" });
            DropTable("dbo.ClaimedRewardEntity");
            RenameTable(name: "dbo.GroupMemberEntity", newName: "GroupMember");
        }
    }
}
