using System;
using System.Collections.Generic;
using System.Text;

namespace beRemote.Core.Common.LogSystem
{
    public enum EventID
    {
        Kernel = 20000,
        ExceptionSystem = 21000,
        PluginSystem = 22000,
        ProtocolSystem = 23000,
        StorageSystem = 24000,
        GUI = 25000,
        Licensing = 26000,
        Common = 27000,
        PluginRDP = 30000,
        PluginTelnet = 31000,
        PluginVNC = 32000,
        PluginSSH = 33000,

        [Obsolete("INVALID IS NOT VALID .. HRHR", false)]
        INVALID = 10000
    }
}


//50 Kernel
//51 ExceptionSystem
//52 PluginSystem
//53 ProtocolSystem
//54 StorageSystem
//55 GUI
//56 Licensing
//57 Common
//58 Common.LogSystem
//59
//60 RDP Plugin
//61 Telnet Plugin
//62 VNC Plugin