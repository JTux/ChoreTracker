using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.WebMVC.DataContract.Group
{
    public class GroupListItemDTO
    {
        [Display(Name = "Group ID")]
        public int GroupId { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [Display(Name = "In Group")]
        public bool InGroup { get; set; }

        [Display(Name = "Group Invite Key")]
        public string InviteKey { get; set; }
    }
}
