using System;
using System.Security;
using System.Windows;
using System.Windows.Forms;
using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using MSTSCLib;
using System.Windows.Threading;
using beRemote.Core.Common.LogSystem;
using IniKey = beRemote.VendorProtocols.RDP.Declaration.IniKey;


namespace beRemote.VendorProtocols.RDP.GUI
{
    /// <summary>
    /// Interaktionslogik für SessionWindow.xaml
    /// </summary>
    public partial class SessionWindow
    {
        private readonly RdpSession _Session;
        public WinFormsWrapper.AXRDPUC Ax;
        private string _Username = "";
        private SecureString _Password;
        private bool _ShowResizeMessage = true;
        
        public SessionWindow(Session session)
        {
            InitializeComponent();

            Logger.Verbose("[Session: " + session.GetSessionID() + "] Creating new session window", EventId.NewSessionWindow);

            _Session = (RdpSession)session;
            
            Header = _Session.GetSessionServer().GetServerName();
            IconSource = _Session.GetSessionProtocol().ProtocolIconSmall;
            TabToolTip = _Session.GetSessionServer().GetServerHostName();       
        }
        
        public WinFormsWrapper.AXRDPUC GetRdpControl()
        {
            if (Ax == null)
            {
                Ax = new WinFormsWrapper.AXRDPUC();
                Ax.Dock = DockStyle.Fill;
            }  

            wfHost.Child = Ax;
            
            return (WinFormsWrapper.AXRDPUC)wfHost.Child;
        }

        private void dockPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            Logger.Verbose("[Session: " + _Session.GetSessionID() + "] Session window resized", EventId.SessionWindowResize);
            
            if (_Session.SessionState == RdpSessionState.StartNew)
            {
                OpenNewConnection(_Username, _Password);               
            }
            else if (_Session.SessionState == RdpSessionState.StartNewWithResize)
            {
                OpenNewConnection(_Username, _Password);               
            }
            else if (_Session.SessionState == RdpSessionState.Opened)
            {
                if (!_ShowResizeMessage) 
                    return;
                
                var dockPanelHeight = Convert.ToInt32(dockPanel.ActualHeight);

                // subtract wfHost Height from dockPanel height and divide it by 2 as it is centered
                var freeBottonHeight = (dockPanelHeight - Convert.ToInt32(Ax.rdpControl.DesktopHeight)) / 2;

                if (freeBottonHeight <= 50) 
                    return;

                var rm = new ResizeMessage();

                messages.Visibility = Visibility.Visible;

                rm.VerticalAlignment = VerticalAlignment.Bottom;
                messages.Children.Add(rm);

                rm.cmdResizeIt.Click += cmdResizeIt_Click;
                rm.cmdNoResize.Click += cmdNoResize_Click;
                rm.cmdIgnore.Click += cmdIgnore_Click;

                _Session.SessionState = RdpSessionState.OpenedResizeMessage;
            }
            else if (_Session.SessionState == RdpSessionState.OpenedResizeConnection)
            {
                ResizeConnection();
            }
        }

        public void OpenNewConnection(String username, SecureString password)
        {
            OpenNewConnection(dockPanel.ActualHeight, dockPanel.ActualWidth, username, password);
        }

