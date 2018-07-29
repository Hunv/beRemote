using System;
using System.Collections.Generic;
using System.Text;

namespace beRemote.Core.Exceptions.Plugin
{
    public class PluginConfigurationException : beRemoteException
    {
        public PluginConfigurationException(beRemoteExInfoPackage infoPack) : base(infoPack) { }
        public PluginConfigurationException(beRemoteExInfoPackage infoPack, String message) : base(infoPack, message) { }
        public PluginConfigurationException(beRemoteExInfoPackage infoPack, String message, Exception innerEx) : base(infoPack, message, innerEx) { }

        public override int EventId
        {
            get { return 102; }
        }
    }
}
