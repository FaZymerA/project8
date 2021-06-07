using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(project8.Startup))]
namespace project8
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