        public void OpenNewConnection(double height, double width, String username, SecureString password)
        {
            _Username = username;
            _Password = password;

            // testing session object functionalities
            _Session.GetProtocolPort();

            if (height.ToString() == "0" || width.ToString() == "0")
            {
                _Session.SessionState = RdpSessionState.StartNewWithResize;
                return;
            }

            GetRdpControl();

            var settings = _Session.GetSessionSettings();
            if (settings[IniKey.FITTOTAB].GetProtocolSettingValue().GetValue().ToString().ToLower() == "true")
            {
                Ax.rdpControl.DesktopHeight = Convert.ToInt32(height);
                Ax.rdpControl.DesktopWidth = Convert.ToInt32(width);
            }
            else if (settings[IniKey.RESOLUTIONX].GetProtocolSettingValue().GetValue().ToString() != "" &&
                settings[IniKey.RESOLUTIONY].GetProtocolSettingValue().GetValue().ToString() != "")
            {
                var sizeX = 1024;
                var sizeY = 768;

                if (Int32.TryParse(settings[IniKey.RESOLUTIONX].GetProtocolSettingValue().GetValue().ToString(), out sizeX) == false)
                    Logger.Log(LogEntryType.Info, String.Format("Failed to convert X-Size of new RDP-Tab. Original value: \"{0}\"", settings[IniKey.RESOLUTIONX].GetProtocolSettingValue().GetValue()));

                if (Int32.TryParse(settings[IniKey.RESOLUTIONY].GetProtocolSettingValue().GetValue().ToString(), out sizeY) == false)
                    Logger.Log(LogEntryType.Info, String.Format("Failed to convert Y-Size of new RDP-Tab. Original value: \"{0}\"", settings[IniKey.RESOLUTIONY].GetProtocolSettingValue().GetValue()));

                Ax.rdpControl.DesktopHeight = sizeY;
                Ax.rdpControl.DesktopWidth = sizeX;
            }
            
            Ax.AdvancedSettings.EnableCredSspSupport = Boolean.Parse(settings[IniKey.SECURITY_AUTH_NLA].GetProtocolSettingValue().GetValue().ToString());
            Ax.AdvancedSettings.ClearTextPassword = _Session.GetCredentialPassword(password);
            Ax.AdvancedSettings.RDPPort = _Session.GetProtocolPort();
            Ax.AdvancedSettings.SmartSizing = Boolean.Parse(settings[IniKey.SMARTSIZE].GetProtocolSettingValue().GetValue().ToString());

            Ax.rdpControl.UserName = username;

            var secured = (IMsTscNonScriptable)Ax.rdpControl.GetOcx();
            secured.ClearTextPassword = _Session.GetCredentialPassword();
            if (settings[IniKey.GATEWAY_USE].GetProtocolSettingValue().GetValue().ToString().ToLower().Contains("true"))
            {
                var cred = (UserCredentials)settings[IniKey.GATEWAY_CREDENTIALS].Value.Value;

                //ax.TransportSettings.GatewayHostname = _settings[IniKey.GATEWAY_SERVER].Value.GetValue().ToString();
                //ax.TransportSettings.GatewayDomain = cred.GetDomain();
                //ax.TransportSettings.GatewayUsername = cred.GetUsername();
                //ax.TransportSettings.GatewayPassword = cred.GetPassword();
                //ax.TransportSettings.GatewayUsageMethod = 0x2;
                //ax.TransportSettings.GatewayCredsSource = 0;
                //ax.TransportSettings.GatewayUserSelectedCredsSource = 0;
                //ax.rdpControl.Server = _session.GetSessionServer().GetServerHostName();
                Ax.TransportSettings.GatewayHostname = settings[IniKey.GATEWAY_SERVER].Value.GetValue().ToString();
                Ax.TransportSettings.GatewayUsageMethod = 1;

                Ax.TransportSettings.GatewayCredsSource = 0;
                Ax.TransportSettings.GatewayProfileUsageMethod = 1;
                Ax.TransportSettings.GatewayDomain = cred.GetDomain();
                Ax.TransportSettings.GatewayPassword = cred.GetPassword();
                Ax.TransportSettings.GatewayUsername = cred.GetUsername();
                Ax.rdpControl.Server = _Session.GetSessionServer().GetServerHostName();
            }
            else
            {
                Ax.rdpControl.Server = _Session.GetSessionServer().GetRemoteIP().ToString();
            }

            Ax.rdpControl.OnDisconnected += rdpControl_OnDisconnected;
            
            Ax.rdpControl.Connect();
            
            _Session.SessionState = RdpSessionState.Opened;
        }
        
