using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Synchronized_Session
{
    public class SessionHub : Hub
    {
        public Task JoinSessionGroup(string connectionId, string sessionId)
        {
            return Groups.Add(Context.ConnectionId, sessionId);
        }

        public override Task OnConnected()
        {
            JoinSessionGroup(Context.ConnectionId, Context.QueryString["sessionId"]);
            return base.OnConnected();
        }
    }
}