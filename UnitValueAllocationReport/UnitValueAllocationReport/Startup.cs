using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UnitValueAllocationReport.Startup))]
namespace UnitValueAllocationReport
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
