using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Budgeteer_Web.Startup))]
namespace Budgeteer_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
