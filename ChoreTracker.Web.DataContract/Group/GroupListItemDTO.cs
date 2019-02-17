using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Web.DataContract.Group
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
        public bool IsMod { get; set; }
    }
}
