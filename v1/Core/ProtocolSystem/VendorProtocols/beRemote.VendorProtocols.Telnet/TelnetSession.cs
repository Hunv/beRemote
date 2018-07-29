using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Controls;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;

namespace beRemote.VendorProtocols.Telnet
{
    public class TelnetSession : Session
    {
        private int _TelnetTimeout = 5000;
        private TelnetInterface _TelConnection;
        private bool _Exit;


        public TelnetSession(IServer server, Protocol protocol, long dbConfigId) : base(server, protocol, dbConfigId) { }
        
        public override Control GetSessionWindow()
        {
            if (_sessionWindow == null)
            {
                _sessionWindow = new ViewModel.TelnetUi(
                        GetSessionServer().GetServerName(),
                        GetSessionProtocol().ProtocolIconSmall,
                        GetSessionServer().GetServerHostName() + " - " + GetSessionServer().GetRemoteIP(),
                        Convert.ToBoolean(GetSessionSettings()["telnet.outputwrap"].GetProtocolSettingValue().GetValue())
                    );
            }

            return _sessionWindow;
        }

        private ViewModel.TelnetUi SessionWindow
        {
            get {return ((ViewModel.TelnetUi)_sessionWindow);}
            set { _sessionWindow = value; }
        }

        /// <summary>
        /// Open a Connection
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public override void OnOpenConnection(string username, SecureString password)
        {
            try
            {
                if (_sessionWindow == null)
                    throw new ProtocolException(beRemoteExInfoPackage.MajorInformationPackage, "Telnet not initialized!");

                if (SessionWindow.IsSendInputRegistered == false)
                    SessionWindow.SendInput += SessionWindowSendInput;

                //Load the configuration
                _TelnetTimeout = Convert.ToInt32(GetSessionSettings()["telnet.timeout"].GetProtocolSettingValue().GetValue());

                try
                {
                    //Create Telnet Connection
                    _TelConnection = new TelnetInterface(GetSessionServer().GetRemoteIP().ToString(), GetProtocolPort());
                }
                catch (Exception ea)
                {
                    Logger.Log(LogEntryType.Warning, "Cannot connect to Telnet-Server.", ea);
                    WriteDisplayText(Environment.NewLine + "***Cannot connect to Telnet-Server.");
                    WriteDisplayText(Environment.NewLine + ea.Message);
                    return;
                }

                
                var telAnswer = "";

                //Login, if there is a password given (try autologin)
                try
                {
                    if (password != null && password.Length > 0)
                        telAnswer = _TelConnection.Login(username, Helper.ConvertToUnsecureString(password), _TelnetTimeout);
                }
                catch (Exception ea)
                {
                    Logger.Log(LogEntryType.Info, "Cannot Login to Telnet-Server", ea);

                    WriteDisplayText(Environment.NewLine + "***Cannot connect to Telnet-Server. Try it without predefined credentials.");
                    WriteDisplayText(Environment.NewLine + ea.Message);
                }

                //Show output
                WriteDisplayText(telAnswer);

                //Start the ReadInput-Loop
                ReadSessionInput();
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Warning, "An unknown error occures on starting telnet-session", ea);
            }
        }

        public void ReadSessionInput()
        {
            // while connected
            while (_TelConnection.IsConnected && _Exit == false)
            {
                // display server output
                WriteDisplayText(_TelConnection.Read());
            }

            WriteDisplayText(Environment.NewLine + "***DISCONNECTED");
            CloseConnection();
        }

        /// <summary>
        /// Receive a Command from the Viewmodel and forward it to the TelnetConnection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SessionWindowSendInput(object sender, EventArgs.SendInputEventArgs e)
        {
            // send client input to server
            var inputtext = e.SendCommand;

            if (inputtext.ToLower() == "exit")
                _Exit = true;

            _TelConnection.WriteLine(inputtext);
        }
    
        /// <summary>
        /// Close the current Connection
        /// </summary>
        public override void CloseConnection()
        {
            if (_TelConnection.IsConnected)
                _TelConnection.WriteLine("exit");

            _TelConnection = null;

            // Triggering upper close connection event!
            TriggerCloseConnectionEvent();

            CloseTab();
        }

        /// <summary>
        /// Write a Text to the Console-Output
        /// </summary>
        /// <param name="text"></param>
        private void WriteDisplayText(string text)
        {
            SessionWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                (System.Threading.ThreadStart) delegate()
                                               {
                                                   SessionWindow.DisplayText += text;
                                               }
                );
        }

        /// <summary>
        /// Close the Tab
        /// </summary>
        private void CloseTab()
        {
            SessionWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                (System.Threading.ThreadStart) delegate()
                                               {
                                                   SessionWindow.CloseTab();
                                               }
                );
        }
    }
}
