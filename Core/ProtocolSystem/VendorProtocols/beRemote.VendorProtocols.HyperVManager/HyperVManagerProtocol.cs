using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.ProtocolSystem.ProtocolBase.Types;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using System.ComponentModel.Composition;

namespace beRemote.VendorProtocols.HyperVManager
{
    [ProtocolMetadata(
        PluginName = "HyperV Manager",
        PluginFullQualifiedName = "beRemote.VendorProtocols.HyperVManager.HyperVManagerProtocol",
        PluginConfigFolder = "beRemote.VendorProtocols.HyperVManager",
        PluginIniFile = "plugin.ini",
        PluginVersionCode = 1,
        ProtocolAuthIsHandled=true)]
    [Export(typeof(Protocol))]
    public class HyperVManagerProtocol : Protocol
    {
        public override ServerType[] GetPrtocolCompatibleServers()
        {
            return new ServerType[] { ServerType.LINUX, ServerType.MACOS, ServerType.WINDOWS };
        }

        public override Session NewSession(IServer server, long dbConfigId)
        {
            return new HyperVManagerSession(server, this, dbConfigId);
        }
    }
}
