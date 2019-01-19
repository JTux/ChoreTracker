﻿using ChoreTracker.Models.CommentModels;
using ChoreTracker.Models.GroupModels;
using ChoreTracker.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChoreTracker.WebMVC.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        // GET: Group
        public ActionResult Index()
        {
            var svc = GetGroupService();
            var commentService = GetCommentService();
            if (!svc.CheckForExistingGroup())
            {
                if (User.IsInRole("GroupOwner"))
                    return RedirectToAction("Create");

                if (User.IsInRole("GroupMember"))
                    return RedirectToAction("JoinGroup");
            }

            var model = svc.GetGroupInfo();
            ViewBag.GroupMembers = svc.GetGroupMembers(model.GroupId);
            ViewBag.GroupApplicants = svc.GetApplicants(model.GroupId);
            ViewBag.Comments = commentService.GetGroupComments(model.GroupId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CommentCreate model)
        {
            var svc = GetCommentService();

            if (svc.CreateComment(model))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Group/Create
        public ActionResult Create()
        {
            var svc = GetGroupService();
            if (svc.CheckForExistingGroup())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var svc = GetGroupService();

            if (svc.CreateGroup(model))
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Group could not be created.");
            return View();
        }

        public ActionResult JoinGroup()
        {
            var svc = GetGroupService();

            if (svc.CheckForExistingGroup())
                return RedirectToAction("Index");

            return View();
        }

        public ActionResult ModalPopUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModalPopUp(int i)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinGroup(GroupJoin model)
        {
            var svc = GetGroupService();

            if (!ModelState.IsValid)
                return View(model);

            if (svc.JoinGroup(model))
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Could not join group.");
            return View();
        }

        private GroupService GetGroupService() => new GroupService(Guid.Parse(User.Identity.GetUserId()));
        private CommentService GetCommentService() => new CommentService(Guid.Parse(User.Identity.GetUserId()));
    }
}