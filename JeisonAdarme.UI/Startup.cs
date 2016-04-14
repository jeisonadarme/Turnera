using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JeisonAdarme.UI.Startup))]
namespace JeisonAdarme.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
