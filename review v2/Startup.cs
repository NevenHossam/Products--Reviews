using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(review_v2.Startup))]
namespace review_v2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
