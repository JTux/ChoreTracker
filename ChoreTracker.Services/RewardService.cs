using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Services.DataContract.Reward;
using ChoreTracker.WebMVC.DataContract.Reward;
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
                GroupId = groupService.GetGroupInfo().GroupId
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Rewards.Add(reward);
                return ctx.SaveChanges() == 1;
            }
        }

        public List<RewardListItemDTO> GetRewards()
        {
            var groupService = new GroupService(_userId);

            var groupId = groupService.GetGroupInfo().GroupId;

            using (var ctx = new ApplicationDbContext())
            {
                return ctx
                    .Rewards
                    .Where(r => r.GroupId == groupId)
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
                var user = ctx.Users.Single(u => u.Id == _userId.ToString());
                var entity = ctx.Rewards.Single(r => r.RewardId == rao.RewardId);
                
            }
            return true;
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
