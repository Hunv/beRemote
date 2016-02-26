using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.Core.ProtocolSystem.ProtocolBase.Types;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.Exceptions;
using beRemote.GUI.Notification;


namespace beRemote.Core.KernelHelper
{
    public class Host
    {
        private IServer _server;
        private List<Session> _sessions = new List<Session>();

        private long _dbFolderID;
        private Dictionary<long, Protocol> _configuredProtocols;

        private String _loggerContext = "Kernel";
        #region Constructors

        public Host(IServer server)
        {
            Logger.Log(LogEntryType.Info, String.Format("Initiating new Host object for server {0}", server), _loggerContext);
            this._server = server;

        }

        public Host()
        {

        }
        
        #endregion
        #region Get IDs
        public long GetFolderID()
        {
            return _dbFolderID;
        }

        public long GetConnectionID()
        {
            return _server.GetServerDBID();
        }
        #endregion
        #region Get Base objects

        /// <summary>
        /// Gets the Remote server object from this object or, if it is inititated, from the session object
        /// </summary>
        public IServer GetServer()
        {
            return this._server;
        }

        /// <summary>
        /// Returns a list of configured Protocols. 
        /// <b>Note: </b> This ist without IDs!
        /// </summary>
        public List<Protocol> GetProtocols()
        {
            if (_configuredProtocols == null)
            {
                _configuredProtocols = new Dictionary<long, Protocol>();
                foreach (ConnectionProtocol setting in StorageCore.Core.GetConnectionSettings(this.GetConnectionID()))
                {
                    if (Kernel.GetAvailableProtocols().ContainsKey(setting.Protocol)) //Only get available protocols
                    {
                        _configuredProtocols.Add(setting.Id, GetProtocol(setting.Id));
                    }
                }
            }
            return _configuredProtocols.Values.ToList();
        }

        public Protocol GetProtocol(long id)
        {
            ConnectionProtocol setting = StorageCore.Core.GetConnectionSetting(id);

            if (_configuredProtocols == null)
            {
                // if this list is empty we will create it
                GetProtocols();
                return GetProtocol(id);
            }
            else if (_configuredProtocols.ContainsKey(id))
            {
                return _configuredProtocols[id];
            }
            else if (Kernel.GetAvailableProtocols().ContainsKey(setting.Protocol))
            {
                return Kernel.GetAvailableProtocols()[setting.Protocol];
            }
            else
            {
                throw new ProtocolException(beRemoteExInfoPackage.SignificantInformationPackage, String.Format("Protocol {0} not found in Kernel", setting.Protocol));
            }
        }

        /// <summary>
        /// Returns a dictionary with configured protocols, including ids as key
        /// </summary>
        /// <returns></returns>
        public Dictionary<long, Protocol> GetProtocolIDs()
        {
            if (_configuredProtocols == null)
                GetProtocols();

            return _configuredProtocols;

        }

        public List<Session> GetSessions()
        {
            return _sessions;
        }
        
        #endregion
        #region Create NEW objects

        //public Session NewSession(String protocolIdentifier, int settingsID)
        //{
        //    if (_protocols.ContainsKey(protocolIdentifier))
        //        return NewSession(_protocols[protocolIdentifier], settingsID);
        //    else
        //        throw new ExceptionSystem.ExceptionBase.Exceptions.ProtocolInvalidException(String.Format("Given protocol identifer is not valid (ident: {0})", protocolIdentifier));
        //}

        //public Session NewSession(Protocol protocol, int settingsID)
        //{
        //    return protocol.NewSession(this._server, settingsID);
        //}

