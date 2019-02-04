using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.WebMVC.DataContract.Reward
{
    public class RewardDetailDTO
    {
        [Display(Name = "Reward Id")]
        public int RewardId { get; set; }

        [Display(Name = "Name of Reward")]
        public string RewardName { get; set; }

        [Display(Name = "Reward Description")]
        public string RewardDescription { get; set; }

        [Display(Name = "Cost of Reward")]
        public int RewardCost { get; set; }
    }
}
