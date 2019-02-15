using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Services.DataContract.Group;
using ChoreTracker.Web.DataContract.Group;

namespace ChoreTracker.Services
{
    public class GroupService
    {
        private Guid _userId;

        public GroupService(Guid userId)
        {
            _userId = userId;
        }

        public GroupDetailDTO GetGroupInfo(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                GroupEntity group;
                var groupMember = ctx.GroupMembers.FirstOrDefault(g => g.MemberId == _userId && g.GroupId == id);

                if (groupMember == null) group = ctx.Groups.FirstOrDefault(g => g.GroupId == groupMember.GroupId);
                else group = groupMember.Group;

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

        public bool IsApplicant(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.GroupMembers.FirstOrDefault(m => m.GroupId == id && m.MemberId == _userId).InGroup)
                    return false;
                else
                    return true;
            }
        }

        public IEnumerable<GroupListItemDTO> GetMyGroups()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var groupMemberList = ctx.GroupMembers.Where(gm => gm.MemberId == _userId).Select(m => new GroupListItemDTO
                {
                    GroupName = m.Group.GroupName,
                    InGroup = m.InGroup,
                    GroupId = m.GroupId
                }).ToList();
                foreach(var member in groupMemberList)
                    member.InviteKey = ctx.Groups.Single(g => g.GroupId == member.GroupId).GroupInviteKey;

                return groupMemberList.ToArray();
            }
        }

        public List<GroupMemberDetailDTO> GetApplicants(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var members = new List<GroupMemberDetailDTO>();
                var groupMembers = ctx.GroupMembers.Where(g => g.GroupId == groupId && g.InGroup == false).ToList();

                foreach (var gm in groupMembers)
                {
                    var member = ctx.Users.Single(u => u.Id == gm.MemberId.ToString());
                    members.Add(new GroupMemberDetailDTO
                    {
                        UserName = member.UserName,
                        MemberId = gm.GroupMemberId
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
                var groupMembers = ctx.GroupMembers.Where(g => g.GroupId == groupId && g.InGroup == true).ToList();

                foreach (var gm in groupMembers)
                {
                    var member = ctx.Users.FirstOrDefault(u => u.Id == gm.MemberId.ToString());
                    members.Add(new GroupMemberDetailDTO
                    {
                        UserName = member.UserName
                    });
                }

                return members;
            }
        }

        public bool Acceptance(GroupAcceptanceRAO rao)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.GroupMembers.Single(gm => gm.GroupMemberId == rao.GroupMemberId);

                if (rao.Accepted)
                {
                    entity.InGroup = true;
                    return ctx.SaveChanges() == 1;
                }

                ctx.GroupMembers.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool CheckForExistingGroup(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var groupMember = ctx.GroupMembers.FirstOrDefault(m => m.GroupId == groupId && m.MemberId == _userId);
                if (groupMember != null)
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
                ctx.Groups.Add(newGroup);

                if (ctx.SaveChanges() != 1)
                    return false;

                var groups = ctx.Groups.Where(g => g.OwnerId == newGroup.OwnerId).ToList();
                var id = groups[(groups.Count() - 1)].GroupId;

                var groupMember = new GroupMemberEntity
                {
                    MemberId = _userId,
                    GroupId = id,
                    InGroup = true
                };

                ctx.GroupMembers.Add(groupMember);
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

                var groupMember = new GroupMemberEntity
                {
                    MemberId = _userId,
                    GroupId = group.GroupId,
                    InGroup = false
                };

                ctx.GroupMembers.Add(groupMember);
                return ctx.SaveChanges() == 1;
            }
        }

        public int GetGroupIDByName(string groupName)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var group = ctx.Groups.Single(g => g.GroupName == groupName && g.OwnerId == _userId);
                return group.GroupId;
            }
        }

        public int GetGroupIDByKey(string inviteKey)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var group = ctx.Groups.Single(g => g.GroupInviteKey == inviteKey);
                return group.GroupId;
            }
        }
    }
}
