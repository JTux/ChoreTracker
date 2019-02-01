using System;
using System.Linq;
using ChoreTracker.Data;
using ChoreTracker.Services;
using ChoreTracker.Services.DataContract.Group;
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
            CreateAdminGroup();
        }

        private void CreateAdminGroup()
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.Groups.Where(g => g.GroupName == "Admins").Count() == 0)
                {
                    var adminGuid = Guid.Parse(ctx.Users.FirstOrDefault(u => u.Email == "admin@admin.com").Id);
                    var groupService = new GroupService(adminGuid);
                    groupService.CreateGroup(new GroupCreateRAO { GroupName = "Admins" });
                }
            }
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
                user.UserName = "Admin";
                user.Email = "admin@admin.com";
                user.FirstName = "Mr";
                user.LastName = "Admin";

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
