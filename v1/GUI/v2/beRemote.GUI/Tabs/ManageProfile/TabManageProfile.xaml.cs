using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using beRemote.Core.Common.Helper;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Tabs.ManageProfile.Commands;
using beRemote.Core.Common.LogSystem;
using beRemote.GUI.Controls.Items;
using beRemote.Core;
using beRemote.Core.ProtocolSystem.ProtocolBase;

namespace beRemote.GUI.Tabs.ManageProfile
{
    /// <summary>
    /// Interaction logic for TabManageProfile.xaml
    /// </summary>
    public partial class TabManageProfile
    {
        private readonly ResourceDictionary _LangDictionary = new ResourceDictionary(); //Contains the Language-Variables

        #region Command definition
        public CmdCancelClickImpl CmdCancelClick { get; set; }
        public CmdSaveClickImpl CmdSaveClick { get; set; }
        public CmdTabLoadedImpl CmdTabLoaded { get; set; }
        #endregion

        public TabManageProfile()
        {
            #region Load Language Dictionary
            var dictionaryFiles = new List<string>
                                           {
                                               "../Language/language.xaml",
                                               "../Language/language.de-DE.xaml",
                                               "../Language/language.es-ES.xaml",
                                               "../Language/language.fr-FR.xaml",
                                               "../Language/language.it-IT.xaml",
                                               "../Language/language.nl-NL.xaml",
                                               "../Language/language.pl-PL.xaml",
                                               "../Language/language.ru-RU.xaml",
                                               "../Language/language.zh-CN.xaml",
                                               "../Language/language.cs-CZ.xaml",
                                               "../Language/language.ar-SA.xaml",
                                               "../Language/language.bg-BG.xaml",
                                               "../Language/language.dk-DK.xaml",
                                               "../Language/language.el-GR.xaml",
                                               "../Language/language.fa-IR.xaml",
                                               "../Language/language.fi-FI.xaml",
                                               "../Language/language.he-IL.xaml",
                                               "../Language/language.hi-IN.xaml",
                                               "../Language/language.hr-HR.xaml",
                                               "../Language/language.hu-HU.xaml",
                                               "../Language/language.ko-KR.xaml",
                                               "../Language/language.nn-NO.xaml",
                                               "../Language/language.se-SE.xaml",
                                               "../Language/language.tr-TR.xaml",
                                               "../Language/language.zh-CN.xaml"
                                           };

            foreach (var aLangfile in dictionaryFiles)
                _LangDictionary.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(aLangfile, UriKind.Relative) });

            #endregion

            #region Commands
            CmdCancelClick = new CmdCancelClickImpl();
            CmdSaveClick = new CmdSaveClickImpl();
            CmdTabLoaded = new CmdTabLoadedImpl();

            CmdCancelClick.CloseTab += CmdCancelClick_CloseTab;
            CmdSaveClick.Save += CmdSaveClick_Save;
            CmdTabLoaded.Loaded += CmdTabLoaded_Loaded;
            #endregion

