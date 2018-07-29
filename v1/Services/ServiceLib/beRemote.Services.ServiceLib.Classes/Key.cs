using System;

namespace beRemote.Services.ServiceLib.Classes
{
    public static class Key
    {
        public static String SessionAuthenticator { get { return "SessionAuthenticator"; } }
        public static String QueryStringAction { get { return "Action"; } }
        public static string SessionMefLoader { get { return "SessionMefLoader"; } }
        public static string QueryStringListener { get { return "Listener"; } }
        public static string ClientHeader { get { return "beRemote.Client"; } }
        public static string ClientChannel { get { return "beRemote.ClientChannel"; } }
    }
}