using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(adwWEB.Startup))]
namespace adwWEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
