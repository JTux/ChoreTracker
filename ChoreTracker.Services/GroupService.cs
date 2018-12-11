using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoreTracker.Data;
using ChoreTracker.Models.GroupModels;
using ChoreTracker.Models.RewardModels;
using ChoreTracker.WebMVC.Data;

namespace ChoreTracker.Services
{
    public class GroupService
    {
        private Guid _userId;

        public GroupService(Guid userId)
        {
            _userId = userId;
        }

        public GroupDetail GetGroupInfo()
        {
            using (var ctx = new ApplicationDbContext())
            {
                Group group;
                var groupMember = ctx.GroupMembers.FirstOrDefault(g => g.MemberId == _userId);

                if (groupMember == null)
                    group = ctx.Groups.FirstOrDefault(g => g.OwnerId == _userId);
                else
                    group = ctx.Groups.FirstOrDefault(g => g.GroupId == groupMember.GroupId);

                if (group != null)
                {
                    var owner = ctx.Users.FirstOrDefault(u => u.Id == group.OwnerId.ToString());
                    return new GroupDetail
                    {
                        GroupId = group.GroupId,
                        GroupName = group.GroupName,
                        GroupInviteKey = group.GroupInviteKey,
                        GroupOwner = owner.UserName
                    };
                }

                return new GroupDetail();
            }
        }

        public List<GroupMemberDetail> GetGroupMembers(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var members = new List<GroupMemberDetail>();
                var groupMembers = ctx.GroupMembers.Where(g => g.GroupId == groupId);

                foreach (var gm in groupMembers)
                {
                    var member = ctx.Users.Single(u => u.Id == gm.MemberId.ToString());
                    members.Add(new GroupMemberDetail
                    {
                        UserName = member.UserName
                    });
                }

                return members;
            }
        }

        public bool CheckForExistingGroup()
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.Groups.Where(g => g.OwnerId == _userId).Count() != 0 || ctx.GroupMembers.Where(gm => gm.MemberId == _userId).Count() != 0)
                    return true;

                return false;
            }
        }

        public bool CreateGroup(GroupCreate model)
        {
            var newGroup = new Group
            {
                GroupName = model.GroupName,
                OwnerId = _userId,
                GroupInviteKey = GenerateRandomString(6)
            };

            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.Groups.Where(g => g.OwnerId == _userId).Count() != 0)
                    return true;

                ctx.Groups.Add(newGroup);

                return ctx.SaveChanges() == 1;
            }
        }

        private string GenerateRandomString(int size)
        {
            var random = new Random();
            var builder = new StringBuilder();
            char character;

            for (int i = 0; i < size; i++)
            {
                character = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(character);
            }
            return builder.ToString();
        }

        public bool JoinGroup(JoinGroup model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var group = ctx.Groups.FirstOrDefault(g => g.GroupInviteKey == model.GroupInviteKey);
                if (group == null)
                    return false;

                var groupMember = new GroupMember
                {
                    MemberId = _userId,
                    GroupId = group.GroupId
                };

                ctx.GroupMembers.Add(groupMember);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
