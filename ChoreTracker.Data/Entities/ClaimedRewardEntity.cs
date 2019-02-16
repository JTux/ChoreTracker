using System;
using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Data.Entities
{
    public class ClaimedRewardEntity
    {
        [Key]
        public int ClaimedRewardId { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public int RewardId { get; set; }

        public virtual RewardEntity Reward { get; set; }

        public int ClaimedCount { get; set; }
    }
}
