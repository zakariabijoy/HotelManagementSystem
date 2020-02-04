using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HotelManagementSystem.Startup))]
namespace HotelManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
