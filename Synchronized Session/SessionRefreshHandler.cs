using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Synchronized_Session
{
    public class SessionRefreshHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            //Placing a variable in session, just for testing purposes
            context.Session["CheckSession"] = "OK";
            //Writing a response for testing purposes
            context.Response.Write("ok");

            //Grab the hub and tell all clients in the same session group that they need to close their popup
            var hub = GlobalHost.ConnectionManager.GetHubContext<SessionHub>();
            var connectionId = context.Request.QueryString["connectionId"];
            hub.Clients.Group(context.Session.SessionID, new string[] { connectionId }).SessionRefreshed(context.Session.SessionID);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
