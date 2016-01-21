using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Driver.WebSite.Startup))]
namespace Driver.WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
