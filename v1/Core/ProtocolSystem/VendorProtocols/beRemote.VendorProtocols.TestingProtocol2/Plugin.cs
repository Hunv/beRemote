using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ProtocolSystem.ProtocolBase.Types;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.Core.ProtocolSystem.ProtocolBase;

namespace beRemote.VendorProtocols.TestingProtocol2
{
    public class Plugin : beRemote.Core.ProtocolSystem.ProtocolBase.Protocol
    {
        public override ServerType[] GetPrtocolCompatibleServers()
        {
            return new ServerType[] { ServerType.LINUX };
        }


        public override Session NewSession(UserCredentials credentials, IServer server)
        {
            throw new NotImplementedException();
        }
    }
}