        void rdpControl_OnDisconnected(object sender, AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEvent e)
        {
            //http://msdn.microsoft.com/en-us/library/windows/desktop/aa382170(v=vs.85).aspx

            _Session.TriggerCloseConnectionEvent();

            //Show Errormessage, if a not estimated Disconnection occures
            if (DisconnectEvents.EventDescription.ContainsKey(e.discReason) &&
                DisconnectEvents.EventDescription[e.discReason].IsEstimated == false)
            {
                TopText = String.Format("Disconnected from Host. ({0})\n{1}", e.discReason, DisconnectEvents.EventDescription[e.discReason].ErrorMessage);
            }
            else if (!DisconnectEvents.EventDescription.ContainsKey(e.discReason))
            {
                TopText = String.Format("Unknown Error ({0})", e.discReason);
            }

            switch (e.discReason)
            {
                case 516:
                    // ReSharper disable once ObjectCreationAsStatement
                    new ProtocolException(beRemoteExInfoPackage.MajorInformationPackage,
                        String.Format(
                            "The remote server could not be contacted. Maybe the ip address is invalid or the dns name not solveable ('{0}'. [disconnectReasonSocketConnectFailed 516 (0x204)]", _Session.GetSessionServer().GetRemoteIP()));
                    break;

                default:
                    Logger.Warning("[Session: " + _Session.GetSessionID() + "] RDP Ctrl received disconnect event! [mstscax-ctrl-evt-id: " + e.discReason + "]", EventId.CloseConnection);
                    break;
            }

            try
            {
                //If the connection is established: disconnect
                if (Ax.rdpControl.Connected != 0)
                    Ax.rdpControl.Disconnect();

                Ax = null;
            }
            catch (Exception ex)
            {
                Logger.Log(LogEntryType.Verbose, "Exeption on Disconnecting RDP-Connection", ex);
            }


            //Close the Tab
            CloseTab();
        }

        private void ResizeConnection()
        {
            var settings = _Session.GetSessionSettings();

            //If it is not "fit to tab" and Smartsizing is true
            if (settings[IniKey.FITTOTAB].GetProtocolSettingValue().GetValue().ToString() == "false"
                && settings[IniKey.SMARTSIZE].GetProtocolSettingValue().GetValue().ToString() == "true") 
                return;
            
            Ax.rdpControl.DesktopHeight = Convert.ToInt32(dockPanel.ActualHeight);
            Ax.rdpControl.DesktopWidth = Convert.ToInt32(dockPanel.ActualWidth);

            Ax.rdpControl.Disconnect();
            Ax.Dispose();
            Ax = null;

            OpenNewConnection(dockPanel.ActualHeight, dockPanel.ActualWidth, _Username, _Password);
        }

        void cmdIgnore_Click(object sender, RoutedEventArgs e)
        {
            _ShowResizeMessage = false;
            messages.Children.Clear();
            messages.Visibility = Visibility.Collapsed;
        }

        void cmdNoResize_Click(object sender, RoutedEventArgs e)
        {            
            messages.Children.Clear();
            messages.Visibility = Visibility.Collapsed;
        }

        void cmdResizeIt_Click(object sender, RoutedEventArgs e)
        {
            messages.Children.Clear();
            messages.Visibility = Visibility.Collapsed;

            _Session.SessionState = RdpSessionState.OpenedResizeConnection;
            ResizeConnection();
            //dockPanel.RaiseEvent(new RoutedEventArgs(DockPanel.SizeChangedEvent, dockPanel));
            
        }

        //private void cmdConnect_Click(object sender, RoutedEventArgs e)
        //{
        //    ax.rdpControl.Server = _session.GetSessionServer().GetRemoteIP().ToString();
        //    ax.rdpControl.UserName = _session.GetUserCredentials().GetUsername();
        //    IMsTscNonScriptable secured = (IMsTscNonScriptable)ax.rdpControl.GetOcx();
        //    secured.ClearTextPassword = _session.GetUserCredentials().GetPassword();

        //    ax.rdpControl.Connect();
        //}

        public override void Dispose()
        {
            base.Dispose();
            
            if (Ax != null)
            {
                if (Ax.rdpControl != null)
                {
                    Ax.rdpControl.Disconnect();
                }
                Ax.Dispose();
                Ax = null;
            }
        }

    }
}
