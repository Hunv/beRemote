using System;
using System.Collections.Generic;
using System.Text;

namespace beRemote.VendorProtocols.RDP
{
    public static class EventId
    {
        public static int OpenConnection = 1;
        public static int CloseConnection = 2;
        public static int NewSessionWindow = 3;

        public static int SessionWindowResize = 4;
    }
}
