using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using beRemote.Core.Common.PluginBase;
using beRemote.Core.Common.Vpn;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.Core.StorageSystem.StorageBase;

using beRemote.Core.Common.LogSystem;
using System.Reflection;
using beRemote.Core.Common.Helper;
using System.Windows;
using beRemote.Core.Common.EventSystem.Events;
using System.Security;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.Exceptions;
using beRemote.GUI.Notification;

namespace beRemote.Core.ProtocolSystem.ProtocolBase
{
    public abstract class Session
    {
        /// <summary>
        /// The ID of this session in beRemote
        /// </summary>
        private Guid _sessionID;
        /// <summary>
        /// The ID of this connection in the storage layer
        /// </summary>
        private long _connectionSettingsID;

        /// <summary>
        /// The underlying base protocol with all needed base settings
        /// </summary>
        private Protocol _protocol;
        /// <summary>
        /// The remote server to wich we will connect
        /// </summary>
        private IServer _server;

        private int _protocolPort;

        /// <summary>
        /// Contains all protocol settings (in the begining basic, after the session is initiated they will be overwirtten by the settings)
        /// </summary>
        private SortedList<String, ProtocolSetting> _sessionSettings;

        protected Control _sessionWindow;

        protected Boolean _debugMode = false;

        private Size _minimalViewSize;

        private String loggerContext = "ProtocolSystem";

        protected int _credID;
        protected String _domain;
        protected String _user;
        private SecureString _pass;

        public ISmartStorage SmartStorage { get; private set; }

        public Session(IServer server, Protocol protocol, long connSettingsID)
        {
            _sessionID = Guid.NewGuid();
            _protocol = protocol;
            _server = server;
            _connectionSettingsID = connSettingsID;

            GetSessionSettings();
            GetSessionCredentials();

            Logger.Log(LogEntryType.Debug, String.Format("Instantiated new session object (GUID: {0}; Protocol: {1}; Server: {2};)", _sessionID, _protocol.GetProtocolIdentifer(), _server), loggerContext);

            if (_protocol.GetSetting(Declaration.IniSection.SETTINGS_BASE, Declaration.IniKey.PROTOCOL_DEBUG).ToLower() == "true")
            {
                Logger.Log(LogEntryType.Warning, String.Format("{0}: This protocols session is running in debug mode. This could lead in strange behaviour! (see plugin.ini -> settings.base -> protocol.debug.mode)", _protocol.GetProtocolIdentifer()), loggerContext);
                SetDebugMode(true);
            }

            // adding this to the event system
            Logger.Log(LogEntryType.Verbose, "Adding session to ConnectionStateChanged event handler");
            Common.EventSystem.EventManager.Instance.AddHandler(Common.EventSystem.EventHandlerType.ConnectionStateChanged,
                new Events.ConnectionEventHandler());

            SmartStorage = new DefaultSmartStorage(protocol);
        }

     

        private void GetSessionCredentials()
        {
            _credID = StorageCore.Core.GetConnectionSetting(_connectionSettingsID).UserCredentialId;
            if (_credID > 0)
            {
                var _cred = StorageCore.Core.GetUserCredentials(_credID);

                if (_cred != null) //if current user is not the owner of the credential, null will be returned
                {
                    _domain = _cred.Domain;
                    _user = _cred.Username;
                    _pass = Helper.DecryptStringFromBytes(_cred.Password, Helper.GetHash1(StorageCore.Core.GetUserSalt1()), Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid()), StorageCore.Core.GetUserSalt3());
                }
            }
        }

        public void CleanSessionCredentials()
        {
            _credID = 0;
            _domain = null;
            _user = null;
            _pass = null;
        }

        public String GetCredentialPassword()
        {
            return GetCredentialPassword(_pass);
        }

        public String GetCredentialPassword(SecureString pass)
        {
            return Helper.ConvertToUnsecureString(pass);
        }
        public abstract Control GetSessionWindow();

        /// <summary>
        /// OPens conection with specified credentials.
        /// <b>Note: </b>Overwrites db credentials!
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void OpenConnection(String username, SecureString password)
        {
            Logger.Verbose("[Session: " + GetSessionID() + "] Trying to connect to remote server (" + GetSessionServer().GetServerHostName() + ")", 0);
            if (username == "")
            {
                if (false == ((ProtocolMetadataAttribute)_protocol.MetaData).ProtocolAuthIsHandled)
                {
                    var wnd = new GUI.UserCredentialsWindow();
                    wnd.ShowDialog();

                    username = wnd.txtUsername.Text;
                    password = Helper.ConvertToSecureString(wnd.txtPassword.Password);
                }
            }
            OnOpenConnectionPreProcessor();
            OnOpenConnection(username, password);
            OnOpenConnectionPostProcessor();
        }

        /// <summary>
        /// Opens connection with credentials from db (if specified)
        /// </summary>
        public void OpenConnection()
        {
            if (!string.IsNullOrEmpty(_user))
            {
                if(_domain.Equals(""))
                    OpenConnection(_user, _pass);
                else
                    OpenConnection(_domain + "\\" + _user, _pass);
            }
            else
            {
                OpenConnection("", null);
            }
        }

       
        public abstract void OnOpenConnection(String username, SecureString password);

        public abstract void CloseConnection();
        
        /// <summary>
        /// Processed before OnOpenConnection is called
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnOpenConnectionPreProcessor(params Object[] args)
        {

        }

        /// <summary>
        /// Processed after OnOpenConnection is called
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnOpenConnectionPostProcessor(params Object[] args)
        {

        }

