using System;
using System.Collections.Generic;
using System.Text;
using beRemote.Core.Common.PluginBase;
using beRemote.Core.ProtocolSystem.ProtocolBase.Types;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using System.ComponentModel.Composition;

namespace beRemote.VendorProtocols.RDP
{
    [ProtocolMetadata(
        PluginIniFile = "plugin.ini",
        PluginFullQualifiedName= "beRemote.VendorProtocols.RDP.RDProtocol",
        PluginConfigFolder = "beRemote.VendorProtocols.RDP",
        PluginName = "Remote Desktop Protocol",
        PluginVersionCode = 1,
        ProtocolAuthIsHandled=true,
        PluginAuthor = "beRemote Team <Benedikt Kröning>",
        PluginAuthorMail = "benediktkroening@beremote.net",
        PluginWebsite = "www.beremote.net",
        PluginType = PluginType.Protocol)]
    [Export(typeof(Protocol))]
    public class RDProtocol : Protocol
    {
        public override Core.ProtocolSystem.ProtocolBase.Types.ServerType[] GetPrtocolCompatibleServers()        
        {            
            return new ServerType[] { ServerType.WINDOWS };
        }

        public override Session NewSession(IServer server, long connSettingsID)
        {
            return new RdpSession(server, (Protocol)this, connSettingsID);
        }

    }
}
