using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FaculdadeSI.Startup))]
namespace FaculdadeSI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
