using ChoreTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Data
{
    public class Reward
    {
        [Key]
        public int RewardId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int Cost { get; set; }

        [Required]
        public string RewardName { get; set; }

        [Required]
        public string Description { get; set; }
        
        public virtual GroupEntity Group { get; set; }
    }
}
