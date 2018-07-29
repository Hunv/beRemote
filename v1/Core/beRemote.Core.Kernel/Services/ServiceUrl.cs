using System;

namespace beRemote.Core.Services
{
    public static class ServiceUrl
    {
        public static String Login { get { return "/Default.aspx"; } }
        public static String Logout { get { return "/Default.aspx?Listener=Session&Action=Logout"; } }
    }
}
