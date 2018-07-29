using System.Net.NetworkInformation;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace beRemote.Core.Common.Vpn
{
    public class ShrewSoftVPN : VpnBase
    {
        private readonly ResourceDictionary _LangDictionary = new ResourceDictionary(); //Contains the Language-Variables

        #region Constructor
        public ShrewSoftVPN()
        {
            LoadLanguage();
        }

        public ShrewSoftVPN(string configName)
        {
            LoadLanguage();
            ConfigName = configName;            
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

        public string ConfigPath
        {
            get { return Parameter2; }
            set { Parameter2 = value; }
        }

        public bool ShowCredentialDialog
        {
            get { return Convert.ToBoolean(Parameter3); }
            set { Parameter3 = value.ToString(); }
        }

        public int CredentialId 
        {
            get { return Convert.ToInt32(Parameter4); } 
            set
            { 
                Parameter4 = value.ToString(); 
            } 
        }

        public bool IsValid
        {
            get { return Validate(); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Connect to a VPN Gateway. Set ConfigName first
        /// </summary>
        /// <returns>Connection successful or not</returns>
        public override bool Connect()
        {
            return (Connect(false));
        }

        /// <summary>
        /// Connect to a VPN Gateway. Set ConfigName first
        /// </summary>
        /// <param name="disconnectExistingConnections">Disconnect an existing other vpn-tunnel?</param>
        /// <returns>Connection successful or not</returns>
        public bool Connect(bool disconnectExistingConnections)
        {
            if (Validate(true) == false)
                return (false);

            if (!System.IO.File.Exists(ConfigPath))
            {
                MessageBox.Show(_LangDictionary["VpnShrewSetPath"].ToString(), _LangDictionary["VpnShrewSetPathTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }

            UserCredential uC = StorageCore.Core.GetUserCredentials(CredentialId);

            if (uC != null)
            {
                return (VpnConnect(
                    ConfigName,
                    ConfigPath,
                    uC.Username,
                    Helper.Helper.DecryptStringFromBytes(uC.Password, Helper.Helper.GetHash1(StorageCore.Core.GetUserSalt1()), Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid()), StorageCore.Core.GetUserSalt3()),
                    uC.Domain
                    ));
            }

            return (VpnConnect(ConfigName, ConfigPath, "", null, ""));
        }

        /// <summary>
        /// Disconnect the current connection
        /// </summary>
        /// <returns>Disconnect successful or not</returns>
        public override bool Disconnect()
        {
            return (VpnDisconnect());
        }

        /// <summary>
        /// Validates your connections; executed before saving
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return (Validate(false));
        }

        /// <summary>
        /// Validates your connections; executed before saving
        /// </summary>
        /// <returns></returns>
        public override bool Validate(bool showError)
        {
            if (string.IsNullOrEmpty(ConfigPath))
            {
                if (showError) MessageBox.Show(_LangDictionary["VpnShrewSetPath"].ToString(), _LangDictionary["VpnShrewSetPathTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }

            if (string.IsNullOrEmpty(ConfigName))
            {
                if (showError) MessageBox.Show(_LangDictionary["VpnShrewSetName"].ToString(), _LangDictionary["VpnShrewSetNameTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }

            if (ShowCredentialDialog == false && CredentialId == 0)
            {
                if (showError) MessageBox.Show(_LangDictionary["VpnShrewSelectCredential"].ToString(), _LangDictionary["VpnShrewSelectCredentialTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }

            return (true);
        }
        #endregion

        #region private Methods
        /// <summary>
        /// Connect to VPN-Gateway
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private bool VpnConnect(string configName, string configPath, string user, SecureString password, string domain)
        {
            // Start the child process.
            var p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = configPath;

            if (String.IsNullOrEmpty(domain))
                user = domain + "\\" + user;

            //With Credentials
            if (user != "" && password != null)
            {
                p.StartInfo.Arguments = String.Format("-r \"{0}\" -u \"{1}\" -p \"{2}\" -a", configName, user, Helper.Helper.ConvertToUnsecureString(password));
            }
            //Without Credentials
            else
            {
                p.StartInfo.Arguments = String.Format("-r \"{0}\" -a", configName);
            }
            
            p.Start();

            IsConnected = true;

            //Not able to get any kind of information, if connection was successfull
            return(true);    
        }

        /// <summary>
        /// Disconnects the established connection
        /// </summary>
        /// <returns></returns>
        private bool VpnDisconnect()
        {
            //Not possible
            return (false);
        }

        /// <summary>
        /// Checks if there is currently an active connection
        /// </summary>
        /// <returns></returns>
        private bool IsConnectionEnabled()
        {
            //Not possible
            return (false);
        }

        /// <summary>
        /// Is ShrewSoft VPN installed?
        /// </summary>
        /// <returns></returns>
        public new bool IsInstalled()
        {
            if (Registry.LocalMachine.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ShrewSoft\\path") == null)
            {
                return (false);
            }

            return (true);
        }

        #endregion
    }
}
