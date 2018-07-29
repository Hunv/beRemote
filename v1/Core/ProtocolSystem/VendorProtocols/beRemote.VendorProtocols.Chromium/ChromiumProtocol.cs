using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.ProtocolSystem.ProtocolBase.Types;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using System.ComponentModel.Composition;

namespace beRemote.VendorProtocols.Chromium
{
    [ProtocolMetadata(
        PluginName = "Chromium Protocol",
        PluginFullQualifiedName = "beRemote.VendorProtocols.Chromium.ChromiumProtocol",
        PluginConfigFolder = "beRemote.VendorProtocols.Chromium",
        PluginIniFile = "plugin.ini",
        PluginVersionCode = 1,
        ProtocolAuthIsHandled=true)]
    [Export(typeof(Protocol))]
    public class ChromiumProtocol : Protocol
    {
        public override ServerType[] GetPrtocolCompatibleServers()
        {
            return new ServerType[] { ServerType.LINUX, ServerType.MACOS, ServerType.WINDOWS };
        }

        public override Session NewSession(IServer server, long dbConfigId)
        {
            return new ChromiumSession(server, this, dbConfigId);
        }
    }
}
