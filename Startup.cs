using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BackEnd.Startup))]
namespace BackEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
