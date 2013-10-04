This is the readme for the Synchronized Session test example. 

The relevant files that were edited/added were as follows:
  - RefreshSessionHandler.cs: Added this file to handle postbacks. When a postback happens, extend the session.
  - Startup.cs: Added this file to map signalR on startup.
  - Global.asax: Edited this file to handle events that keep track of when sessions timeout. This file contains the
    main logic that controls the session.
  - Site.Master: This file contains the javascript events bound to SignalR. These events will drive the behavior of what 
    happens when the global.asax calls the SignalR events from the server side code.
  - SessionHub.cs: This file contains the join group event that occurs whenever a client is added.
  - Default.aspx: Simple page to explain what will happen when the page expires.
  - SessionExpired.aspx: Page to throw the user to once the page expires.
  - Scripts Folder: Added relevant scripts for the jQuery popups and signalR
  - Packages: Installed the SignalR package.
  
  
  Here is an explanation of what all is happening and how it ties together.
  
  
  When the application is first started, signalR is initialized. When A user first requests to join a page two things will
  happen. The first is that the session will be created. When this happens, an entry is added to a dictionary that is
  stored with the sessionId as the key, and when the session will expire as the value. Next an item is added to the cache
  identified by the same sessionId which has a sixty second expiration. More on this later. 
  
  Now that the backend has been loaded, the master page loads. This does a number of things. It will connect the session
  hub, and register three events for the client. They are SessionWarning, SesionRefreshed, and session expired. I will delve
  into these in a bit. Next, signalR will add this client to a group also identified by the SessionId.
  Any tabs or windows that share the session Id will be in the same group. Following this, the default page is loaded and
  the request is finished. 
  
  Sixty seconds from when that cache item is created, it will expire. When this happens, we will re add the item to the
  cache and check our dictionary entry to see when the session expires. If there are greater than two minutes left, do
  nothing. If less than 2 but greater than 0, warn the user with the SessionWarning method on the master page. If less than
  or equal to 0, throw all clients sharing that session to the session expired page.
  
  On to the master page popups and what happens when the client methods are called from the server. When session warning is
  called, all clients in the group will be persented with a popup that asks them to extend the session. Should they choose
  yes, call the SessionRefreshed method and then postback to the handler and extend the session. If they choose no, only close
  that instance of the popup.
  
  The SessionRefreshed client method merely tells all clients in the group to close their popups at the same time.
  
  Any request to the server, postback or not will also extend the session. That means that the popup warning only appears if the
  user has been inactive for the entirety of the session, minus two minutes. 
  
  Finally the SessionExpired client method, this simply throws all clients in the group to the sessionExpired page.
  
  
  Notes:
  - This code was replicated from an existing implementation and has been changed to be more of a generic case/
  - Bugs are not expected with this simple of a project, but they can happen. Inform me or pull and fix!
  - My contact email is erik33045@gmail.com
