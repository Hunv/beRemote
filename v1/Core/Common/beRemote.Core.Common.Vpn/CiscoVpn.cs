using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using beRemote.Core.Common.Vpn.Cisco;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using Microsoft.Win32;

namespace beRemote.Core.Common.Vpn
{    
    // Reference for vpnclient.exe:
    // http://docstore.mik.ua/univercd/cc/td/doc/product/vpn/client/rel4_0/admin_gd/vcach4.htm
    
    public class CiscoVpn : VpnBase
    {
        private readonly ResourceDictionary _LangDictionary = new ResourceDictionary(); //Contains the Language-Variables

        #region Constructor
        public CiscoVpn()
        {
            LoadLanguage();
        }

        public CiscoVpn(string clientPath, string configName)
        {
            LoadLanguage();

            ClientPath = clientPath;
            ConfigName = configName;
        }

        public CiscoVpn(string clientPath, string configName, bool showLoginDialog)
        {
            LoadLanguage();

            ClientPath = clientPath;
            ConfigName = configName;
            ShowAuthenticationWindow = showLoginDialog;
        }

        public CiscoVpn(string clientPath, string configName, bool showLoginDialog, int credentialId)
        {
            LoadLanguage();

            ClientPath = clientPath;
            ConfigName = configName;
            ShowAuthenticationWindow = showLoginDialog;
            CredentialId = credentialId;
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
        
        public string ConfigName
        {
            get { return Parameter1; }
            set { Parameter1 = value; }
        }

        public string ClientPath
        {
            get { return Parameter2; }
            set { Parameter2 = value; }
        }

        public bool ShowAuthenticationWindow
        {
            get { return Convert.ToBoolean(Parameter3); }
            set { Parameter3 = value.ToString(); }
        }

        public int CredentialId
        {
            get { return Convert.ToInt32(Parameter4); }
            set { Parameter4 = value.ToString(); }
        }

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

            if (!System.IO.File.Exists(ClientPath))
            {
                MessageBox.Show(_LangDictionary["VpnCiscoClientMissing"].ToString(), _LangDictionary["VpnCiscoClientMissingTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }

            if (disconnectExistingConnections == false && IsConnectionEnabled(ClientPath))
            {
                MessageBox.Show(_LangDictionary["VpnCiscoAlreadyOpen"].ToString(), _LangDictionary["VpnCiscoAlreadyOpenTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }
            
            if (disconnectExistingConnections && IsConnectionEnabled(ClientPath))
            {
                VpnDisconnect(ClientPath);
            }

            UserCredential uC = StorageCore.Core.GetUserCredentials(CredentialId);

            if (uC != null)
            {
                return (VpnConnect(
                    ClientPath,
                    ConfigName,
                    uC.Username,
                    Helper.Helper.DecryptStringFromBytes(uC.Password, Helper.Helper.GetHash1(StorageCore.Core.GetUserSalt1()), Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid()), StorageCore.Core.GetUserSalt3()),
                    uC.Domain
                    ));
            }

            return (VpnConnect(ClientPath, ConfigName, "", null, ""));
        }

        /// <summary>
        /// Disconnect the current connection
        /// </summary>
        /// <returns>Disconnect successful or not</returns>
        public override bool Disconnect()
        {
            if (IsValid == false)                            
                return (false);
            
            return (VpnDisconnect(ClientPath));
        }

        /// <summary>
        /// Is OpenVPN installed?
        /// </summary>
        /// <returns></returns>
        public override bool IsInstalled()
        {
            if (System.Environment.Is64BitOperatingSystem)
            {
                if (Registry.LocalMachine.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Cisco Systems\\VPN Client\\InstallPath") == null &&
                    Registry.LocalMachine.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Cisco Systems\\VPN Client\\InstallPath") == null)
                {
                    return (false);
                }

            }
            else if (Registry.LocalMachine.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Cisco Systems\\VPN Client\\InstallPath") == null)
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
            if (ConfigName.Length < 1)
            {
                if (showError) MessageBox.Show(_LangDictionary["VpnCiscoEnterConfigname"].ToString(), _LangDictionary["VpnCiscoEnterConfignameTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }
                        
            if (ClientPath.Length < 8) //Smallest: c:\a.exe
            {
                if (showError) MessageBox.Show(_LangDictionary["VpnCiscoClientMissing"].ToString(), _LangDictionary["VpnCiscoClientMissingTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }

            if (ShowAuthenticationWindow == false &&
                CredentialId == 0)
            {
                if (showError) MessageBox.Show(_LangDictionary["VpnCiscoSelectCredential"].ToString(), _LangDictionary["VpnCiscoSelectCredentialTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
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
        /// <param name="configName"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private bool VpnConnect(string clientPath, string configName, string user, SecureString password, string domain)
        {
            /*
             * Cisco vpnclient.exe Usage:
                 vpnclient connect <profile> [user <username>] [eraseuserpwd | pwd <password>]
                                             [domain <domainname>]
                                             [nocertpwd] [cliauth] [stdin] [sd]
                 vpnclient disconnect
                 vpnclient stat [reset] [traffic] [tunnel] [route] [firewall] [repeat]
                 vpnclient notify
                 vpnclient verify [autoinitconfig]
                 vpnclient suspendfw
                 vpnclient resumefw
             */

            // Start the child process.
            var p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = clientPath;
            p.StartInfo.UseShellExecute = false;
            
            //With Credentials and Domain
            if (user != "" && password != null && domain != "")
            {
                p.StartInfo.Arguments = String.Format("connect {0} user {1} pwd {2} domain {3}", configName, user, Helper.Helper.ConvertToUnsecureString(password), domain);
            }
            //With Credentials and without Domain
            else if (user != "" && password != null && domain == "")
            {
                p.StartInfo.Arguments = String.Format("connect {0} user {1} pwd {2}", configName, user, Helper.Helper.ConvertToUnsecureString(password));
            }
            //Without Credentials
            else
            {
                p.StartInfo.Arguments = String.Format("connect {0}", configName);
            }
            
            p.Start();
            p.WaitForExit(60000);
            
            if (p.ExitCode >= 200)
            {
                IsConnected = true;
                //Maybe it is possible to get some more information like IP-Addresses etc. here and on every isconnected-Check
                return (true);
            }

            p.Close();

            MessageBox.Show("An error occured on connecting to VPN Gateway" + Environment.NewLine +
                            "Code: " + p.ExitCode + Environment.NewLine +
                            "Message: " + ((VpnClientReturnCode)p.ExitCode).ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return (false);
        }

        /// <summary>
        /// Disconnects the established connection
        /// </summary>
        /// <param name="clientPath"></param>
        /// <returns></returns>
        private bool VpnDisconnect(string clientPath)
        {
            // Start the child process.
            var p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = clientPath;
            p.StartInfo.Arguments = "disconnect";
            p.StartInfo.UseShellExecute = false;
            
            p.Start();
            p.WaitForExit(60000);
            
            if (p.ExitCode >= 200)
            {
                IsConnected = false;
                return (true);
            }

            p.Close();

            MessageBox.Show("An error occured on disconnecting from VPN Gateway" + Environment.NewLine +
                            "Code: " + p.ExitCode + Environment.NewLine +
                            "Message: " + ((VpnClientReturnCode)p.ExitCode).ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return (false);
        }

        /// <summary>
        /// Checks if there is currently an active connection
        /// </summary>
        /// <param name="clientPath"></param>
        /// <returns></returns>
        private bool IsConnectionEnabled(string clientPath)
        {
            /*
            C:\Program Files (x86)\Cisco Systems\VPN Client>vpnclient.exe stat tunnel
            Cisco Systems VPN Client Version 5.0.07.0440
            Copyright (C) 1998-2010 Cisco Systems, Inc. All Rights Reserved.
            Client Type(s): Windows, WinNT
            Running on: 6.2.9200
            Config file directory: C:\Program Files (x86)\Cisco Systems\VPN Client\

            Some of the requested information could not be retrieved.
            No connection exists.
            */

            // Start the child process.
            var p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = clientPath;
            p.StartInfo.Arguments = "stat tunnel";
            p.StartInfo.UseShellExecute = false;

            p.Start();
            p.WaitForExit(60000);

            if (p.ExitCode == 16)
            {
                IsConnected = false;
                return (false);
            }

            p.Close();

            IsConnected = true;
            return (true);
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

        #endregion
    }
}
