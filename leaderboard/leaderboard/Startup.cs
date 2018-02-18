using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(leaderboard.Startup))]
namespace leaderboard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
