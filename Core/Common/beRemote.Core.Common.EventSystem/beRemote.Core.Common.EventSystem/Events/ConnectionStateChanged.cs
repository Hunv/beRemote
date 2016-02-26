using System;
using System.Collections.Generic;

//using System.Text;
//using beRemote.Core.ProtocolSystem.ProtocolBase;

namespace beRemote.Core.Common.EventSystem.Events
{
    /// <summary>
    /// Delegate for conection closed event
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void ConnectionStateChangedEventHandler(object o, ConnectionStateChangedEventArgs e);

    public enum ConnectionState
    {
        NEW,
        CONNECTED,
        DISCONNECTED
    }

    /// <summary>
    /// Event arguments for ConnectionClosedEvent
    /// </summary>
    public class ConnectionStateChangedEventArgs : beRemoteEventArgs
    {
        //public readonly Server Server;
        //public readonly Protocol Protocol;
        //public readonly Session Session;

        //public ConnectionStateChangedEventArgs(Server server, Protocol protocol, Session session)
        //{
        //    Server = server;
        //    Protocol = protocol;
        //    Session = session;
        //}

        public readonly String DataBaseConnectionHostID;
        public readonly String DataBaseConnectionHostName;
        public readonly String ProtocolIdentifier;
        public readonly String SessionID;
        public readonly ConnectionState ConnectionState;

        public ConnectionStateChangedEventArgs(ConnectionState state, String connectionHostID, String connectionHostName, String protocolIdentifier, String sessionID)
        {
            DataBaseConnectionHostID = connectionHostID;
            DataBaseConnectionHostName = connectionHostName;
            ProtocolIdentifier = protocolIdentifier;
            SessionID = sessionID;

            ConnectionState = state;
        }

        public override string ToString()
        {
            String a = "HostId: {0} \r\nHostName: {1} \r\nProtocol: {2} \r\nSessionId: {3} \r\nState: {4}\r\n";
            return String.Format(a, DataBaseConnectionHostID,DataBaseConnectionHostName, ProtocolIdentifier, SessionID, ConnectionState);
        }
    }


}