        /// <summary>
        /// Triggers the protocol system to fire an close event. This should be called from inside the protocol implementation
        /// </summary>
        public void TriggerCloseConnectionEvent()
        {
            Common.EventSystem.EventManager.Instance.OnEvent(Common.EventSystem.EventHandlerType.ConnectionStateChanged,
                new ConnectionStateChangedEventArgs(
                    ConnectionState.DISCONNECTED, 
                    _server.GetServerDBID().ToString(), 
                    _server.GetServerName(), 
                    _protocol.GetProtocolIdentifer(), 
                    _sessionID.ToString())
                );
        }

        public Guid GetSessionID()
        {
            return _sessionID;
        }

        public Size GetMinimalViewSize()
        {
            if (_minimalViewSize == null)
                throw new ProtocolException(beRemoteExInfoPackage.MinorInformationPackage, "The current minimal view size is invalid. Change it with SetMinimalViewSize(Size size)");

            return _minimalViewSize;
        }

        public void SetMinimalViewSize(Size size)
        {
            if (size == null)
                throw new ProtocolException(beRemoteExInfoPackage.MinorInformationPackage, "Cannot change the view size to a null value");

            _minimalViewSize = size;
        }


        /// <summary>
        /// Gets the protocol object of the current session.
        /// <b>NOTE:</b> This object will be delivered as default instance. If you are looking for user specific settings user session object
        /// </summary>
        /// <returns></returns>
        public Protocol GetSessionProtocol()
        {
            return _protocol;
        }

        public IServer GetSessionServer()
        {
            return _server;
        }

        /// <summary>
        /// This will return the used protocol port
        /// <b>Note:</b> This value overwrites the default protocol port!
        /// </summary>
        public int GetProtocolPort()
        {
            if (_protocolPort > 0)
            {
                return _protocolPort;
            }
            
            var settingPort = Convert.ToInt32(StorageCore.Core.GetConnectionSetting(_connectionSettingsID).getPort());

            _protocolPort = settingPort;

            return _protocolPort;
        }

        /// <summary>
        /// Sets and overwrites the default protocol port for this session
        /// </summary>
        /// <param name="port">The port that we have to use</param>
        public void SetProtocolPort(int port)
        {
            if (port < 1)
                throw new ProtocolException(beRemoteExInfoPackage.SignificantInformationPackage, "Could not change the protocols port in the session. \r\nThe Portnumber seems to be invalid!");

            _protocolPort = port;
        }

        public SortedList<String, ProtocolSetting> GetSessionSettings()
        {
            if (_sessionSettings == null)
            {
                _sessionSettings = new SortedList<string, ProtocolSetting>();

                _sessionSettings = _protocol.GetProtocolSettings();

                LoadConnectionOptions();
            }

            return _sessionSettings;
        }

        private void LoadConnectionOptions()
        {
            Dictionary<String, ConnectionProtocolOption> options = GetConnectionOptionsDictionary(_connectionSettingsID);

            var defSett = _protocol.GetProtocolSettings();

            foreach (KeyValuePair<String, ConnectionProtocolOption> kvp in options)
            {
                if (_sessionSettings.ContainsKey(kvp.Key))
                {
                    var sett = defSett[kvp.Key];
                    var a = new ProtocolSetting(kvp.Key, sett.GetTitle(), sett.GetDescription(), sett.GetDataType());
                    if (sett.GetDataType() == typeof (UserCredential))
                    {
                        var _cred = StorageCore.Core.GetUserCredentials((int)kvp.Value.getSettingvalue());
                        if (_cred != null)
                            a.Value = new DefinedProtocolSettingValue(sett.GetTitle(), new UserCredentials(_cred.Domain, _cred.Username, Helper.ConvertToUnsecureString(Helper.DecryptStringFromBytes(_cred.Password, Helper.GetHash1(StorageCore.Core.GetUserSalt1()), Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid()), StorageCore.Core.GetUserSalt3()))));
                    }
                    else
                    {
                        
                        a.Value = new DefinedProtocolSettingValue(sett.GetTitle(), kvp.Value.getSettingvalue());
                        
                    }
                    _sessionSettings[kvp.Key] = a;
                }
            }

        }

        private Dictionary<string, ConnectionProtocolOption> GetConnectionOptionsDictionary(long _connectionOptionsID)
        {
            List<ConnectionProtocolOption> options = StorageCore.Core.GetConnectionOptions(_connectionSettingsID);
            var returnValue = new Dictionary<string, ConnectionProtocolOption>();

            foreach (var option in options)
            {
                returnValue.Add(option.getSettingname(), option);
            }

            return returnValue;
        }

        public override string ToString()
        {
            String retVal = base.ToString() + "\r\n";

            retVal += String.Format("Session GUID: {0}\r\nDebug enabled: {1}", _sessionID, _debugMode);

            return retVal;
        }


        internal void SetDebugMode(bool state)
        {
            _debugMode = state;


        }


        public bool IsInDebugMode()
        {
            return _protocol.IsInDebugMode();
        }



        public void OnConnectionOpening(object sender, RoutedEventArgs routedEventArgs)
        {

            // starting VPN connection
            if (_server.UsesVpn)
            {
                TrayIcon.TrayIconInstance.ShowNotification("Trying to establish configured VPN connection... please be patient.", 3000);

                VpnManager.Instance.ConnectVpn(_server.VpnId);
            }
        }
        public void OnConnectionClosing(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_server.UsesVpn)
            {
                TrayIcon.TrayIconInstance.ShowNotification("Trying to disconnect configured VPN connection... please be patient.", 3000);

                VpnManager.Instance.DisconnectVpn(_server.VpnId);
            }
        }
    }
}
