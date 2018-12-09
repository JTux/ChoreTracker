using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoreTracker.Data;
using ChoreTracker.Models.GroupModels;
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
                var group = ctx.Groups.FirstOrDefault(g => g.OwnerId == _userId);

                if (group == null) return new GroupDetail();

                return new GroupDetail
                {
                    GroupName = group.GroupName,
                    GroupInviteKey = group.GroupInviteKey
                };
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
                var groupMember = new GroupMember
                {
                    MemberId = _userId,
                    GroupId = ctx.Groups.Single(g => g.GroupInviteKey == model.GroupInviteKey).GroupId
                };

                ctx.GroupMembers.Add(groupMember);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
