using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.Core.Common.Vpn.OVpn;
using Microsoft.Win32;

namespace beRemote.Core.Common.Vpn
{
    // For Management-Telnet-Commands see:
    // http://openvpn.net/index.php/open-source/documentation/miscellaneous/79-management-interface.html

    public class OpenVpn : VpnBase
    {
        private readonly ResourceDictionary _LangDictionary = new ResourceDictionary(); //Contains the Language-Variables

        #region Constructor
        public OpenVpn()
        {
            LoadLanguage();
        }
    
        private void LoadLanguage()
        {         
            #region Load Language Dictionary
            var dictionaryFiles = new List<string>
                                           {
                                               "Language/language.xaml",
                                               "Language/language.de-DE.xaml",
                                               "Language/language.es-ES.xaml",
                                               "Language/language.fr-FR.xaml",
                                               "Language/language.it-IT.xaml",
                                               "Language/language.nl-NL.xaml",
                                               "Language/language.pl-PL.xaml",
                                               "Language/language.ru-RU.xaml",
                                               "Language/language.zh-CN.xaml",
                                               "Language/language.cs-CZ.xaml",
                                               "Language/language.ar-SA.xaml",
                                               "Language/language.bg-BG.xaml",
                                               "Language/language.dk-DK.xaml",
                                               "Language/language.el-GR.xaml",
                                               "Language/language.fa-IR.xaml",
                                               "Language/language.fi-FI.xaml",
                                               "Language/language.he-IL.xaml",
                                               "Language/language.hi-IN.xaml",
                                               "Language/language.hr-HR.xaml",
                                               "Language/language.hu-HU.xaml",
                                               "Language/language.ko-KR.xaml",
                                               "Language/language.nn-NO.xaml",
                                               "Language/language.se-SE.xaml",
                                               "Language/language.tr-TR.xaml",
                                               "Language/language.zh-CN.xaml"
                                           };

            foreach (var aLangfile in dictionaryFiles)
                _LangDictionary.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(aLangfile, UriKind.Relative) });

