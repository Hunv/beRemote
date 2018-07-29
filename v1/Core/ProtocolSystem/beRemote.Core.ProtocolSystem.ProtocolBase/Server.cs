using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.Core.Common.Helper;
using System.Net;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.ProtocolSystem.ProtocolBase.Types;
using System.Text.RegularExpressions;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.Exceptions;

namespace beRemote.Core.ProtocolSystem.ProtocolBase
{
    public class Server : Interfaces.IServer
    {
        private Types.ServerType _serverType;
        //private String _serverName;
        //private System.Net.IPAddress _remoteIP;
        private Dictionary<Guid, Session> _sessions;
        private Dictionary<String, Protocol> _configuredProtocols;

        private ConnectionHost _dbConnection;

        private string loggerContext = "ProtocolSystem";

        public bool UsesVpn
        {
            get {
                return _dbConnection.Vpn > 0;
            }
        }

        public int VpnId
        {
            get { return _dbConnection.Vpn; }
        }
        public Server(long serverID)
        {
            

            _sessions = new Dictionary<Guid, Session>();
            _configuredProtocols = new Dictionary<String, Protocol>();
            _dbConnection = StorageCore.Core.GetConnection(serverID);

            Logger.Log(LogEntryType.Debug, String.Format("Initiating new Server object ({0})", this), loggerContext);
        }




        #region Get IDs


        public int GetServerTypeID()
        {
            return _dbConnection.OS;
        }


        public long GetServerDBID()
        {
            return _dbConnection.ID;
        }
        #endregion

        public static Interfaces.IServer NewServer(long serverID)
        {
            return new Server(serverID);
        }

        /// <summary>
        /// Gets the ip
        /// </summary>
        /// <returns></returns>
        public IPAddress GetRemoteIP()
        {
            Logger.Log(LogEntryType.Debug, "Resolving hostname to ip if possible for server object with db id " + _dbConnection.ID, loggerContext);

            String host = _dbConnection.Host;

            if (IsValidIP(host))
            {
                return IPAddress.Parse(_dbConnection.Host);
            }
            else
            {
                try
                {
                    IPHostEntry hostEintrag = Dns.GetHostEntry(_dbConnection.Host);
                    return hostEintrag.AddressList[0];
                }
                catch (Exception ex)
                {
                    Logger.Log(LogEntryType.Exception, "Problem resolving hostname to ip for server object with db id " + _dbConnection.Host, ex, loggerContext);
                    Logger.Warning("Returning a 0.0.0.0 as IP in order to keep all following processes working");
                    //This is not an Execption that should result in a crash!
                    //throw new ProtocolConfigurationException(beRemoteExInfoPackage.MajorInformationPackage, "Problem resolving hostname to ip (Hostname: " + _dbConnection.Host + ")", ex);
                    return (IPAddress.Parse("0.0.0.0"));
                }
            }
        }

        public string GetServerName()
        {
            Logger.Log(LogEntryType.Verbose, "Returning servername (displayname) for server object with db id " + _dbConnection.ID, loggerContext);
            return _dbConnection.Name;
        }

        public string GetServerHostName()
        {
            Logger.Log(LogEntryType.Verbose, "Returning servername (hostname) for server object with db id " + _dbConnection.ID, loggerContext);
            return _dbConnection.Host;
        }

        public ServerType GetServerType()
        {
            var db_connection_os = StorageCore.Core.GetVersion(GetServerTypeID());

            switch (db_connection_os.getDistribution().ToLower())
            {
                case "windows":
                    return ServerType.WINDOWS;
                case "linux":
                    return ServerType.LINUX;
                case "apple":
                    return ServerType.MACOS;
                default :
                    return ServerType.UNASSIGNED;
            }
        }

        public void AddSession(Session session)
        {
            Logger.Log(LogEntryType.Verbose, "Adding new session to server object with db id " + _dbConnection.ID, loggerContext);
            _sessions.Add(session.GetSessionID(), session);
        }
        
        public Session GetSession(Guid sessionId)
        {
            Logger.Log(LogEntryType.Verbose, "Returning active session with id " + sessionId.ToString() + " for server object with DB id " + _dbConnection.ID, loggerContext);
            return _sessions[sessionId];
        }

        public Session[] GetSessions()
        {
            Logger.Log(LogEntryType.Verbose, "Returning all active session object for server object with DB id " + _dbConnection.ID, loggerContext);
            return _sessions.Values.ToArray();
        }


        public void AddProtocol(Protocol protocol)
        {
            Logger.Log(LogEntryType.Verbose, "Adding new protocol to server object with DB id " + _dbConnection.ID, loggerContext);

            if (_configuredProtocols == null)
                _configuredProtocols = new Dictionary<string, Protocol>();

            if (!_configuredProtocols.Keys.Contains(protocol.GetProtocolName()))
                _configuredProtocols.Add(protocol.GetProtocolName(), protocol);
        }
        
        public Protocol[] GetConfiguredProtocols()
        {
            Logger.Log(LogEntryType.Verbose, "Returning current configured list of protocols for server object with DB id " + _dbConnection.ID, loggerContext);
            return _configuredProtocols.Values.ToArray();
        }

        public Protocol GetConfiguredProtocol(string name)
        {
            Logger.Log(LogEntryType.Verbose, "Returning the protocol object for " + name + " that is configured in server object with DB id " + _dbConnection.ID, loggerContext);
            return _configuredProtocols[name];
        }

        public override string ToString()
        {
            String retVal = base.ToString() + "\r\n";

            retVal += String.Format("Server name: {0} ({1})\r\nServer ip: {2}\r\nSession count: {3}\r\nProtocol count: {4}", _dbConnection.ID, _serverType, _dbConnection.Name, _sessions.Count, _configuredProtocols.Count);

            return retVal;
        }

        public void SetSessionDebugMode(Guid sessionID, bool state)
        {
            this.GetSession(sessionID).SetDebugMode(state);
        }

        /// <summary>
        /// method to validate an IP address
        /// using regular expressions. The pattern
        /// being used will validate an ip address
        /// with the range of 0.0.2.2 to 255.255.255.255
        /// </summary>
        /// <param name="addr">Address to validate</param>
        /// <returns></returns>
        public bool IsValidIP(string addr)
        {
            IPAddress i_out;
            return IPAddress.TryParse(addr, out i_out);
        }





    }
}
