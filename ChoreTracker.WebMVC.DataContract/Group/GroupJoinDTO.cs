﻿using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.WebMVC.DataContract.Group
{
    public class GroupJoinDTO
    {
        [Required]
        [Display(Name = "Group Invite Key")]
        public string GroupInviteKey { get; set; }
    }
}
