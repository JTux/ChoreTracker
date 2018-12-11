using ChoreTracker.Models.CommentModels;
using ChoreTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoreTracker.WebMVC.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var svc = GetCommentService();

            if (svc.CreateComment(model))
            {
                return RedirectToAction("Index");
            };

            return View(model);
        }

        private CommentService GetCommentService() => new CommentService(Guid.Parse(User.Identity.ToString()));
    }
}