using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using beRemote.Core.Common.Helper;

namespace beRemote.VendorProtocols.HyperVManager
{
    public class HyperVManagerSession : Session
    {
        public HyperVManagerSession(IServer server, Protocol protocol, long dbConfigId) : base(server, protocol, dbConfigId) { }
        
        public override System.Windows.Controls.Control GetSessionWindow()
        {
            if (_sessionWindow == null)
            {
                _sessionWindow = new HyperVManagerSessionWindow(this);
            }

            return _sessionWindow;
        }

        public override void OnOpenConnection(string domuser, SecureString password)
        {
            if (_sessionWindow == null)
            {
                throw new ProtocolException(beRemoteExInfoPackage.MajorInformationPackage, "Session window not initialized!");
            }
            HyperVManagerSessionWindow sessionWnd = (HyperVManagerSessionWindow)_sessionWindow;
            //sessionWnd.OpenNewConnection(username, password);

            sessionWnd.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                     (System.Threading.ThreadStart)delegate()
                     {
                         //*** TEMPORARY UNTIL PLUGIN SYSTEM USES DEVIDED USER AND DOMAIN ***//
                         string username = domuser.Split('\\')[0];
                         string domain = "";
                         
                         if (domuser.Contains('\\'))
                            domain = domuser.Split('\\')[1];
                         //*** TEMPORARY UNTIL PLUGIN SYSTEM USES DEVIDED USER AND DOMAIN ***//

                         sessionWnd.OpenNewConnection(username , password, domain);
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
