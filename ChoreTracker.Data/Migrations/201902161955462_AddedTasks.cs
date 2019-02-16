namespace ChoreTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTasks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskEntity",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        TaskName = c.String(nullable: false),
                        TaskDescription = c.String(nullable: false),
                        RewardPoints = c.Int(nullable: false),
                        Completed = c.Boolean(nullable: false),
                        GroupId = c.Int(nullable: false),
                        DateCreated = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.GroupEntity", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskEntity", "GroupId", "dbo.GroupEntity");
            DropIndex("dbo.TaskEntity", new[] { "GroupId" });
            DropTable("dbo.TaskEntity");
        }
    }
}
