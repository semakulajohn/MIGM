using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Higgs.Mbale.Branch.Startup))]
namespace Higgs.Mbale.Branch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
