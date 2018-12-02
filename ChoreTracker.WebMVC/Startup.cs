using System;
using ChoreTracker.WebMVC.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChoreTracker.WebMVC.Startup))]
namespace ChoreTracker.WebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "Josh";
                user.Email = "admin@admin.com";

                string password = "Test1!";

                var checkUser = userManager.Create(user, password);

                if (checkUser.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("GroupOwner"))
            {
                var role = new IdentityRole();
                role.Name = "GroupOwner";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("GroupMember"))
            {
                var role = new IdentityRole();
                role.Name = "GroupMember";
                roleManager.Create(role);
            }
        }
    }
}
