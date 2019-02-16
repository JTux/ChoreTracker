using ChoreTracker.Services;
using ChoreTracker.Services.DataContract.Task;
using ChoreTracker.Web.DataContract.Task;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoreTracker.WebMVC.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        public ActionResult Index(int id)
        {
            var service = GetTaskService();
            var model = service.GetGroupTasks(id);

            ViewBag.GroupId = id;

            return View(model);
        }

        public ActionResult Create(int id)
        {
            var model = new TaskCreateDTO { GroupId = id };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskCreateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var rao = new TaskCreateRAO
            {
                GroupId = dto.GroupId,
                TaskName = dto.TaskName,
                TaskDescription = dto.TaskDescription,
                RewardPoints = dto.RewardPoints
            };

            var service = GetTaskService();
            if (service.CreateTask(rao))
            {
                return RedirectToAction("Index", new { id = dto.GroupId });
            }

            return View(dto);
        }

        private TaskService GetTaskService() => new TaskService(Guid.Parse(User.Identity.GetUserId()));
    }
}