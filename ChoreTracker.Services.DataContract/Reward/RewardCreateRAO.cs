namespace ChoreTracker.Services.DataContract.Reward
{
    public class RewardCreateRAO
    {
        public int GroupId { get; set; }

        public string RewardName { get; set; }

        public string RewardDescription { get; set; }

        public int RewardCost { get; set; }
    }
}
