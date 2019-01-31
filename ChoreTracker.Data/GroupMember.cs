using ChoreTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Data
{
    public class GroupMember
    {
        [Key]
        public int GroupMemberId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public Guid MemberId { get; set; }

        public bool InGroup { get; set; }

        public virtual GroupEntity Group { get; set; }
    }
}
