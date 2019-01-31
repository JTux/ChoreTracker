﻿using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.WebMVC.DataContract.Group
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
    }
}