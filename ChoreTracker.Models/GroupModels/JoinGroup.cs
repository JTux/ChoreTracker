using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.GroupModels
{
    public class JoinGroup
    {
        [Required]
        [Display(Name = "Group Invite Key")]
        public string GroupInviteKey { get; set; }
    }
}
