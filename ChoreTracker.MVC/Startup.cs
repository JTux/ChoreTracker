using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChoreTracker.MVC.Startup))]
namespace ChoreTracker.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
