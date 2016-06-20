using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyApplication1.Startup))]
namespace MyApplication1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