            InitializeComponent();
        }       

        #region Command-Events
        public void CmdCancelClick_CloseTab(object sender, RoutedEventArgs e)
        {
            CloseTab();
        }

        public void CmdSaveClick_Save(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        public void CmdTabLoaded_Loaded(object sender, RoutedEventArgs e)
        {
            FolderList = GetFolderList();
            RaisePropertyChanged("FolderList");
            LoadSettings();
        }
        #endregion

        #region Methods

        public void LoadSettings()
        {
            //Load the UserSettings
            var userSettings = StorageCore.Core.GetUserSettings();

            //Set the Username (=Loginname)
            Username = userSettings.Winname;

            //Set the Displayname
            Displayname = userSettings.Name;

            //Get the Statistics
            Statistics =
                "Last login: " + userSettings.LastLogin + Environment.NewLine +
                "Last machine: " + userSettings.LastMachine + Environment.NewLine +
                "Logins: " + userSettings.LoginCount;

            //Load Credentials-List
            CredentialList = new ObservableCollection<UserCredential>(StorageCore.Core.GetUserCredentialsAll());
            
            //If logged in User is Superadmin: Show ID
            if (StorageCore.Core.GetUserSuperadmin())
                Statistics += Environment.NewLine + "UserID: " + userSettings.Id;

            //Set the advanced settings
            DeleteQuickConnections = userSettings.DeleteQuickConnect;
            SelectedDefaultFolder = new ConnectionItem("x", ConnectionTypeItems.folder, null, null, null, userSettings.DefaultFolder, "", "", "");
            SelectedDefaultProtocol = userSettings.DefaultProtocol;

            //Load Proxysettings
            var ups = StorageCore.Core.GetUserProxySettings();
            if (ups == null || ups.ConfiguredProxy == null)
            {
                //No Proxy
                ProxyNoProxy = true;
            }
            else
            {
                if (ups.UseSystemSettings)
                {
                    //System-Settings
                    ProxySystemSettings = true;
                }
                else
                {
                    //Custom Settings
                    ProxyCustomSettings = true;

                    ProxyHostname = ups.ConfiguredProxy.Address.Host;
                    ProxyPort = ups.ConfiguredProxy.Address.Port;
                    ProxyBypass = ups.ConfiguredProxy.BypassProxyOnLocal;

                    if (ups.ConfiguredProxy.UseDefaultCredentials)
                    {
                        ProxyCurrentCredentials = true;
                    }
                    else if (ups.ConfiguredProxy.Credentials == null)
                    {
                        ProxyNoCredentials = true;
                    }
                    else
                    {
                        if (ups.UserCredentialId == -1)
                        {
                            ProxyCurrentCredentials = true;
                        }
                        else if (ups.UserCredentialId == 0)
                        {
                            ProxyNoCredentials = true;
                        }
                        else
                        {
                            ProxyDefinedCredentials = true;
                            ProxyCredentials = ups.UserCredentialId;
                        }
                    }
                }
            }
        }

        public void SaveSettings()
        {
            if (String.IsNullOrWhiteSpace(Displayname) || String.IsNullOrEmpty(Displayname))
            {
                if (_LangDictionary.Contains("ErrorDisplayname") && _LangDictionary.Contains("ErrorDisplaynameTitle"))
                    MessageBox.Show(_LangDictionary["ErrorDisplayname"].ToString(), _LangDictionary["ErrorDisplaynameTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Get userId
            var userId = StorageCore.Core.GetUserId();

            //Change data except Password
            StorageCore.Core.ModifyUser(userId, Displayname, (byte[])null);

            //Save the Proxy Settings
            SaveProxySettings();

            //Change the password, if required
            ChangePassword();

            //Change advanced Settings
            SaveAdvancedSettings();

            CloseTab();
        }

        /// <summary>
        /// Save Settings, defined in the Advanced-Tab 
        /// </summary>
        private void SaveAdvancedSettings()
        {
            StorageCore.Core.SetUserSettings("deletequickconnect", DeleteQuickConnections);
            StorageCore.Core.SetUserSettings("defaultfolder", SelectedDefaultFolder.ConnectionID);
            StorageCore.Core.ModifyUserDefaultProtocol(SelectedDefaultProtocol);

        }

        /// <summary>
        /// Saves the new Proxy-Settings
        /// </summary>
        private void SaveProxySettings()
        {
            //Save Proxy-Settings
            if (StoreLocal)
            {
                #region Save proxy settings local
                var proxyConfigFilePath = Environment.SpecialFolder.LocalApplicationData + "\\beRemote\\proxy.cfg";

                //Create new file; overwrite if exists
                var sW = new StreamWriter(proxyConfigFilePath, false, Encoding.Default);

                //Save settings global
                if (ProxyNoProxy)
                {
                    //No Proxy
                    sW.WriteLine("proxyenabled:0");
                    sW.WriteLine("proxyhost:");
                    sW.WriteLine("proxyport:0");
                    sW.WriteLine("proxycredentials:0");
                    sW.WriteLine("proxyflags:0");
                }
                else if (ProxySystemSettings)
                {
                    //Proxy with using IE-Settings
                    sW.WriteLine("proxyenabled:1");
                    sW.WriteLine("proxyhost:");
                    sW.WriteLine("proxyport:0");
                    sW.WriteLine("proxycredentials:0");
                    sW.WriteLine("proxyflags:2");
                }
                else if (ProxyCustomSettings)
                {
                    //User defined settings
                    if (ProxyHostname == "")
                    {
                        //todo:
                        //MessageBox.Show("Please enter a hostname or IP into the proxyhost-field.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (ProxyNoCredentials)
                    {
                        //Save Proxy without credentials
                        sW.WriteLine("proxyenabled:1");
                        sW.WriteLine("proxyhost:" + ProxyHostname);
                        sW.WriteLine("proxyport:" + ProxyPort);
                        sW.WriteLine("proxycredentials:0");
                        sW.WriteLine(ProxyBypass ? "proxyflags:1" : "proxyflags:0");
                    }
                    else if (ProxyCurrentCredentials)
                    {
                        //Save Proxy with current credentials
                        sW.WriteLine("proxyenabled:1");
                        sW.WriteLine("proxyhost:" + ProxyHostname);
                        sW.WriteLine("proxyport:" + ProxyPort);
                        sW.WriteLine("proxycredentials:-1");
                        sW.WriteLine(ProxyBypass ? "proxyflags:1" : "proxyflags:0");
                    }
                    else if (ProxyCustomSettings)
                    {
                        //Save Proxy with custom credentials
                        sW.WriteLine("proxyenabled:1");
                        sW.WriteLine("proxyhost:" + ProxyHostname);
                        sW.WriteLine("proxyport:" + ProxyPort);
                        sW.WriteLine("proxycredentials:" + ProxyCredentials);
                        sW.WriteLine(ProxyBypass ? "proxyflags:1" : "proxyflags:0");
                    }
                    else
                    {
                        //todo:
                        //MessageBox.Show("Please selecte a credentialoption to save your settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                sW.Close();
                #endregion
            }
            else
            {
                #region Save proxy settings global
                if (ProxyNoProxy)
                {
                    //No Proxy
                    StorageCore.Core.SetUserProxySettings(false);
                }
                else if (ProxySystemSettings)
                {
                    //Proxy with using IE-Settings
                    StorageCore.Core.SetUserProxySettings(true, true);
                }
                else if (ProxyCustomSettings)
                {
                    if (ProxyHostname == "")
                    {
                        //todo:
                        //MessageBox.Show("Please enter a hostname or IP into the proxyhost-field.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (ProxyNoCredentials)
                        {
                            //Save Proxy without credentials
                            StorageCore.Core.SetUserProxySettings(ProxyHostname, ProxyPort, 0, ProxyBypass);
                        }
                        else if (rbProxyCredCurrent.IsChecked == true)
                        {
                            //Save Proxy with current credentials
                            StorageCore.Core.SetUserProxySettings(ProxyHostname, ProxyPort, -1, ProxyBypass);
                        }
                        else if (rbProxyCredCustom.IsChecked == true)
                        {
                            //Save Proxy with custom credentials
                            StorageCore.Core.SetUserProxySettings(ProxyHostname, ProxyPort, ProxyCredentials, ProxyBypass);
                        }
                        else
                        {
                            //todo:
                            //MessageBox.Show("Please selecte a credentialoption to save your settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Change the Password; this is not MVVM-conform because binding of PasswordBox is not possible
        /// </summary>
        private void ChangePassword()
        {
            //Check if a Change is required; If old Password field is emtpy => no password change
            if (pbOld.SecurePassword.Length == 0)
            {                
                return;
            }

            //Check if the new password is long enough
            if (pbNew1.SecurePassword.Length < 3 || pbNew2.SecurePassword.Length < 3)
            {
                //Message "too short"
                if (_LangDictionary.Contains("TabManageProfileErrorNewPasswordToShort") && _LangDictionary.Contains("TabManageProfileErrorNewPasswordToShortTitle"))
                    MessageBox.Show(_LangDictionary["TabManageProfileErrorNewPasswordToShort"].ToString(), _LangDictionary["TabManageProfileErrorNewPasswordToShortTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Check if new Passwords are matching
            if (!pbNew1.SecurePassword.SecureStringsAreEqual(pbNew2.SecurePassword))
            {
                //Message "password not matching"
                if (_LangDictionary.Contains("TabManageProfileErrorNewPasswordUnmatch") && _LangDictionary.Contains("TabManageProfileErrorNewPasswordUnmatchTitle"))
                    MessageBox.Show(_LangDictionary["TabManageProfileErrorNewPasswordUnmatch"].ToString(), _LangDictionary["TabManageProfileErrorNewPasswordUnmatchTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Check if old password matches
            if (!StorageCore.Core.CheckUserPassword(
                Helper.GetPasswordHash(
                    pbOld.SecurePassword,
                    StorageCore.Core.GetUserSalt1(),
                    StorageCore.Core.GetUserSalt2()
                    )))
            {
                //Message "old password wrong"
                if (_LangDictionary.Contains("TabManageProfileErrorOldPassword") && _LangDictionary.Contains("TabManageProfileErrorOldPasswordTitle"))
                    MessageBox.Show(_LangDictionary["TabManageProfileErrorOldPassword"].ToString(), _LangDictionary["TabManageProfileErrorOldPasswordTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Get the Hash1 with old Password
            var hash1Old = Helper.GetHash1(StorageCore.Core.GetUserSalt1());

            //Change user password
            StorageCore.Core.ModifyUserPassword(
                StorageCore.Core.GetUserId(),
                Helper.GetPasswordHash(pbNew1.SecurePassword, StorageCore.Core.GetUserSalt1(), StorageCore.Core.GetUserSalt2()));

            //Set Cache for new password
            Helper.SetUserPassword(pbNew1.SecurePassword);

            //Recrypt all credentials
            var credentials = StorageCore.Core.GetUserCredentialsAll();
            foreach (var cred in credentials)
            {
                if (cred == null)
                    continue;

                try
                {
                    var oldPw = Helper.DecryptStringFromBytes(
                                    cred.Password,
                                    hash1Old,
                                    Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid()),
                                    StorageCore.Core.GetUserSalt3());

                    var newPw = Helper.EncryptStringToBytes(
                                    oldPw,
                                    Helper.GetHash1(StorageCore.Core.GetUserSalt1()),
                                    Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid().ToCharArray()),
                                    StorageCore.Core.GetUserSalt3());

                    StorageCore.Core.ModifyUserCredential(cred.Id, cred.Username, newPw, cred.Domain, cred.Owner, cred.Description);
                }
                catch (Exception ea)
                {
                    Logger.Log(LogEntryType.Warning, "Error on recrypting credential with ID " + cred.Id.ToString(), ea);
                }
            }
        }

        #region Get FolderList
        /// <summary>
        /// Gets a non-Stacked List of all Folders. So there is just one Collection, that includes all Items. The Subconnections-Property is Unused.
        /// </summary>
        private ObservableCollection<ConnectionItem> GetFolderList()
        {
            var listFolder = StorageCore.Core.GetFolders(); //Contains all folders, that are not directed to a parent
            var rootList = new List<ConnectionItem>(); //Contains all folders, that are directed to a parent

            foreach (var aFolder in listFolder)
            {
                if (aFolder.ParentId == 0)
                {
                    rootList.InsertRange(rootList.Count, GetSubFolders(aFolder.Id, listFolder, 0));
                }
            }

            return (new ObservableCollection<ConnectionItem>(rootList));
        }

        private List<ConnectionItem> GetSubFolders(long mainFolderId, List<Folder> folderList, byte rootLevel)
        {
            var conItem = new ConnectionItem("tempname");
            var conList = new List<ConnectionItem>();

            //Foreach Folder
            foreach (var aFolder in folderList)
            {
                //Search the folder itself
                if (aFolder.Id == mainFolderId)
                {
                    conItem.ConnectionName = aFolder.Name;
                    conItem.ConnectionID = aFolder.Id;
                    conItem.RootLevel = rootLevel;
                }

                //Search the Folder-Childs
                if (aFolder.ParentId == mainFolderId)
                {
                    var listSubConnections = GetSubFolders(aFolder.Id, folderList, (byte)(rootLevel + 1));
                    conItem.SubConnections = new ObservableCollection<ConnectionItem>(listSubConnections);
                    conList.InsertRange(conList.Count, listSubConnections);
                }
            }

            conList.Insert(0, conItem);

            return (conList);
        }
        #endregion
        #endregion

        #region Properties

        #region CredentialList
        private ObservableCollection<UserCredential> _CredentialList = new ObservableCollection<UserCredential>();

        public ObservableCollection<UserCredential> CredentialList
        {
            get { return _CredentialList; }
            private set
            {
                _CredentialList = value;
                RaisePropertyChanged("CredentialList");
            }
        }
        #endregion 

        #region Username
        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Username.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(TabManageProfile), new PropertyMetadata(""));
        #endregion

        #region Displayname
        public string Displayname
        {
            get { return (string)GetValue(DisplaynameProperty); }
            set { SetValue(DisplaynameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Displayname.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplaynameProperty =
            DependencyProperty.Register("Displayname", typeof(string), typeof(TabManageProfile), new PropertyMetadata(""));
        #endregion

        #region ProxyHostname
        public string ProxyHostname
        {
            get { return (string)GetValue(ProxyHostnameProperty); }
            set { SetValue(ProxyHostnameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxyHostname.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProxyHostnameProperty =
            DependencyProperty.Register("ProxyHostname", typeof(string), typeof(TabManageProfile), new PropertyMetadata(""));
        #endregion

        #region ProxyPort
        public int ProxyPort
        {
            get { return (int)GetValue(ProxyPortProperty); }
            set { SetValue(ProxyPortProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxyPort.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProxyPortProperty =
            DependencyProperty.Register("ProxyPort", typeof(int), typeof(TabManageProfile), new PropertyMetadata(8080));
        #endregion

        #region ProxyCredentials
        public int ProxyCredentials
        {
            get { return (int)GetValue(ProxyCredentialsProperty); }
            set { SetValue(ProxyCredentialsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxyCredentials.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProxyCredentialsProperty =
            DependencyProperty.Register("ProxyCredentials", typeof(int), typeof(TabManageProfile), new PropertyMetadata(0));
        #endregion

        #region ProxyBypass
        public bool ProxyBypass
        {
            get { return (bool)GetValue(ProxyBypassProperty); }
            set { SetValue(ProxyBypassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxyBypass.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProxyBypassProperty =
            DependencyProperty.Register("ProxyBypass", typeof(bool), typeof(TabManageProfile), new PropertyMetadata(true));
        #endregion

        #region StoreLocal
        public bool StoreLocal
        {
            get { return (bool)GetValue(StoreLocalProperty); }
            set { SetValue(StoreLocalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StoreLocal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StoreLocalProperty =
            DependencyProperty.Register("StoreLocal", typeof(bool), typeof(TabManageProfile), new PropertyMetadata(false));
        #endregion

        #region Statistics
        public string Statistics
        {
            get { return (string)GetValue(StatisticsProperty); }
            set { SetValue(StatisticsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Statistics.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatisticsProperty =
            DependencyProperty.Register("Statistics", typeof(string), typeof(TabManageProfile), new PropertyMetadata(""));
        #endregion

        #region ProxyNoProxy
        public bool ProxyNoProxy
        {
            get { return (bool)GetValue(ProxyNoProxyProperty); }
            set { SetValue(ProxyNoProxyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxyNoProxy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProxyNoProxyProperty =
            DependencyProperty.Register("ProxyNoProxy", typeof(bool), typeof(TabManageProfile), new PropertyMetadata(false));
        #endregion

        #region ProxySystemSettings
        public bool ProxySystemSettings
        {
            get { return (bool)GetValue(ProxySystemSettingsProperty); }
            set { SetValue(ProxySystemSettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxySystemSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProxySystemSettingsProperty =
            DependencyProperty.Register("ProxySystemSettings", typeof(bool), typeof(TabManageProfile), new PropertyMetadata(false));
        #endregion

        #region ProxyCustomSettings
        public bool ProxyCustomSettings
        {
            get { return (bool)GetValue(ProxyCustomSettingsProperty); }
            set { SetValue(ProxyCustomSettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxyCustomSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProxyCustomSettingsProperty =
            DependencyProperty.Register("ProxyCustomSettings", typeof(bool), typeof(TabManageProfile), new PropertyMetadata(false));
        #endregion

        #region ProxyNoCredentials
        public bool ProxyNoCredentials
        {
            get { return (bool)GetValue(ProxyNoCredentialsProperty); }
            set { SetValue(ProxyNoCredentialsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxyNoCredentials.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProxyNoCredentialsProperty =
            DependencyProperty.Register("ProxyNoCredentials", typeof(bool), typeof(TabManageProfile), new PropertyMetadata(false));
        #endregion

        #region ProxyCurrentCredentials
        public bool ProxyCurrentCredentials
        {
            get { return (bool)GetValue(ProxyCurrentCredentialsProperty); }
            set { SetValue(ProxyCurrentCredentialsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxyCurrentCredentials.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProxyCurrentCredentialsProperty =
            DependencyProperty.Register("ProxyCurrentCredentials", typeof(bool), typeof(TabManageProfile), new PropertyMetadata(false));
        #endregion

        #region ProxyDefinedCredentails
        public bool ProxyDefinedCredentials
        {
            get { return (bool)GetValue(ProxyDefinedCredentialsProperty); }
            set { SetValue(ProxyDefinedCredentialsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxyDefiniedCredentails.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProxyDefinedCredentialsProperty =
            DependencyProperty.Register("ProxyDefinedCredentials", typeof(bool), typeof(TabManageProfile), new PropertyMetadata(false));
        #endregion

        #region DeleteQuickConnections
        private bool _DeleteQuickConnections;
        public bool DeleteQuickConnections
        {
            get { return _DeleteQuickConnections; }
            set 
            {
                _DeleteQuickConnections = value;
                RaisePropertyChanged("DeleteQuickConnections");
            }
        }
        #endregion
        
        #region FolderList
        private ObservableCollection<ConnectionItem> _FolderList = new ObservableCollection<ConnectionItem>();

        /// <summary>
        /// Contains the FolderList
        /// </summary>
        public ObservableCollection<ConnectionItem> FolderList
        {
            get { return _FolderList; }
            set
            {
                _FolderList = value;
                RaisePropertyChanged("FolderList");
            }
        }
        #endregion

        #region SelectedDefaultFolder
        private ConnectionItem _SelectedDefaultFolder;
        public ConnectionItem SelectedDefaultFolder
        {
            get { return _SelectedDefaultFolder; }
            set
            {
                if (value == null)
                {
                    _SelectedDefaultFolder = null;
                    RaisePropertyChanged("SelectedDefaultFolder");
                    return;
                }

                foreach (var conItm in FolderList)
                {
                    if (conItm.ConnectionID != value.ConnectionID)
                        continue;

                    _SelectedDefaultFolder = conItm;
                }

                RaisePropertyChanged("SelectedDefaultFolder");
            }
        }
        #endregion

        #region ProtocolList

        /// <summary>
        /// The Protocol-Buttons in the Server-Ribbon; set just sends a property-changed
        /// </summary>
        public IList<Protocol> ProtocolList
        {
            get
            {
                if (Kernel.IsKernelReady())
                    return Kernel.GetAvailableProtocols().Values;

                return (new List<Protocol>());
            }
        }

        #endregion

        #region SelectedDefaultProtocol
        private string _SelectedDefaultProtocol = "";

        /// <summary>
        /// Contains the SelectedDefaultProtocol
        /// </summary>
        public string SelectedDefaultProtocol
        {
            get { return _SelectedDefaultProtocol; }
            set
            {
                _SelectedDefaultProtocol = value;
                RaisePropertyChanged("SelectedDefaultProtocol");
            }
        }
        #endregion
        


        #endregion

        public override void Dispose()
        {
            base.Dispose();
            _LangDictionary.Clear();
            _CredentialList.Clear();
            _FolderList.Clear();
            _SelectedDefaultFolder = null;
            ProtocolList.Clear();

            CmdCancelClick.CloseTab -= CmdCancelClick_CloseTab;
            CmdSaveClick.Save -= CmdSaveClick_Save;
            CmdTabLoaded.Loaded -= CmdTabLoaded_Loaded;

            CmdCancelClick = null;
            CmdSaveClick = null;
            CmdTabLoaded = null;

            

        }
    }
}