            #endregion
        }
        #endregion

        #region Properties

        /// <summary>
        /// The path to the .exe-file of openvpn.exe
        /// </summary>
        public string ConfigPath
        {
            get { return Parameter1; }
            set { Parameter1 = value; }
        }

        /// <summary>
        /// The name of the config-file
        /// </summary>
        public string ConfigName
        {
            get { return Parameter2; }
            set { Parameter2 = value; }
        }
        
        public string VpnIp { get; private set; }
        public string RemoteIp { get; private set; }
        public string ConnectionState { get; private set; }
        public string ConnectionDetails { get; private set; }
        
        public bool IsValid
        {
            get { return Validate(); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Connect to a VPN Gateway. Set ConfigName and ClientPath first
        /// </summary>
        /// <returns>Connection successful or not</returns>
        public override bool Connect()
        {
            return (Connect(false));
        }

        /// <summary>
        /// Connect to a VPN Gateway. Set ConfigName and ClientPath first
        /// </summary>
        /// <param name="disconnectExistingConnections">Disconnect an existing other vpn-tunnel?</param>
        /// <returns>Connection successful or not</returns>
        public bool Connect(bool disconnectExistingConnections)
        {
            if (Validate(true) == false)            
                return (false);


            if (IsClientPathValid(ConfigPath) == false)
            {
                MessageBox.Show(_LangDictionary["VpnOpenSetPath"].ToString(), _LangDictionary["VpnOpenSetNameTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }  

            if (disconnectExistingConnections == false && IsConnectionEnabled(10))
            {
                MessageBox.Show(_LangDictionary["VpnOpenAlreadyOpen"].ToString(), _LangDictionary["VpnOpenAlreadyOpenTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }

            if (disconnectExistingConnections && IsConnectionEnabled(10))
            {
                VpnDisconnect();
            }

            return (VpnConnect(
                ConfigPath,
                ConfigName
                ));
        }

        /// <summary>
        /// Disconnect the current connection
        /// </summary>
        /// <returns>Disconnect successful or not</returns>
        public override bool Disconnect()
        {
            if (IsValid == false)
                return false;

            return (VpnDisconnect());
        }

        /// <summary>
        /// Is OpenVPN installed?
        /// </summary>
        /// <returns></returns>
        public override bool IsInstalled()
        {
            if (Registry.LocalMachine.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\OpenVPN\\exe_path") == null)
            {
                return (false);
            }
            
            return (true);
        }

        /// <summary>
        /// Checks if all information are valid. Executed before saving
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return (Validate(false));
        }

        /// <summary>
        /// Checks if all information are valid. Executed before saving
        /// </summary>
        /// <returns></returns>
        public override bool Validate(bool showError)
        {
            if (string.IsNullOrEmpty(ConfigName) || ConfigName.Length < 8)
            {
                if (showError) MessageBox.Show(_LangDictionary["VpnOpenSetPath"].ToString(), _LangDictionary["VpnOpenSetPathTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }

            if (string.IsNullOrEmpty(ConfigPath))
            {
                if (showError) MessageBox.Show(_LangDictionary["VpnOpenSetPath"].ToString(), _LangDictionary["VpnOpenSetNameTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }  

            return true;
        }
        #endregion

        #region private Methods
        /// <summary>
        /// Connect to VPN-Gateway
        /// </summary>
        /// <param name="clientPath"></param>
        /// <param name="configPath"></param>
        /// <returns></returns>
        private bool VpnConnect(string clientPath, string configPath)
        {
            if (String.IsNullOrEmpty(clientPath) || string.IsNullOrEmpty(configPath))
                return (false);

            // Start the child process.
            var p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = clientPath;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(configPath);
            p.StartInfo.UseShellExecute = false;

            p.StartInfo.Arguments = String.Format("--config \"{0}\" --management 127.0.0.1 23321 --script-security 3", configPath);
            
            p.Start();

            var tC = ConnectTelnetSession("127.0.0.1", 23321, 250, 20);

            //Connection established successfully
            if (tC != null)
            {
                tC.Read();

                int stateCounter = 5;

                while (stateCounter > 0)
                {
                    if (IsConnectionEnabled(5))
                        return (true);
                    
                    System.Threading.Thread.Sleep(1000);

                    stateCounter--;
                }
            }
            

            //If this part of code is reached, no return(true) was performed anywhere, so this is false
            if (tC != null && tC.IsConnected)
                tC.Logout();

            return (false);
        }

        /// <summary>
        /// Disconnects the established connection
        /// </summary>
        /// <returns></returns>
        private bool VpnDisconnect()
        {
            TelnetConnection tC = ConnectTelnetSession("127.0.0.1", 23321, 1000, 10);

            //Connection established successfully
            if (tC != null)
            {
                tC.WriteLine("signal SIGTERM");
                    
                if (tC.IsConnected)
                    tC.Logout();

                IsConnected = false;
                return (true);
            }

            return (false);
        }

        /// <summary>
        /// Checks if there is currently an active connection
        /// </summary>
        /// <returns></returns>
        private bool IsConnectionEnabled(int maxWaitTime)
        {
            TelnetConnection tC = ConnectTelnetSession("127.0.0.1", 23321, 250, maxWaitTime);

            //Connection established successfully
            if (tC != null)
            {
                tC.Read();

                tC.WriteLine("state");
                string[] statusString = tC.Read().Split(',');
                
                if (statusString.Length < 5)
                    return (false);

                if (statusString[1] == "CONNECTED")
                {
                    ConnectionState = statusString[1];
                    ConnectionDetails = statusString[2];
                    VpnIp = statusString[3];
                    RemoteIp = statusString[4];
                    IsConnected = true;

                    tC.Logout();
                    return (true);
                }
            }


            ConnectionState = "DISCONNECTED";
            ConnectionDetails = "";
            VpnIp = "";
            RemoteIp = "";
            IsConnected = false;
            return (false);
        }

        /// <summary>
        /// Checks if the vpnclient-exe-file exists
        /// </summary>
        /// <param name="clientPath"></param>
        /// <returns></returns>
        private bool IsClientPathValid(string clientPath)
        {
            if (System.IO.File.Exists(clientPath))
                return (true);
            
            return (false);
        }

        /// <summary>
        /// Connects to a Telnet-Server and returns an established telnet session
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="pausetime"></param>
        /// <param name="retrys"></param>
        /// <returns></returns>
        private TelnetConnection ConnectTelnetSession(string ip, int port, int pausetime, int retrys)
        {
            int counter = retrys;
            
            while (counter > 0)
            {
                try
                {
                    return (new TelnetConnection(ip, port));
                }
                catch (SocketException)
                {
                    System.Threading.Thread.Sleep(pausetime);
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(pausetime);
                }
                counter--;
            }

            return (null);
        }

        #endregion
    }
}
