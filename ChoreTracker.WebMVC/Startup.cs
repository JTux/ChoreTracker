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
        }
    }
}