        public Session NewSession(long connectionSettingID)
        {
            Logger.Log(LogEntryType.Info, String.Format("Initiating new session object called from host"), _loggerContext);
            Logger.Log(LogEntryType.Debug, String.Format("... session object for server: {0}", _server), _loggerContext);
            Logger.Log(LogEntryType.Debug, String.Format("... session object for db connection id: {0}", GetConnectionID()), _loggerContext);
            Logger.Log(LogEntryType.Debug, String.Format("... session object for protocol: {0}", GetProtocol(connectionSettingID).GetProtocolName()), _loggerContext);

            ConnectionProtocol db_protocol = StorageCore.Core.GetConnectionSetting(connectionSettingID);

            if (!Kernel.GetAvailableProtocols().ContainsKey(db_protocol.Protocol))
                throw new ProtocolException(beRemoteExInfoPackage.SignificantInformationPackage, "Protocol with given identifier not found in Kernel (" + db_protocol.Protocol + "; db-id: " + db_protocol.Id.ToString() + ")");

            Protocol protocol = Kernel.GetAvailableProtocols()[db_protocol.Protocol];


            Session sess = protocol.NewSession(this._server, connectionSettingID);

            //try
            //{
            //    if (sess.IsInDebugMode())
            //        Kernel.GetDebugWorker().AddDebugObject("Sessions", sess);
            //}
            //catch (Exception ex)
            //{
            //    // if this is thrown we don't have a valid debugger
            //}

            return sess;
        }

        public static Host NewConnection(String hostorip, String name, String description, int osID, bool isPublic, int vpn)
        {
            return NewConnection(hostorip, name, description, osID, isPublic, StorageCore.Core.GetUserSettings().DefaultFolder, vpn);
        }

        public static Host NewConnection(String hostorip, String name, String description, int osID, bool isPublic, long folderID, int vpn)
        {
            String _loggerContext = "Kernel";
            Logger.Log(LogEntryType.Info, String.Format("Creating new connection object for {0}", hostorip), _loggerContext);
            Logger.Log(LogEntryType.Debug, String.Format("... description: ", description), _loggerContext);
            Logger.Log(LogEntryType.Debug, String.Format("... os db id: ", osID), _loggerContext);


            long newConnectionId = StorageCore.Core.AddConnection(hostorip, name, description, osID, folderID, StorageCore.Core.GetUserId(), isPublic, vpn);
            Logger.Log(LogEntryType.Debug, String.Format("... saved in storage with id: ", newConnectionId), _loggerContext);
            return LoadConnection((int)newConnectionId);
        }
        #endregion


        public int AddProtocol(String protocolIdentifier, int port, int CredentialId)
        {
            Logger.Log(LogEntryType.Info, String.Format("Adding protocol {0} to connection {1}", protocolIdentifier, GetConnectionID()), _loggerContext);

            long id = StorageCore.Core.AddConnectionSetting(_server.GetServerDBID(), protocolIdentifier, port,CredentialId);
            
            return (int)id;
        }

        public void AddProtocolOptions(int protID, Dictionary<String, Object> options)
        {
            foreach (KeyValuePair<string, object> kvp in options)
            {
                StorageCore.Core.ModifyConnectionOption(kvp.Value, kvp.Key, protID);
            }
        }

        public void SetFolderID(long folderID)
        {
            this._dbFolderID = folderID;
        }

        #region Statics
        private static Dictionary<long, Host> _hosts;

        /// <summary>
        /// Searches a connection with specified connectionId.
        /// </summary>
        /// <param name="connectionID"></param>
        /// <returns></returns>
        public static Host LoadConnection(long connectionID)
        {
            if (_hosts == null)
                _hosts = new Dictionary<long, Host>();

            //We don't need this buffer, I thing. Without this Bug#293 is fixed
            if (_hosts.ContainsKey(connectionID))
                return _hosts[connectionID];
            else
            {
                Host retCon = new Host(LoadServer(connectionID));
                retCon.SetFolderID(StorageCore.Core.GetConnection(connectionID).Folder);
                //_hosts.Add(connectionID, retCon);
                return retCon;
            }
        }

        /// <summary>
        /// Returns the <b>FIRST</b> matching host object. 
        /// </summary>
        /// <param name="hostName"></param>
        public static Host LoadFirstMatchingConnection(string hostName)
        {
            if (_hosts == null)
                _hosts = new Dictionary<long, Host>();

            foreach (Host host in _hosts.Values)
            {
                if (host.GetDisplayName().ToLower().Equals(hostName.ToLower()))
                {
                    return host;
                }
            }

            // If we reach this code part there were no matching nodes in cache...
            LoadAllConnections(); // doin a refresh and retrying it...

            foreach (Host host in _hosts.Values)
            {
                if (host.GetDisplayName().ToLower().Equals(hostName.ToLower()))
                {
                    return host;
                }
            }

            // nothing found

            return null;
        }

