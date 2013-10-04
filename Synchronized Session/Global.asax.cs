using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Synchronized_Session;
using System.Web.Caching;
using Microsoft.AspNet.SignalR;

namespace Synchronized_Session
{
    public class Global : HttpApplication
    {
        /// <summary>
        /// This dictionary is to store the individual timeouts for every session currently running
        /// </summary>
        public static IDictionary<string, DateTime> SessionExpiryDictionary = new System.Collections.Concurrent.ConcurrentDictionary<string, DateTime>();

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }             

        protected void Session_Start(object sender, EventArgs e)
        {
            //When session is initially created, place it in our dictionary
            AddOrExtendSessionDictionaryItem();

            //If our Timer object for this session is not in the cache, add it
            if (HttpRuntime.Cache[Context.Session.SessionID] == null)
                AddTask(Context.Session.SessionID, 60);
        }

        /// <summary>
        /// Method which extends or Adds a time when the session will expire.
        /// </summary>
        private void AddOrExtendSessionDictionaryItem()
        {
            //Find out how long we have until session is expired
            DateTime SessionWillExpire = DateTime.Now.AddMinutes(Context.Session.Timeout);

            //If we have an entry in the dictionary for the session Id, then extend it.
            if (SessionExpiryDictionary.ContainsKey(Context.Session.SessionID))
            {
                SessionExpiryDictionary[Context.Session.SessionID] = SessionWillExpire;
            }
            //Otherwise, add an entry with this key
            else
            {
                SessionExpiryDictionary.Add(Context.Session.SessionID, SessionWillExpire);
            }
        }

        private static CacheItemRemovedCallback OnCacheRemove = null;

        //Method which places an object into the cache
        private void AddTask(string name, int seconds)
        {
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            HttpRuntime.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        /// <summary>
        /// Method which runs when a cached item is expired
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <param name="r"></param>
        public void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {
            
            //Get the time when the session expires
            DateTime expires = SessionExpiryDictionary[k];
            
            //How much time do we have left?
            TimeSpan timeRemaining = expires.Subtract(DateTime.Now);

            // re-add our task so it recurs
            AddTask(k, Convert.ToInt32(v));

            //If expired, inform all clients in the session group to kick the user to the session expired page.
            if ((int)(timeRemaining.TotalMinutes) <= 0)
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<SessionHub>();
                hub.Clients.Group(k, new string[] { }).SessionExpired(k);
            }
            //If they have less than two minutes left, we will show the warning to all clients in the session group.
            else if ((int)(timeRemaining.TotalMinutes) <= 2)
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<SessionHub>();
                hub.Clients.Group(k, new string[] { }).SessionWarning(k);
            }
            //Otherwise, we call the session refreshed method. This method removes the warning popup from all clients in our group. 
            else
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<SessionHub>();
                hub.Clients.Group(k, new string[] { }).SessionRefreshed(k);
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //Whenever an request is begun, extend the session
            HttpContext context = HttpContext.Current;
            if (context != null && context.Session != null)
            {
                //Extend the session
                AddOrExtendSessionDictionaryItem();
                
                //This will cause all clients in the group to close their popups if they are open.
                var hub = GlobalHost.ConnectionManager.GetHubContext<SessionHub>();
                hub.Clients.Group(context.Session.SessionID, new string[] { }).SessionRefreshed(context.Session.SessionID);
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }    
}
