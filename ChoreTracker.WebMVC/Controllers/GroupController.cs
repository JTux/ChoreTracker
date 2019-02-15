using ChoreTracker.Services;
using ChoreTracker.Services.DataContract.Group;
using ChoreTracker.Web.DataContract.Group;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace ChoreTracker.WebMVC.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        // GET: Group/MyGroups
        public ActionResult MyGroups()
        {
            var svc = GetGroupService();
            var model = svc.GetMyGroups();
            return View(model);
        }

        // GET: Group
        public ActionResult Index(int id)
        {
            var svc = GetGroupService();
            if (svc.IsApplicant(id) || !svc.CheckForExistingGroup(id))
                return RedirectToAction("MyGroups");

            var commentSvc = GetCommentService();
            var model = svc.GetGroupInfo(id);

            ViewBag.GroupMembers = svc.GetGroupMembers(model.GroupId);
            ViewBag.GroupApplicants = svc.GetApplicants(model.GroupId);

            ViewBag.Comments = commentSvc.GetGroupComments(model.GroupId);

            return View(model);
        }

        // GET: Group/Create
        public ActionResult Create()
        {
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
                var id = svc.GetGroupIDByName(rao.GroupName);
                return RedirectToAction("Index", new { id });
            }

            ModelState.AddModelError("", "Group could not be created.");
            return View();
        }

        [ActionName("Join")]
        public ActionResult JoinGroup()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Join")]
        [ValidateAntiForgeryToken]
        public ActionResult JoinGroup(GroupJoinDTO dto)
        {
            var svc = GetGroupService();

            if (!ModelState.IsValid)
                return View(dto);

            var rao = new GroupJoinRAO { GroupInviteKey = dto.GroupInviteKey };

            if (svc.JoinGroup(rao))
            {
                var id = svc.GetGroupIDByKey(rao.GroupInviteKey);
                return RedirectToAction("Index", new { id });
            }

            ModelState.AddModelError("", "Could not join group.");
            return View();
        }

        public ActionResult Acceptance(int id, int groupId, bool accepted)
        {
            var svc = GetGroupService();
            var rao = new GroupAcceptanceRAO
            {
                GroupMemberId = id,
                GroupId = groupId,
                Accepted = accepted
            };

            if (svc.Acceptance(rao))
            {
                return RedirectToAction("Index", new { id = groupId });
            }

            return RedirectToAction("Index", new { id = groupId });
        }

        private GroupService GetGroupService() => new GroupService(Guid.Parse(User.Identity.GetUserId()));
        private CommentService GetCommentService() => new CommentService(Guid.Parse(User.Identity.GetUserId()));
    }
}