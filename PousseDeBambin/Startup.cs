using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PousseDeBambin.Startup))]
namespace PousseDeBambin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
