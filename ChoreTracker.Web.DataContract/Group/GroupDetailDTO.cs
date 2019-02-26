using ChoreTracker.Web.DataContract.Comment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Web.DataContract.Group
{
    public class GroupDetailDTO
    {
        [Display(Name = "Group Id")]
        public int GroupId { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [Display(Name = "Group Invite Key")]
        public string GroupInviteKey { get; set; }

        [Display(Name = "Group Owner")]
        public string GroupOwner { get; set; }

        public Guid GroupOwnerId { get; set; }

        public IEnumerable<GroupMemberDetailDTO> GroupMembers { get; set; }

        public IEnumerable<GroupMemberDetailDTO> GroupApplicants { get; set; }

        public IEnumerable<CommentListItemDTO> Comments { get; set; }

        public bool IsMod { get; set; }
    }
}
