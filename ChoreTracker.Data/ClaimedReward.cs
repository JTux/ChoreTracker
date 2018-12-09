using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Data
{
    public class ClaimedReward
    {
        [Key]
        public int ClaimedRewardId { get; set; }

        [Required]
        public int RewardId { get; set; }

        [Required]
        public Guid OwnerId { get; set; }
    }
}
