using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SflStucco.Site.Startup))]
namespace SflStucco.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
