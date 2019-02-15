﻿using ChoreTracker.Services;
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
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Claim(RewardClaimDTO dto)
        {
            if (!ModelState.IsValid)
                return View();

            var rao = new RewardClaimRAO { RewardId = dto.RewardId, ClaimedCount = dto.ClaimedCount, RewardCost = dto.RewardCost };

            var svc = CreateRewardService();

            if (svc.ClaimReward(rao)) return RedirectToAction("Index", "Group");
            return RedirectToAction("Index", "Group");
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