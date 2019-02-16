using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Data.Entities
{
    public class TaskEntity
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string TaskName { get; set; }

        [Required]
        public string TaskDescription { get; set; }

        [Required]
        public int RewardPoints { get; set; }

        public bool Completed { get; set; }

        [Required]
        public int GroupId { get; set; }

        public virtual GroupEntity Group { get; set; }

        public DateTimeOffset DateCreated { get; set; }
    }
}
