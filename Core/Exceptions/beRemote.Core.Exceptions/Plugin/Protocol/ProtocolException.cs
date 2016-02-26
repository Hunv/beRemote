using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace beRemote.Core.Exceptions.Plugin.Protocol
{
    public class ProtocolException : PluginException
    {
        public ProtocolException(beRemoteExInfoPackage infoPackage, String message) : base(infoPackage, message) { }

        public ProtocolException(beRemoteExInfoPackage infoPackage, String message, Exception innerEx) : base(infoPackage, message, innerEx) { }

        public override int EventId
        {
            get { return 201; }
        }

        public override Action GetHandlerAction()
        {
            return new Action(HandlerVoid);
        }

        public void HandlerVoid()
        {
            MessageBox.Show("The following error occured:\r\n" + this.Message, "Problem starting connection", MessageBoxButton.OK, MessageBoxImage.Asterisk);

        }
    }
}
