using ChoreTracker.Models.RewardModels;
using ChoreTracker.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult Create(RewardCreate model)
        {
            var svc = CreateRewardService();
            if (svc.CreateReward(model))
            {
                return RedirectToAction("Index", "Group", null);
            }

            return View(model);
        }

        private RewardService CreateRewardService() => new RewardService(Guid.Parse(User.Identity.GetUserId()));
    }
}