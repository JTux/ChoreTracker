using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.WebMVC.DataContract.Reward
{
    public class RewardEditDTO
    {
        [Required]
        [Display(Name = "Reward Id")]
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
