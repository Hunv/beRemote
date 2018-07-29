using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces
{
    public interface IServer
    {
        /// <summary>
        /// Gets the remote server IPAddress object
        /// </summary>
        /// <returns></returns>
        System.Net.IPAddress GetRemoteIP();
        /// <summary>
        /// Gets the remote servers name. This is an decription object. Use GetServerHostname() to obtain the host name
        /// </summary>
        /// <returns></returns>
        String GetServerName();
        /// <summary>
        /// Gets the remote servers host name that is useable in webservices
        /// </summary>
        /// <returns></returns>
        String GetServerHostName();
        Types.ServerType GetServerType();
        void AddSession(Session session);
        Session GetSession(Guid sessionId);

        void AddProtocol(Protocol protocol);
        Protocol[] GetConfiguredProtocols();
        Protocol GetConfiguredProtocol(String name);

        int GetServerTypeID();
        long GetServerDBID();
        bool UsesVpn { get; }
        int VpnId { get; }
    }
}
