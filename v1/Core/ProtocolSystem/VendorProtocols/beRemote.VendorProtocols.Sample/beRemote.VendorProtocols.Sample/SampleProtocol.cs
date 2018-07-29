using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.ProtocolSystem.ProtocolBase.Types;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using System.ComponentModel.Composition;

namespace beRemote.VendorProtocols.Sample
{
    [ProtocolMetadata(
        PluginName = "Sample Protocol",
        PluginFullQualifiedName = "beRemote.VendorProtocols.Sample.SampleProtocol",
        PluginConfigFolder = "beRemote.VendorProtocols.Sample",
        PluginIniFile = "plugin.ini",
        PluginVersionCode = 1,
        ProtocolAuthIsHandled = false,
        PluginAuthor = "Sample Developer 1337",
        PluginAuthorMail = "pro@samp-dev.invalid",
        PluginWebsite = "www.no-domain.invalid")]
    [Export(typeof(Protocol))]
    public class SampleProtocol : Protocol
    {
        public override ServerType[] GetPrtocolCompatibleServers()
        {
            return new ServerType[] { ServerType.LINUX, ServerType.MACOS, ServerType.WINDOWS };
        }

        public override Session NewSession(IServer server, long dbConfigId)
        {
            return new SampleSession(server, this, dbConfigId);
        }
    }
}
