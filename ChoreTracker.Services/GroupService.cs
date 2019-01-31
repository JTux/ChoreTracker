using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Services.DataContract.Group;
using ChoreTracker.WebMVC.DataContract.Group;

namespace ChoreTracker.Services
{
    public class GroupService
    {
        private Guid _userId;

        public GroupService(Guid userId)
        {
            _userId = userId;
        }

        public GroupDetailDTO GetGroupInfo()
        {
            using (var ctx = new ApplicationDbContext())
            {
                GroupEntity group;
                var groupMember = ctx.GroupMembers.FirstOrDefault(g => g.MemberId == _userId);

                if (groupMember == null)
                    group = ctx.Groups.FirstOrDefault(g => g.OwnerId == _userId);
                else
                    group = groupMember.Group;

                if (group != null)
                {
                    var owner = ctx.Users.FirstOrDefault(u => u.Id == group.OwnerId.ToString());
                    return new GroupDetailDTO
                    {
                        GroupId = group.GroupId,
                        GroupName = group.GroupName,
                        GroupInviteKey = group.GroupInviteKey,
                        GroupOwner = owner.UserName
                    };
                }

                return new GroupDetailDTO();
            }
        }

        public List<GroupMemberDetailDTO> GetApplicants(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var members = new List<GroupMemberDetailDTO>();
                var groupMembers = ctx.GroupMembers.Where(g => g.GroupId == groupId && g.InGroup == false);

                foreach (var gm in groupMembers)
                {
                    var member = ctx.Users.Single(u => u.Id == gm.MemberId.ToString());
                    members.Add(new GroupMemberDetailDTO
                    {
                        UserName = member.UserName
                    });
                }

                return members;
            }
        }

        public List<GroupMemberDetailDTO> GetGroupMembers(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var members = new List<GroupMemberDetailDTO>();
                var groupMembers = ctx.GroupMembers.Where(g => g.GroupId == groupId && g.InGroup == true);

                foreach (var gm in groupMembers)
                {
                    var member = ctx.Users.Single(u => u.Id == gm.MemberId.ToString());
                    members.Add(new GroupMemberDetailDTO
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

        public bool CreateGroup(GroupCreateRAO model)
        {
            var newGroup = new GroupEntity
            {
                GroupName = model.GroupName,
                OwnerId = _userId,
                GroupInviteKey = GenerateRandomString(8)
            };

            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.Groups.Where(g => g.OwnerId == _userId).Count() != 0)
                    return true;

                ctx.Groups.Add(newGroup);

                return ctx.SaveChanges() == 1;
            }
        }

        public void EditGroupInviteKey()
        {

        }

        private string GenerateRandomString(int size)
        {
            var random = new Random();
            var builder = new StringBuilder();
            char character;
            bool isUnique = false;

            string key = "";

            while (!isUnique)
            {
                for (int i = 0; i < size; i++)
                {
                    character = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(character);
                }

                key = builder.ToString();

                using (var ctx = new ApplicationDbContext())
                {
                    isUnique = ctx.Groups.Where(g => g.GroupInviteKey == key).Count() == 0;
                }
            }
            return key;
        }

        public bool JoinGroup(GroupJoinRAO model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var group = ctx.Groups.FirstOrDefault(g => g.GroupInviteKey == model.GroupInviteKey);
                if (group == null)
                    return false;

                var groupMember = new GroupMember
                {
                    MemberId = _userId,
                    GroupId = group.GroupId,
                    InGroup = true
                };

                ctx.GroupMembers.Add(groupMember);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
