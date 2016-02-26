using System;
using System.Collections.Generic;
using System.Text;

namespace beRemote.Core.Exceptions.Plugin.Protocol
{
    public class ProtocolConfigurationException : ProtocolException
    {
        //public ProtocolConfigurationException(beRemoteExInfoPackage infoPack) : base(infoPack) { }
        public ProtocolConfigurationException(beRemoteExInfoPackage infoPack, String message) : base(infoPack, message) { }
        public ProtocolConfigurationException(beRemoteExInfoPackage infoPack, String message, Exception innerEx) : base(infoPack, message, innerEx) { }

        public override int EventId
        {
            get { return 202; }
        }
    }
}
