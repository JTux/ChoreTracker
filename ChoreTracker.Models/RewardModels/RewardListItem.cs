using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.RewardModels
{
    public class RewardListItem
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
    }
}
