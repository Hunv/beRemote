using System.Security;
using System.Windows.Controls;
using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.VendorProtocols.Sample
{
    public class SampleSession : Session
    {
        public SampleSession(IServer server, Protocol protocol, long dbConfigId) : base(server, protocol, dbConfigId) { }
        
        public override Control GetSessionWindow()
        {
            if (_sessionWindow == null)
            {
                _sessionWindow = new SampleSessionWindow();
            }

            return _sessionWindow;
        }

        public override void OnOpenConnection(string username, SecureString password)
        {
            if (_sessionWindow == null)
            {
                throw new ProtocolException(beRemoteExInfoPackage.MajorInformationPackage, "Session window not initialized!");
            }
            SampleSessionWindow sessionWnd = (SampleSessionWindow)_sessionWindow;
            //sessionWnd.OpenNewConnection(username, password);

            sessionWnd.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                     (System.Threading.ThreadStart)delegate()
                     {
                         sessionWnd.OpenNewConnection(username, password);
                     }
                       );
        }
    
        public override void CloseConnection()
        {
            // Triggering upper close connection event!
            this.TriggerCloseConnectionEvent();
        }
    }
}
