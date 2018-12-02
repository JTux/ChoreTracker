using ChoreTracker.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoreTracker.WebMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var service = CreateAdminService();

            if (service.IsAdminUser())
                return View();

            return RedirectToAction("Error");
        }

        //-- Will control view for viewing all users through an admin portal
        public ActionResult UserIndex()
        {
            var service = CreateAdminService();

            if (!service.IsAdminUser())
                return RedirectToAction("Index", "Home");

            var users = service.GetUserList();

            return View(users);
        }

        //-- Will control view for viewing roles through an admin portal
        public ActionResult RoleIndex()
        {
            var service = CreateAdminService();

            if (!service.IsAdminUser())
                return RedirectToAction("Index", "Home");

            var roles = service.GetRolesList();

            return View(roles);
        }

        //-- Will control view for creating roles through an admin portal
        public ActionResult RoleCreate()
        {
            var service = CreateAdminService();

            if (!service.IsAdminUser())
                return RedirectToAction("Index", "Home");

            var role = new IdentityRole();
            return View(role);
        }

        //-- Will control view for creating roles through an admin portal
        [HttpPost]
        public ActionResult RoleCreate(IdentityRole role)
        {
            var service = CreateAdminService();

            if (!service.IsAdminUser())
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Index");
        }

        public ActionResult Error()
        {
            return View();
        }

        private AdminService CreateAdminService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new AdminService(userId);
            return service;
        }
    }
}