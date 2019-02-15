using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Web.DataContract.Group
{
    public class GroupCreateDTO
    {
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
    }
}
