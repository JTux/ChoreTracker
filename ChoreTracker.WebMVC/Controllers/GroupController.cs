﻿using ChoreTracker.Models.CommentModels;
using ChoreTracker.Models.GroupModels;
using ChoreTracker.Services;
using ChoreTracker.Services.DataContract.Comment;
using ChoreTracker.Services.DataContract.Group;
using ChoreTracker.WebMVC.DataContract.Comment;
using ChoreTracker.WebMVC.DataContract.Group;
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
        public ActionResult Index(CommentCreateDTO model)
        {
            var svc = GetCommentService();

            var rao = new CommentCreateRAO
            {
                Content = model.Content,
                GroupId = model.GroupId,
                ParentId = model.ParentId
            };

            if (svc.CreateComment(rao))
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
        public ActionResult Create(GroupCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var svc = GetGroupService();
            var rao = new GroupCreateRAO { GroupName = model.GroupName };


            if (svc.CreateGroup(rao))
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
        public ActionResult JoinGroup(GroupJoinDTO model)
        {
            var svc = GetGroupService();

            if (!ModelState.IsValid)
                return View(model);

            var rao = new GroupJoinRAO { GroupInviteKey = model.GroupInviteKey };

            if (svc.JoinGroup(rao))
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Could not join group.");
            return View();
        }

        private GroupService GetGroupService() => new GroupService(Guid.Parse(User.Identity.GetUserId()));
        private CommentService GetCommentService() => new CommentService(Guid.Parse(User.Identity.GetUserId()));
    }
}