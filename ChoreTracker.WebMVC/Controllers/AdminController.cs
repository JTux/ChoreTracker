﻿using ChoreTracker.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web.Mvc;
using ChoreTracker.Services.DataContract.Admin;
using ChoreTracker.Web.DataContract.Admin;

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
        public ActionResult UserIndex(string[] dynamicField)
        {
            ViewBag.Data = string.Join(",", dynamicField ?? new string[] { });
            var service = CreateAdminService();

            if (!service.IsAdminUser())
                return RedirectToAction("Index", "Home");

            var users = service.GetUserList();

            return View(users);
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
        //-- Allows Admins to create another Admin Account
        public ActionResult AdminCreate()
        {
            var service = CreateAdminService();

            if (!service.IsAdminUser())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult AdminCreate(AdminUserCreateDTO user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var service = CreateAdminService();

            if (!service.IsAdminUser())
                return RedirectToAction("Index", "Home");

            var rao = new AdminUserCreateRAO
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                ConfirmPassword = user.ConfirmPassword
            };

            if (!service.CreateNewAdmin(rao))
                return View(user);

            return RedirectToAction("Index");
        }

        //-- Will be used to edit user eventually
        public ActionResult EditUser()
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
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            var service = CreateAdminService();

            if (!service.IsAdminUser())
                return RedirectToAction("Index", "Home");

            service.CreateNewRole(role);

            return RedirectToAction("Index");
        }

        //-- Controls view for Generic Error for now
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