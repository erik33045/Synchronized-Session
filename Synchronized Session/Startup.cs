using Microsoft.Owin;
using Owin;
using Synchronized_Session;

namespace Synchronized_Session
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Map all hubs to "/signalr"
            app.MapSignalR();            
        }      
    }
}