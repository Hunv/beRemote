namespace beRemote.Core.Definitions.Classes
{
    public class ConnectionProtocol
    {
        private long _Id = 0;
        private long _ConnectionId = 0;
        private string _Protocol = "";
        private int _Port = 0;
        private int _UserCredentialId = 0;

        public ConnectionProtocol(long id, long connectionId, string protocol, int port)
        {
            _Id = id;
            _ConnectionId = connectionId;
            _Protocol = protocol;
            _Port = port;
        }
        public ConnectionProtocol(long id, long connectionId, string protocol, int port, int usercredentialId)
            :this(id,connectionId,protocol,port)
        {
            _UserCredentialId = usercredentialId;
        }

        [System.Obsolete("Use property instead!")]
        public long getId() { return (_Id); }
        [System.Obsolete("Use property instead!")]
        public long getConnectionId() { return (_ConnectionId); }
        [System.Obsolete("Use property instead!")]
        public string getProtocol() { return (_Protocol); }
        [System.Obsolete("Use property instead!")]
        public int getPort() { return (_Port); }
        [System.Obsolete("Use property instead!")]
        public int getUserCredentialId() { return (_UserCredentialId); }

        [System.Obsolete("Use property instead!")]
        public void setUserCredentialId(int id) {_UserCredentialId = id;}
        [System.Obsolete("Use property instead!")]
        public void setPort(int port) { _Port = port; }

        /// <summary>
        /// Reset the Usercredentials to "no credentials"
        /// </summary>
        public void cleanUserCredentials() { _UserCredentialId = 0; }

        /// <summary>
        /// Get or Set the Id of the ConnectionProtocol-Information
        /// </summary>
        public long Id { get { return _Id; } set { _Id = value; } }

        /// <summary>
        /// Get or Set the ConnectionId of the ConnectionProtocol-Information
        /// </summary>
        public long ConnectionId { get { return _ConnectionId; } set { _ConnectionId = value; } }

        /// <summary>
        /// Get or Set the Protocol of the ConnectionProtocol-Information
        /// </summary>
        public string Protocol { get { return _Protocol; } set { _Protocol = value; } }

        /// <summary>
        /// Get or Set the Port of the ConnectionProtocol-Information
        /// </summary>
        public int Port { get { return _Port; } set { _Port = value; } }

        /// <summary>
        /// Get or Set the UsercredentialId of the ConnectionProtocol-Information
        /// </summary>
        public int UserCredentialId { get { return _UserCredentialId; } set { _UserCredentialId = value; } }
    }
}
