using ChoreTracker.Services;
using ChoreTracker.Services.DataContract.Reward;
using ChoreTracker.WebMVC.DataContract.Reward;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace ChoreTracker.WebMVC.Controllers
{
    [Authorize(Roles = "Admin,GroupOwner")]
    public class RewardController : Controller
    {
        // GET: Reward
        public ActionResult Index()
        {
            var svc = CreateRewardService();
            var model = svc.GetRewards();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RewardCreateDTO dto)
        {
            var svc = CreateRewardService();
            var rao = new RewardCreateRAO
            {
                RewardCost = dto.RewardCost,
                RewardDescription = dto.RewardDescription,
                RewardName = dto.RewardName
            };

            if (svc.CreateReward(rao))
            {
                return RedirectToAction("Index", "Group", null);
            }

            return View(dto);
        }

        private RewardService CreateRewardService() => new RewardService(Guid.Parse(User.Identity.GetUserId()));
    }
}