using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using beRemote.Core.Common.Helper;

namespace beRemote.VendorProtocols.Chromium
{
    public class ChromiumSession : Session
    {
        public ChromiumSession(IServer server, Protocol protocol, long dbConfigId) : base(server, protocol, dbConfigId) { }
        
        public override System.Windows.Controls.Control GetSessionWindow()
        {
            if (_sessionWindow == null)
            {
                _sessionWindow = new ChromiumSessionWindow(this);
            }

            return _sessionWindow;
        }

        public override void OnOpenConnection(string username, SecureString password)
        {
            if (_sessionWindow == null)
            {
                throw new ProtocolException(beRemoteExInfoPackage.MajorInformationPackage, "Chromium Session window not initialized!");
            }
            ChromiumSessionWindow sessionWnd = (ChromiumSessionWindow)_sessionWindow;            

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
