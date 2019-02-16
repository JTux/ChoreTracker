namespace ChoreTracker.Services.DataContract.Reward
{
    public class RewardClaimRAO
    {
        public int RewardId { get; set; }
        public int ClaimedCount { get; set; }
        public int RewardCost { get; set; }
        public int GroupId { get; set; }
    }
}
