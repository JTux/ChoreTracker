using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Services.DataContract.Reward;
using ChoreTracker.Web.DataContract.Reward;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChoreTracker.Services
{
    public class RewardService
    {
        private Guid _userId;
        public RewardService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateReward(RewardCreateRAO model)
        {
            var groupService = new GroupService(_userId);

            var reward = new RewardEntity
            {
                RewardName = model.RewardName,
                Description = model.RewardDescription,
                Cost = model.RewardCost,
                GroupId = model.GroupId
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Rewards.Add(reward);
                return ctx.SaveChanges() == 1;
            }
        }

        public List<RewardListItemDTO> GetRewards(int id)
        {
            var groupService = new GroupService(_userId);

            using (var ctx = new ApplicationDbContext())
            {
                return ctx
                    .Rewards
                    .Where(r => r.GroupId == id)
                    .Select(r =>
                        new RewardListItemDTO
                        {
                            RewardId = r.RewardId,
                            RewardName = r.RewardName,
                            RewardDescription = r.Description,
                            RewardCost = r.Cost
                        })
                        .ToList();
            }
        }

        public bool ClaimReward(RewardClaimRAO rao)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var groupMember = ctx.GroupMembers.Single(gm => gm.MemberId == _userId && gm.GroupId == rao.GroupId);
                var reward = ctx.Rewards.Single(r => r.RewardId == rao.RewardId);

                if (groupMember.EarnedPoints < (reward.Cost * rao.ClaimedCount))
                    return false;

                var entity = new ClaimedRewardEntity
                {
                    OwnerId = groupMember.MemberId,
                    RewardId = reward.RewardId,
                    ClaimedCount = rao.ClaimedCount
                };

                ctx.ClaimedRewards.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public RewardDetailDTO GetRewardById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Rewards.Single(r => r.RewardId == id);
                return new RewardDetailDTO
                {
                    RewardId = entity.RewardId,
                    RewardCost = entity.Cost,
                    RewardName = entity.RewardName,
                    RewardDescription = entity.Description
                };
            }
        }
    }
}
