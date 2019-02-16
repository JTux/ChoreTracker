using ChoreTracker.Services;
using ChoreTracker.Services.DataContract.Reward;
using ChoreTracker.Web.DataContract.Reward;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace ChoreTracker.WebMVC.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class RewardController : Controller
    {
        // GET: Reward
        public ActionResult Index(int id)
        {
            var svc = CreateRewardService();
            var model = svc.GetRewards(id);

            ViewBag.GroupId = id;

            return View(model);
        }

        public ActionResult Create(int id)
        {
            var model = new RewardCreateDTO { GroupId = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RewardCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var svc = CreateRewardService();
            var rao = new RewardCreateRAO
            {
                GroupId = dto.GroupId,
                RewardCost = dto.RewardCost,
                RewardDescription = dto.RewardDescription,
                RewardName = dto.RewardName
            };

            if (svc.CreateReward(rao))
                return RedirectToAction("Index", "Reward", new { id = dto.GroupId });

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Claim(RewardClaimDTO dto)
        {
            if (!ModelState.IsValid)
                return View();

            var rao = new RewardClaimRAO { GroupId = dto.GroupId, RewardId = dto.RewardId, ClaimedCount = dto.ClaimedCount, RewardCost = dto.RewardCost };

            var svc = CreateRewardService();

            if (svc.ClaimReward(rao)) return RedirectToAction("Index", "Group", new { id = dto.GroupId });
            return RedirectToAction("Index", "Group", new { id = dto.GroupId });
        }

        public ActionResult Details(int id)
        {
            var svc = CreateRewardService();
            var dto = svc.GetRewardById(id);
            return View(dto);
        }

        private RewardService CreateRewardService() => new RewardService(Guid.Parse(User.Identity.GetUserId()));
    }
}