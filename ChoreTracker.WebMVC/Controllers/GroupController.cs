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

            TempData["FailResult"] = "Cannot create group.";
            return View();
        }

        // GET: Group/UpdateInviteKey
        public ActionResult EditInviteKey(int groupId)
        {
            var service = GetGroupService();
            if (service.EditGroupInviteKey(groupId))
                return RedirectToAction("Index", new { id = groupId });

            TempData["FailResult"] = "Cannot edit key.";
            return RedirectToAction("Index", new { id = groupId });
        }

        // GET: Group/Join
        [ActionName("Join")]
        public ActionResult JoinGroup()
        {
            return View();
        }

        // POST: Group/Join
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

            TempData["FailResult"] = "Cannot join group.";
            return View();
        }

        public ActionResult Leave(int id)
        {
            var svc = GetGroupService();
            var group = svc.GetGroupInfo(id);

            var rao = new GroupLeaveRAO { GroupId = group.GroupId, GroupInviteKey = group.GroupInviteKey };

            if (svc.LeaveGroup(rao))
                return RedirectToAction("MyGroups");

            TempData["FailResult"] = "Cannot leave group.";
            return RedirectToAction("Index", new { id });
        }

        public ActionResult KickMember(int memberId, int groupId)
        {
            var svc = GetGroupService();

            if (svc.KickMember(memberId))
                return RedirectToAction("Index", new { id = groupId });

            TempData["FailResult"] = "Cannot kick member.";
            return RedirectToAction("Index", new { id = groupId });
        }

        public ActionResult Acceptance(GroupAcceptanceDTO dto)
        {
            var svc = GetGroupService();
            var rao = new GroupAcceptanceRAO
            {
                GroupMemberId = dto.GroupMemberId,
                GroupId = dto.GroupId,
                Accepted = dto.Accepted
            };

            if (svc.Acceptance(rao))
                return RedirectToAction("Index", new { id = dto.GroupId });

            TempData["FailResult"] = "Cannot accept applicant.";
            return RedirectToAction("Index", new { id = dto.GroupId });
        }

        public ActionResult GrantMod(GroupPromoteDTO dto)
        {
            var svc = GetGroupService();
            var rao = new GroupPromoteRAO
            {
                GroupId = dto.GroupId,
                GroupMemberId = dto.GroupMemberId
            };
            if(svc.PromoteMember(rao))
                return RedirectToAction("Index", new { id = dto.GroupId });

            TempData["FailResult"] = "Cannot promote member.";
            return RedirectToAction("Index", new { id = dto.GroupId });
        }

        private GroupService GetGroupService() => new GroupService(Guid.Parse(User.Identity.GetUserId()));
        private CommentService GetCommentService() => new CommentService(Guid.Parse(User.Identity.GetUserId()));
    }
}