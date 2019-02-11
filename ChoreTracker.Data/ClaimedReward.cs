using ChoreTracker.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

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


        public virtual RewardEntity Reward { get; set; }
    }
}
