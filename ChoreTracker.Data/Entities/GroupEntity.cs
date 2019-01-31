using System;
using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Data.Entities
{
    public class GroupEntity
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public string GroupName { get; set; }

        [Required]
        public string GroupInviteKey { get; set; }

        public virtual ApplicationUser Owner { get; set; }
    }
}
