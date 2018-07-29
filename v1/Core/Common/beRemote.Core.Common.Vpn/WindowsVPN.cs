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

namespace beRemote.Core.Common.Vpn
{
    public class WindowsVPN : VpnBase
    {
        private readonly ResourceDictionary _LangDictionary = new ResourceDictionary(); //Contains the Language-Variables
        
        #region Constructor
        public WindowsVPN()
        {
            LoadLanguage();
        }

        public WindowsVPN(string configName)
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
            set 
            { 
                Parameter1 = value;                 
            }
        }

        public int CredentialId 
        {
            get { return Convert.ToInt32(Parameter4); } 
            set
            { 
                Parameter4 = value.ToString(); 
            } 
        }

        public bool ShowCredentialDialog
        {
            get 
            {
                if (Parameter3 == null)                
                    Parameter3 = true.ToString();

                return Convert.ToBoolean(Parameter3); 
            }
            set { Parameter3 = value.ToString(); }
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
                return false;

            if (IsInstalled() == false)
                return false;
            
            UserCredential uC = StorageCore.Core.GetUserCredentials(CredentialId);

            if (uC != null)
            {
                return (VpnConnect(
                    ConfigName,
                    uC.Username,
                    Helper.Helper.DecryptStringFromBytes(uC.Password, Helper.Helper.GetHash1(StorageCore.Core.GetUserSalt1()) , Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid()), StorageCore.Core.GetUserSalt3()),
                    uC.Domain
                    ));
            }
            
            return (VpnConnect(ConfigName, "", null, ""));
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
        /// Gets all VPN Connections, that 
        /// </summary>
        /// <returns></returns>
        public List<string> GetVpnConnections()
        {
            //// Start the child process.
            //var p = new Process();
            //// Redirect the output stream of the child process.
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.FileName = "rasdial.exe";

            //p.Start();
            //p.WaitForExit(60000);

            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var aAdapter in adapters)
            {
                
            }

            //Todo: get results ans return them in a List

            return (new List<string>());
        }

        /// <summary>
        /// Is rasdial installed?
        /// </summary>
        /// <returns></returns>
        public override bool IsInstalled()
        {
            var sysDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.System);

            return (System.IO.File.Exists(sysDir + "\\rasdial.exe"));
        }

        /// <summary>
        /// Validates the configuration; Executed before saving
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return(Validate(false));
        }

        /// <summary>
        /// Validates the configuration; Executed before saving
        /// </summary>
        /// <returns></returns>
        public override bool Validate(bool showError)
        {
            if (string.IsNullOrEmpty(ConfigName))
            {
                if (showError) MessageBox.Show(_LangDictionary["VpnWinSetName"].ToString(), _LangDictionary["VpnWinSetNameTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                return (false);
            }

            if (ShowCredentialDialog == false && CredentialId == 0)
            {
                if (showError) MessageBox.Show(_LangDictionary["VpnWinSelectCredential"].ToString(), _LangDictionary["VpnWinSelectCredentialTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
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
        private bool VpnConnect(string configName, string user, SecureString password, string domain)
        {
            // Start the child process.
            var p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "rasdial.exe";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //With Credentials and Domain
            if (user != "" && password != null && domain != "")
            {
                p.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\" {3} /domain:{2}", configName, user, domain, Helper.Helper.ConvertToUnsecureString(password), domain);
            }
            //With Credentials and without Domain
            else if (user != "" && password != null && domain == "")
            {
                p.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\" {2}", configName, user, Helper.Helper.ConvertToUnsecureString(password));
            }
            //Without Credentials
            else
            {
                p.StartInfo.Arguments = String.Format("\"{0}\"", configName);
            }
            
            p.Start();
            p.WaitForExit(60000);
            

            //Todo: Check if connection is established
            if (p.ExitCode == 0)
            {
                IsConnected = true;
                p.Close();
                return (true);
            }

            var exitCode = p.ExitCode;
            p.Close();

            MessageBox.Show("An error occured on connecting to VPN Gateway. Errorcode: " + exitCode.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return (false);
        }

        /// <summary>
        /// Disconnects the established connection
        /// </summary>
        /// <returns></returns>
        private bool VpnDisconnect()
        {
            // Start the child process.
            var p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "rasdial.exe";
            p.StartInfo.Arguments = "\"" + ConfigName + "\" /d";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit(60000);
            

            //Todo: check if connection was disconnected
            if (p.ExitCode == 0)
            {
                IsConnected = false;
                p.Close();
                return (true);
            }

            var errorcode = p.ExitCode;
            p.Close();

            MessageBox.Show("An error occured on disconnecting from VPN Gateway. Errorcode: " + errorcode.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return (false);
        }

        /// <summary>
        /// Checks if there is currently an active connection
        /// </summary>
        /// <returns></returns>
        private bool IsConnectionEnabled()
        {
            // Start the child process.
            var p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "rasdial.exe";
            p.StartInfo.Arguments = "";

            p.Start();
            p.WaitForExit(60000);

            //Todo: Analyse output
            var output = p.StandardOutput.ReadToEnd();

            foreach (var aLine in output.Replace("\r","").Split('\n'))
            {
                if (ConfigName.ToUpper() == aLine.ToUpper().Trim())
                {
                    p.Close();
                    IsConnected = true;
                    return (true);
                }
            }

            p.Close();
            IsConnected = false;
            return (false);
        }



        #endregion
    }
}
