using System.Windows;
using beRemote.Core.Definitions.Classes;

namespace beRemote.Core.Definitions.EventArgs
{
    public class ConnectEventArgs : RoutedEventArgs
    {
        public ConnectEventArgs()
        { }

        public ConnectEventArgs(ConnectionHost host, ConnectionProtocol protocol)
        {
            TargetSystem = host;
            TargetProtocol = protocol;
        }

        private ConnectionHost _TargetSystem;
        private ConnectionProtocol _TargetProtocol;

        public ConnectionHost TargetSystem { get { return _TargetSystem; } set { _TargetSystem = value; } }
        public ConnectionProtocol TargetProtocol { get { return _TargetProtocol; } set { _TargetProtocol = value; } }
    }
}
