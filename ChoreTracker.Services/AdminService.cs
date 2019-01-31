using ChoreTracker.Contracts;
using ChoreTracker.Models.AdminModels;
using ChoreTracker.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using ChoreTracker.Services.DataContract.Admin;

namespace ChoreTracker.Services
{
    public class AdminService : IAdminService
    {
        private readonly Guid _userId;

        public AdminService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<ApplicationUser> GetUserList()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var userList = ctx.Users.ToList();
                return userList.ToArray();
            }
        }

        public bool CreateNewAdmin(AdminUserCreateRAO user)
        {
            if (user.Password != user.ConfirmPassword)
                return false;

            var entity = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email
            };

            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.Users.Where(u => u.Email == user.Email || u.UserName == user.UserName).Count() == 0)
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
                    userManager.Create(entity, user.Password);
                    var newAdmin = ctx.Users.FirstOrDefault(u => u.Email == user.Email);
                    userManager.AddToRole(newAdmin.Id, "Admin");
                    return true;
                }
                else
                    return false;
            }
        }

        public IEnumerable<IdentityRole> GetRolesList()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var rolesList = ctx.Roles.ToList();
                return rolesList.ToArray();
            }
        }

        public void CreateNewRole(IdentityRole role)
        {
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Roles.Add(role);
                ctx.SaveChanges();
            }
        }

        public bool IsAdminUser()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
                var s = userManager.GetRoles(_userId.ToString());
                if (s.Count != 0 && s[0].ToString() == "Admin")
                    return true;
                else
                    return false;
            }
        }
    }
}
