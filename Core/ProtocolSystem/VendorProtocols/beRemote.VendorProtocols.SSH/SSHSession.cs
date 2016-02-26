using System;
using System.IO;
using System.Security;
using System.Windows.Controls;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using FSM.DotNetSSH;
using Session = beRemote.Core.ProtocolSystem.ProtocolBase.Session;

namespace beRemote.VendorProtocols.SSH
{
    public class SshSession : Session
    {
        private int _sshTimeout = 5000;
        private SshShell _sshConnection;
        private Stream _readerStream;
        public SshSession(IServer server, Protocol protocol, long dbConfigId) : base(server, protocol, dbConfigId) { }
        
        public override Control GetSessionWindow()
        {
            if (_sessionWindow == null)
            {
                _sessionWindow = new ViewModel.SSHUi(
                        GetSessionServer().GetServerName(),
                        GetSessionProtocol().ProtocolIconSmall,
                        GetSessionServer().GetServerHostName() + " - " + GetSessionServer().GetRemoteIP(),
                        Convert.ToBoolean(GetSessionSettings()["ssh.outputwrap"].GetProtocolSettingValue().GetValue())
                    );
            }

            return _sessionWindow;
        }

        private ViewModel.SSHUi SessionWindow
        {
            get {return ((ViewModel.SSHUi)_sessionWindow);}
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
                    throw new ProtocolException(beRemoteExInfoPackage.MajorInformationPackage, "SSH not initialized!");

                if (SessionWindow.IsSendInputRegistered == false)
                    SessionWindow.SendInput += SessionWindowSendInput;

                //Load the configuration
                _sshTimeout = Convert.ToInt32(GetSessionSettings()["ssh.timeout"].GetProtocolSettingValue().GetValue());

                try
                {
                    //Create SSH Connection
                    SshConnectionInfo input;
                    input.Host = GetSessionServer().GetRemoteIP().ToString();
                    input.User = username;
                    input.Pass = password == null ? null : Helper.ConvertToUnsecureString(password);
                    //if (input.IdentityFile != null) shell.AddIdentityFile(input.IdentityFile);

                    _sshConnection = new SshShell(input.Host, input.User);
                    _readerStream = new MemoryStream();
                    if (input.Pass != null) _sshConnection.Password = input.Pass;
                    //if (input.IdentityFile != null) shell.AddIdentityFile(input.IdentityFile);

                    //This statement must be prior to connecting
                    //_SshConnection.RedirectToConsole();

                    WriteDisplayText("Connecting...");
                    _sshConnection.Connect();
                    WriteDisplayText(_sshConnection.Expect());
                }
                catch (Exception ea)
                {
                    Logger.Log(LogEntryType.Warning, "Cannot connect to SSH-Server.", ea);
                    WriteDisplayText(Environment.NewLine + "***Cannot connect to SSH-Server.");
                    WriteDisplayText(Environment.NewLine + ea.Message);
                    return;
                }

                WaitForSessionClose();

                CloseConnection();
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Warning, "An unknown error occures on starting SSH-session", ea);
            }
        }

        public void WaitForSessionClose()
        {
            while (_sshConnection.ShellOpened)
                System.Threading.Thread.Sleep(500);

            WriteDisplayText(Environment.NewLine + "***DISCONNECTED");
            CloseConnection();
        }

        /// <summary>
        /// Receive a Command from the Viewmodel and forward it to the SSHConnection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SessionWindowSendInput(object sender, EventArgs.SendInputEventArgs e)
        {
            // send client input to server
            var inputtext = e.SendCommand;
            try
            {
                //_SshConnection.
                _sshConnection.WriteLine(inputtext);
               var outputtext = _sshConnection.Expect();
               WriteDisplayText(outputtext);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString(), 0);
                
                WriteDisplayText("Problem while executing command: " + ex.Message);
                
            }


            
        }
    
        /// <summary>
        /// Close the current Connection
        /// </summary>
        public override void CloseConnection()
        {
            WriteDisplayText("Disconnecting...");
            _sshConnection.Close();
            WriteDisplayText("OK");

            _sshConnection = null;

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
                                                   //SessionWindow.DisplayText += text;
                                                   SessionWindow.WriteLine(text);
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
