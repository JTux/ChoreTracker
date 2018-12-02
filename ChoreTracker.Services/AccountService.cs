using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoreTracker.Models.RoleModels;
using ChoreTracker.WebMVC.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChoreTracker.Services
{
    public class AccountService
    {
        private readonly Guid _userId;
        public AccountService(Guid userId)
        {
            _userId = userId;
        }

        public void AssignRole(RoleAssign model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                string group = "";
                switch (model.GroupChoice)
                {
                    default:
                    case Role.GroupOwner:
                        group = "GroupOwner";
                        break;
                    case Role.GroupMember:
                        group = "GroupMember";
                        break;
                }

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
                var user = ctx.Users.FirstOrDefault(u => u.Id == _userId.ToString());
                userManager.AddToRole(user.Id, group);
            }
        }
    }
}
