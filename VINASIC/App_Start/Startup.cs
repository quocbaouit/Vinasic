using Microsoft.Owin;
using Owin;
using VINASIC;

[assembly: OwinStartup(typeof(Startup))]

namespace VINASIC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
