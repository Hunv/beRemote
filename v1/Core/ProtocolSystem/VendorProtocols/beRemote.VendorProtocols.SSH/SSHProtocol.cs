using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.Core.ProtocolSystem.ProtocolBase.Types;

namespace beRemote.VendorProtocols.SSH
{
    [ProtocolMetadata(
        PluginName = "SSH Protocol",
        PluginFullQualifiedName = "beRemote.VendorProtocols.SSH.SshProtocol",
        PluginConfigFolder = "beRemote.VendorProtocols.SSH",
        PluginIniFile = "plugin.ini",
        PluginVersionCode = 1,
        ProtocolAuthIsHandled = true,
        PluginAuthor = "Kristian Reukauff",
        PluginAuthorMail = "kristianreukauff@beremote.net",
        PluginWebsite = "www.beremote.net")]
    [Export(typeof (Protocol))]
    public class SshProtocol : Protocol
    {
        public override ServerType[] GetPrtocolCompatibleServers()
        {
            return new ServerType[] {ServerType.LINUX, ServerType.MACOS, ServerType.WINDOWS};
        }

        public override Session NewSession(IServer server, long dbConfigId)
        {
            return new SshSession(server, this, dbConfigId);
        }
    }
}
