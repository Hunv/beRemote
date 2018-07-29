using System;
using System.Collections.Generic;
using System.Text;

namespace beRemote.Core.Exceptions.Plugin
{
    public class PluginException : beRemoteException
    {
        public PluginException(beRemoteExInfoPackage infoPackage) : base(infoPackage) { }
        public PluginException(beRemoteExInfoPackage infoPackage, String message) : base(infoPackage, message) { }
        public PluginException(beRemoteExInfoPackage infoPackage, String message, Exception innerEx) : base(infoPackage, message, innerEx) { }

        public override int EventId
        {
            get { return 101; }
        }
    }
}