        public static List<Host> LoadAllConnections()
        {
            _hosts = new Dictionary<long, Host>();

            foreach (ConnectionHost con in StorageCore.Core.GetConnections())
            {
                LoadConnection(con.ID);
            }

            return _hosts.Values.ToList();
        }

        public static List<Host> LoadAllConnections(long folderID)
        {
            List<Host> returnValue = new List<Host>();
            foreach (Host host in LoadAllConnections())
            {
                if (host.GetFolderID() == folderID)
                    returnValue.Add(host);
            }

            return returnValue;

        }

        /// <summary>
        /// Unloads a connection from cache. Neccessary on editing a connection, so the connection will be reloaded from Database
        /// </summary>
        /// <param name="connectionId"></param>
        public static void UnloadConnection(long connectionId)
        {
            if (_hosts != null && _hosts.ContainsKey(connectionId))
                _hosts.Remove(connectionId);
        }

        ///// <summary>
        ///// Saves a connection to the storage system (VERY BASIC; PRIVATE ONLY!!!)
        ///// </summary>
        ///// <param name="connection"></param>
        ///// <returns>The connection id in the storage</returns>
        //public static int SaveConnection(Host connection)
        //{

        //    IDbPlugin storage = StorageCore.Core;

        //    List<OSVersion> versions = storage.GetVersionlist();
        //    int osID = 11254234;
        //    foreach (OSVersion version in versions)
        //    {
        //        if (version.getFamily().ToLower() == connection._server.GetServerType().ToString().ToLower())
        //        {
        //            osID = version.getId();
        //            break;
        //        }
        //    }



        //    if (connection._inDB && connection._dbID != null)
        //    {
        //        // modify it
        //    }
        //    else
        //    {
        //        // add it
        //        connection._dbID = (int)storage.AddConnection(connection.GetServer().GetRemoteIP().ToString(), connection.GetServer().GetServerName(), "", osID, connection.GetFolderID());
                
        //        foreach (KeyValuePair<String, Protocol> kvp in connection.GetProtocols())
        //        {
        //            int _dbConnSettingsID = (int)storage.AddConnectionSetting(connection._dbID, kvp.Value.GetProtocolIdentifer(), kvp.Value.GetDefaultProtocolPort(), false);

        //            foreach (Session session in connection.GetSessions())
        //            {
        //                if (session.GetSessionProtocol().GetProtocolIdentifer().ToLower() == kvp.Key.ToLower())
        //                {
        //                    SortedList<String, String> settings = session.GetSessionSettings();
        //                    foreach (KeyValuePair<String, String> kvpSesSet in settings)
        //                    {
        //                        storage.ModifyConnectionOption(kvpSesSet.Value, kvpSesSet.Key, _dbConnSettingsID, false);
        //                    }
        //                }
        //            }

        //        }
                
        //    }


        //    //if (connection._alreadyInStorage && connection._connectionID != null)
        //    //{
        //    //    //storage.ModifyConnection(connection._connectionID, server.GetRemoteIP().ToString(), server.GetServerName(), "", osID);
        //    //    //storage.ModifyConnectionSetting(connection._connectionID, connection.GetSession().GetProtocolPort(), storage.GetUserId(), false);

        //    //    //foreach (KeyValuePair<String, String> kvp in connection.GetSession().GetSessionSettings())
        //    //    //{
        //    //    //    List<ConnectionSetting> db_settings = storage.GetConnectionSettings(connection._connectionID);
        //    //    //}
        //    //}
        //    //else
        //    //{
        //    //    connection._connectionID = (int)storage.AddConnection(server.GetRemoteIP().ToString(), server.GetServerName(), "", osID);
        //    //    int connectionSettingsID = (int)storage.AddConnectionSetting(connection._connectionID, connection.GetProtocol().GetProtocolIdentifer(), connection.GetSession().GetProtocolPort(), false);

