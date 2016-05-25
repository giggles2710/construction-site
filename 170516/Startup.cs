using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_170516.Startup))]
namespace _170516
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
