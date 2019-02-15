using ChoreTracker.Services;
using ChoreTracker.Services.DataContract.Comment;
using ChoreTracker.WebMVC.DataContract.Comment;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace ChoreTracker.WebMVC.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentCreateDTO dto)
        {
            var svc = GetCommentService();

            var rao = new CommentCreateRAO
            {
                Content = dto.Content,
                GroupId = dto.GroupId,
                ParentId = dto.ParentId
            };

            if (svc.CreateComment(rao))
            {
                return RedirectToAction("Index", "Group", new { id = dto.GroupId });
            }
            return RedirectToAction("Index", "Group", new { id = dto.GroupId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CommentEditDTO dto)
        {
            var svc = GetCommentService();
            var rao = new CommentEditRAO
            {
                CommentId = dto.CommentId,
                Content = dto.Content,
                GroupId = dto.GroupId
            };
            if (svc.EditComment(rao)) return RedirectToAction("Index","Group", new { id = dto.GroupId });
            else return RedirectToAction("Index", "Group", new { id = dto.GroupId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CommentDetailDTO dto)
        {
            var svc = GetCommentService();
            var rao = new CommentDetailRAO
            {
                CommentId = dto.CommentId,
                Content = dto.Content,
                GroupId = dto.GroupId
            };
            if (svc.DeleteComment(rao)) return RedirectToAction("Index", "Group", new { id = dto.GroupId });
            else return RedirectToAction("Index", "Group", new { id = dto.GroupId });
        }

        private CommentService GetCommentService() => new CommentService(Guid.Parse(User.Identity.GetUserId()));
    }
}