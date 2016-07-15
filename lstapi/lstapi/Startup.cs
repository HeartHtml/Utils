using lst;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace lst
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
