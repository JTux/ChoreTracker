namespace ChoreTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeddbset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        OwnerId = c.Guid(nullable: false),
                        GroupName = c.String(nullable: false),
                        GroupInviteKey = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.Reward",
                c => new
                    {
                        RewardId = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        Cost = c.Int(nullable: false),
                        RewardName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RewardId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reward");
            DropTable("dbo.Group");
        }
    }
}
