using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chidono.Startup))]
namespace Chidono
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
