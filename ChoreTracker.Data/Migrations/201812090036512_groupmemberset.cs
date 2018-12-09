namespace ChoreTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class groupmemberset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupMember",
                c => new
                    {
                        GroupMemberId = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        MemberId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GroupMemberId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GroupMember");
        }
    }
}
