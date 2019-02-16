﻿using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Web.DataContract.Reward
{
    public class RewardListItemDTO
    {
        [Required]
        public int RewardId { get; set; }

        [Required]
        [Display(Name = "Name of Reward")]
        public string RewardName { get; set; }

        [Required]
        [Display(Name = "Reward Description")]
        public string RewardDescription { get; set; }

        [Required]
        [Display(Name = "Cost of Reward")]
        public int RewardCost { get; set; }

        [Required]
        public int GroupId { get; set; }
    }
}