        //    //    foreach (KeyValuePair<String, String> kvp in connection.GetSession().GetSessionSettings())
        //    //    {
        //    //        storage.ModifyConnectionOption(kvp.Value, kvp.Key, connectionSettingsID, false);
        //    //    }

        //    //}
        //    return connection._dbID;
        //    //return connection._connectionID;
        //}


        private static IServer LoadServer(long id)
        {
            IDbPlugin storage = StorageCore.Core;
            IServer server = Server.NewServer(id);
            OSVersion db_connection_os = storage.GetVersion(server.GetServerTypeID());

            List<ConnectionProtocol> connectionSettings = storage.GetConnectionSettings(id);

            ServerType serverType = server.GetServerType();

            foreach (ConnectionProtocol conSet in connectionSettings)
            {
                if (Kernel.GetAvailableProtocols().Keys.Contains(conSet.Protocol))
                {
                    server.AddProtocol(Kernel.GetAvailableProtocols()[conSet.Protocol]);   
                    // session settings zuweisen!
                    
                }
            }


            return server;
        }

        private static SortedList<String, Protocol> LoadProtocols(int connectionid)
        {
            IDbPlugin storage = StorageCore.Core;
            ConnectionHost connection = storage.GetConnection(connectionid);
            SortedList<String, Protocol> retList = new SortedList<string, Protocol>();

            if (connection != null)
            {
                List<ConnectionProtocol> db_protocols = storage.GetConnectionSettings(connectionid);
                foreach (ConnectionProtocol db_prot in db_protocols)
                {
                    String prot_identifier = db_prot.Protocol;
                    if (Kernel.GetAvailableProtocols().ContainsKey(prot_identifier))
                    {
                        retList.Add(prot_identifier, Kernel.GetAvailableProtocols()[prot_identifier]);
                    }
                }
            }
            else
            {
                throw new ProtocolConfigurationException(beRemoteExInfoPackage.SignificantInformationPackage, String.Format("Server not found in connectionsdatabase (id: {0})", connectionid));

            }

            return retList;
        }
        
        #endregion

        public String GetDisplayName()
        {
            return _server.GetServerName();
        }

        /// <summary>
        /// Modifies a Protocolsetting
        /// </summary>
        /// <param name="ConnectionSettingId">ID of the Protocol to Edit</param>
        /// <param name="Port">New Port of the Protocol</param>
        public void ModifyProtocol(long ConnectionSettingId, int Port)
        {
            ModifyProtocol(ConnectionSettingId, Port, -1);
        }

        /// <summary>
        /// Modifies a Protocolsetting
        /// </summary>
        /// <param name="ConnectionSettingId">ID of the Protocol to Edit</param>
        /// <param name="Port">New Port of the Protocol</param>
        /// <param name="CredentialId">ID of the new CredentialId</param>
        public void ModifyProtocol(long ConnectionSettingId, int Port, int CredentialId)
        {
            if (CredentialId != 0)
                StorageCore.Core.ModifyConnectionSetting(ConnectionSettingId, Port, CredentialId);
            else
            {
                StorageCore.Core.ModifyConnectionSetting(ConnectionSettingId, Port);
                StorageCore.Core.DeleteConnectionSettingCredential(ConnectionSettingId);
            }
        }

        /// <summary>
        /// Modifies Options of a Protocol
        /// </summary>
        /// <param name="ConnectionSettingId">ID of the ConnectionSetting</param>
        /// <param name="ProtocolOptions">ProtocolOptions to edit</param>
        public void ModifyProtocolOptions(long ConnectionSettingId, Dictionary<string, object> ProtocolOptions)
        {
            foreach (KeyValuePair<string, object> kvp in ProtocolOptions)
            {
                StorageCore.Core.ModifyConnectionOption(kvp.Value, kvp.Key, ConnectionSettingId);
            }
        }

    }
}
