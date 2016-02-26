using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.Exceptions;
using beRemote.Core.Common.LogSystem;

namespace beRemote.VendorProtocols.RDP
{
    public enum RdpSessionState
    {
        StartNew,        
        StartNewWithResize,
        Opened,
        OpenedResizeMessage,
        OpenedResizeConnection,
        Closed


    }

    public class RdpSession : Session
    {
       
        public RdpSession(IServer server, Protocol protocol, long conOptionId) 
            : base(server, protocol, conOptionId)
        {
         
        }

        public override void CloseConnection()
        {
            if (_sessionWindow == null)
            {
                throw new ProtocolException(beRemoteExInfoPackage.MajorInformationPackage, "Session window not initialized!");
            }

            Logger.Verbose("[Session: " + GetSessionID() + "] Closing connection (" + GetSessionServer().GetRemoteIP() + ")", EventId.CloseConnection);

            var sessionWnd = (GUI.SessionWindow)_sessionWindow;
            var ax = sessionWnd.GetRdpControl();
            ax.rdpControl.Disconnect();
                        
            ax.rdpControl.Dispose();
            ax.rdpControl = null;

            // Triggering upper close connection event!
            TriggerCloseConnectionEvent();
        }
       
        internal RdpSessionState SessionState = RdpSessionState.Closed;

        public override void OnOpenConnection(String username, SecureString password)
        {
            if (_sessionWindow == null)
            {
                throw new ProtocolException(beRemoteExInfoPackage.MajorInformationPackage, "Session window not initialized!");
            }
            var sessionWnd = (GUI.SessionWindow)_sessionWindow;
            //sessionWnd.OpenNewConnection(username, password);

            sessionWnd.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                     (System.Threading.ThreadStart)delegate 
                     {
                         sessionWnd.OpenNewConnection(username, password);
                     }
                       );
        }

        public override System.Windows.Controls.Control GetSessionWindow()
        {
            return _sessionWindow ?? (_sessionWindow = new GUI.SessionWindow(this));
        }
    }
}
