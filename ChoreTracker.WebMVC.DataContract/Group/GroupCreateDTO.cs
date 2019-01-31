using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.WebMVC.DataContract.Group
{
    public class GroupCreateDTO
    {
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
    }
}
