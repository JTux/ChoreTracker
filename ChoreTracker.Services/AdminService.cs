using ChoreTracker.Contracts;
using ChoreTracker.WebMVC.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<IdentityRole> GetRolesList()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var rolesList = ctx.Roles.ToList();
                return rolesList.ToArray();
            }
        }

        public bool IsAdminUser()
        {
            using (var ctx = new ApplicationDbContext())
            {
                try
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
                    var s = userManager.GetRoles(_userId.ToString());
                    if (s.Count != 0 && s[0].ToString() == "Admin")
                        return true;
                    else
                        return false;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
