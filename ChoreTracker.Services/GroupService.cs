﻿using System;
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

        public GroupDetailDTO GetGroupInfo(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var groupMember = ctx.GroupMembers.FirstOrDefault(g => g.MemberId == _userId && g.GroupId == groupId);

                if (groupMember == null) return new GroupDetailDTO();

                var group = groupMember.Group;

                var commentService = new CommentService(_userId);

                if (group != null)
                {
                    var owner = ctx.Users.FirstOrDefault(u => u.Id == group.OwnerId.ToString());
                    return new GroupDetailDTO
                    {
                        GroupId = group.GroupId,
                        GroupName = group.GroupName,
                        GroupInviteKey = group.GroupInviteKey,
                        GroupOwner = owner.UserName,
                        GroupOwnerId = Guid.Parse(owner.Id),
                        GroupApplicants = GetApplicants(groupId),
                        GroupMembers = GetGroupMembers(groupId),
                        IsMod = IsMod(groupId),
                        Comments = commentService.GetGroupComments(groupId)
                    };
                }

                return new GroupDetailDTO();
            }
        }

        public bool IsMod(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var groupMember = ctx.GroupMembers.FirstOrDefault(gm => gm.MemberId == _userId && gm.GroupId == groupId);

                if (groupMember == null)
                    return false;

                return groupMember.IsMod;
            }
        }

        public bool IsApplicant(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var member = ctx.GroupMembers.FirstOrDefault(m => m.GroupId == id && m.MemberId == _userId);

                if (member == null || member.InGroup) return false;
                else return true;
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
                    GroupId = m.GroupId,
                    IsMod = m.IsMod
                }).ToList();

                foreach (var member in groupMemberList)
                    member.InviteKey = ctx.Groups.Single(g => g.GroupId == member.GroupId).GroupInviteKey;

                return groupMemberList.ToArray();
            }
        }

        public IEnumerable<GroupMemberDetailDTO> GetApplicants(int groupId)
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

        public IEnumerable<GroupMemberDetailDTO> GetGroupMembers(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var members = new List<GroupMemberDetailDTO>();
                var groupMembers = ctx.GroupMembers.Where(g => g.GroupId == groupId && g.InGroup == true).ToList();
                var ownerId = ctx.Groups.Single(g => g.GroupId == groupId).OwnerId;
                foreach (var gm in groupMembers)
                {
                    bool isOwner = false;
                    if (gm.MemberId == ownerId)
                        isOwner = true;

                    var member = ctx.Users.FirstOrDefault(u => u.Id == gm.MemberId.ToString());
                    members.Add(new GroupMemberDetailDTO
                    {
                        MemberId = gm.GroupMemberId,
                        UserName = member.UserName,
                        IsOwner = isOwner,
                        IsMod = gm.IsMod
                    });
                }

                return members;
            }
        }

        public bool PromoteMember(GroupPromoteRAO rao)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var groupMember = ctx.GroupMembers.Single(gm => gm.GroupMemberId == rao.GroupMemberId);
                var ownerId = groupMember.Group.OwnerId;

                if (_userId != ownerId) return false;

                groupMember.IsMod = true;
                return ctx.SaveChanges() == 1;
            }
        }

        public bool KickMember(int memberId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var groupMember = ctx.GroupMembers.Single(gm => gm.GroupMemberId == memberId);

                if (!IsMod(groupMember.GroupId)) return false;

                var ownerId = groupMember.Group.OwnerId;

                if (groupMember.IsMod && ownerId != _userId) return false;

                ctx.GroupMembers.Remove(groupMember);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool Acceptance(GroupAcceptanceRAO rao)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.GroupMembers.FirstOrDefault(gm => gm.GroupMemberId == rao.GroupMemberId && gm.GroupId == rao.GroupId);

                if (entity == null)
                    return false;

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
                    InGroup = true,
                    IsMod = true
                };

                ctx.GroupMembers.Add(groupMember);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool EditGroupInviteKey(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (!IsMod(groupId)) return false;

                var group = ctx.Groups.Single(g => g.GroupId == groupId);

                group.GroupInviteKey = GenerateRandomString(8);

                return ctx.SaveChanges() == 1;
            }
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

        public bool JoinGroup(GroupJoinRAO rao)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var group = ctx.Groups.FirstOrDefault(g => g.GroupInviteKey == rao.GroupInviteKey);
                if (group == null)
                    return false;

                if (ctx.GroupMembers.Where(gm => gm.MemberId == _userId).Count() > 0)
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

        public bool LeaveGroup(GroupLeaveRAO rao)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var group = ctx.Groups.FirstOrDefault(g => g.GroupInviteKey == rao.GroupInviteKey);
                var groupMemberCount = ctx.GroupMembers.Where(gm => gm.GroupId == rao.GroupId).Count();

                if (group == null)
                    return false;
                else if (group.OwnerId == _userId && groupMemberCount > 1)
                    return false;

                int changes = 0;

                var groupMember = ctx.GroupMembers.FirstOrDefault(gm => gm.MemberId == _userId && gm.GroupId == rao.GroupId);
                ctx.GroupMembers.Remove(groupMember);
                changes++;

                if (groupMemberCount == 1)
                {
                    ctx.Groups.Remove(group);
                    changes++;
                }

                return ctx.SaveChanges() == changes;
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
