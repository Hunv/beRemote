using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using System.Windows.Controls;
using beRemote.VendorProtocols.TestingProtocol.GUI;
using beRemote.Core.ProtocolSystem.ProtocolBase.Declaration;
using beRemote.Core.ProtocolSystem.ProtocolBase.Types;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;

namespace beRemote.VendorProtocols.TestingProtocol
{
    public class TestProtocol : beRemote.Core.ProtocolSystem.ProtocolBase.Protocol
    {
        public override ServerType[] GetPrtocolCompatibleServers()
        {
            return new ServerType[] { ServerType.WINDOWS, ServerType.LINUX };
        }


        public override Session NewSession(UserCredentials credentials, IServer server)
        {
            throw new NotImplementedException();
        }
    }
}
