using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using beRemote.Services.ServiceLib.Classes;
using beRemote.Services.ServiceLib.Classes.Clients;

namespace beRemote.Services.WebService.Authentication
{

    public class SessionStorage
    {
        private readonly HttpSessionState _session;
        public HttpSessionState Session { get { return _session; } }

        public MefLoader MefLoader
        {
            get { return (MefLoader) Session[Key.SessionMefLoader]; }
            set { Session[Key.SessionMefLoader] = value; }
        }

        public ClientType ClientType
        {
            get { return (ClientType)Session[Key.ClientHeader]; }
        }

        public SessionStorage(HttpSessionState session)
        {
            _session = session;
        }

        public static SessionStorage DoAuthentication(HttpContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException();
            }

            ClientType clientType = GetClientTypeFromContext(context);

            if (clientType != null)
            {
                //if (context.Request[Key.ClientKey] != null)
                //{
                //    if (context.Request[Key.ClientKey].Equals("13377331"))
                //    {
                //        var instance = new SessionStorage(context.Session);
                //        instance.Session[Key.SessionAuthenticator] = instance;
                //        return instance;
                //    }
                //}
                
                // ensure encryption

                var instance = new SessionStorage(context.Session);
                instance.Session[Key.SessionAuthenticator] = instance;
                instance.Session[Key.ClientHeader] = clientType;
                instance.Session[Key.ClientChannel] = context.Request.QueryString["ClientChannel"];
                return instance;

            }

            InvalidateSession(context);
            return null;
            
        }

        public bool IsCurrentClientTypeValid(HttpContext context)
        {
            return ClientType == GetClientTypeFromContext(context);
        }

        public static ClientType GetClientTypeFromContext(HttpContext context)
        {
            String clientHeader = null;
            for (int i = 0; i < context.Request.Headers.Count; i++)
            {
                string key = context.Request.Headers.GetKey(i);
                string value = context.Request.Headers.Get(i);

                if (key.Equals(Key.ClientHeader))
                {
                    clientHeader = value;
                }
            }

            if (clientHeader != null)
            {
                foreach (var client in ClientType.Clients)
                {
                    if (client.SecretKey.Equals(clientHeader))
                    {
                        return client;
                    }
                }
            }

            return null;
        }

        internal static void InvalidateSession(HttpContext context)
        {
            context.Session.Abandon();
            context.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
        }
    }
}