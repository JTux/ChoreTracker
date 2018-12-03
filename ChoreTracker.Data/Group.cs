using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Data
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public string GroupName { get; set; }

        [Required]
        public string GroupInviteKey { get; set; }
    }
}
