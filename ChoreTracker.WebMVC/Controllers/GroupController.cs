using ChoreTracker.Services;
using ChoreTracker.Services.DataContract.Comment;
using ChoreTracker.Services.DataContract.Group;
using ChoreTracker.WebMVC.DataContract.Comment;
using ChoreTracker.WebMVC.DataContract.Group;
using ChoreTracker.WebMVC.Models.CommentModels;
using Microsoft.AspNet.Identity;
using System;
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
            if (!svc.CheckForExistingGroup())
            {
                if (User.IsInRole("GroupOwner"))
                    return RedirectToAction("Create");

                if (User.IsInRole("GroupMember"))
                    return RedirectToAction("JoinGroup");
            }

            var commentService = GetCommentService();
            var model = svc.GetGroupInfo();
            ViewBag.GroupMembers = svc.GetGroupMembers(model.GroupId);
            ViewBag.GroupApplicants = svc.GetApplicants(model.GroupId);
            ViewBag.Comments = commentService.GetGroupComments(model.GroupId);

            return View(model);
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
        public ActionResult Create(GroupCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var svc = GetGroupService();
            var rao = new GroupCreateRAO { GroupName = dto.GroupName };


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinGroup(GroupJoinDTO dto)
        {
            var svc = GetGroupService();

            if (!ModelState.IsValid)
                return View(dto);

            var rao = new GroupJoinRAO { GroupInviteKey = dto.GroupInviteKey };

            if (svc.JoinGroup(rao))
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Could not join group.");
            return View();
        }

        private GroupService GetGroupService() => new GroupService(Guid.Parse(User.Identity.GetUserId()));
        private CommentService GetCommentService() => new CommentService(Guid.Parse(User.Identity.GetUserId()));
    }
}