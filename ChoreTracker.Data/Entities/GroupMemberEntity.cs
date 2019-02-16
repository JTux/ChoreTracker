using System;
using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Data.Entities
{
    public class GroupMemberEntity
    {
        [Key]
        public int GroupMemberId { get; set; }

        [Required]
        public Guid MemberId { get; set; }

        public int EarnedPoints { get; set; }

        public bool InGroup { get; set; }

        [Required]
        public int GroupId { get; set; }

        public virtual GroupEntity Group { get; set; }
    }
}
