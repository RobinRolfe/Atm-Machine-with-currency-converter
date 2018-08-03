using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Atm11.Startup))]
namespace Atm11
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
