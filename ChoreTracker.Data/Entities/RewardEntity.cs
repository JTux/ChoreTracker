using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Data.Entities
{
    public class RewardEntity
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
