using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.ProtocolSystem.ProtocolBase.Types;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using System.ComponentModel.Composition;

namespace beRemote.VendorProtocols.IETrident
{
    [ProtocolMetadata(
        PluginName = "IETrident Protocol",
        PluginFullQualifiedName = "beRemote.VendorProtocols.IETrident.IETridentProtocol",
        PluginConfigFolder = "beRemote.VendorProtocols.IETrident",
        PluginIniFile = "plugin.ini",
        PluginVersionCode = 1,
        ProtocolAuthIsHandled=true)]
    [Export(typeof(Protocol))]
    public class IETridentProtocol : Protocol
    {
        public override ServerType[] GetPrtocolCompatibleServers()
        {
            return new ServerType[] { ServerType.LINUX, ServerType.MACOS, ServerType.WINDOWS };
        }

        public override Session NewSession(IServer server, long dbConfigId)
        {
            return new IETridentSession(server, this, dbConfigId);
        }
    }
}
