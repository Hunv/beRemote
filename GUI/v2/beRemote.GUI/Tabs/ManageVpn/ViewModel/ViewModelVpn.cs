using beRemote.Core.Common.Vpn;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.Tabs.ManageVpn.ViewModel.Commands;

namespace beRemote.GUI.Tabs.ManageVpn.ViewModel
{
    public class ViewModelVpn : INotifyPropertyChanged
    {
        #region private Variables
        private VpnManager _VpnMgr = new VpnManager();
        private readonly ResourceDictionary _LangDictionary = new ResourceDictionary(); //Contains the Language-Variables
        #endregion

        #region Command Definitions
        public CmdVpnTabLoadedImpl CmdVpnTabLoaded { get;set;}
        public CmdVpnTabUnloadedImpl CmdVpnTabUnloaded { get; set; }
        public CmdVpnTabBrowseImpl CmdVpnTabBrowse { get; set; }
        #endregion

        #region Constructor
        public ViewModelVpn()
        {
            #region Command Initialisation
            CmdVpnTabLoaded = new CmdVpnTabLoadedImpl();
            CmdVpnTabUnloaded = new CmdVpnTabUnloadedImpl();
            CmdVpnTabBrowse = new CmdVpnTabBrowseImpl();

            CmdVpnTabLoaded.TabLoaded += CmdVpnTabLoaded_TabLoaded;
            CmdVpnTabBrowse.BrowseFile += CmdVpnTabBrowse_BrowseFile;
            #endregion
        }
        #endregion

        #region Command Events

        void CmdVpnTabBrowse_BrowseFile(object sender, BrowseFileEventArgs e)
        {
            switch (e.BrowseSender)
            {
                case "c1":
                case "s1":
                case "o2": //Path of exe
                    SelectedVpn.Parameter2 = e.BrowsePath;
                    break;
                case "o1": //Config
                    SelectedVpn.Parameter1 = e.BrowsePath;
                    break;
            }

            RaisePropertyChanged("SelectedVpn");
            //        RaisePropertyChanged("SelectedItem.Parameter2");
        }

        void CmdVpnTabLoaded_TabLoaded(object sender, RoutedEventArgs e)
        {
            #region Load Language Dictionary
            var dictionaryFiles = new List<string>
                                           {
                                               "Tabs/ManageVpn/Language/language.xaml",
                                               "Tabs/ManageVpn/Language/language.de-DE.xaml",
                                               "Tabs/ManageVpn/Language/language.es-ES.xaml",
                                               "Tabs/ManageVpn/Language/language.fr-FR.xaml",
                                               "Tabs/ManageVpn/Language/language.it-IT.xaml",
                                               "Tabs/ManageVpn/Language/language.nl-NL.xaml",
                                               "Tabs/ManageVpn/Language/language.pl-PL.xaml",
                                               "Tabs/ManageVpn/Language/language.ru-RU.xaml",
                                               "Tabs/ManageVpn/Language/language.zh-CN.xaml",
                                               "Tabs/ManageVpn/Language/language.cs-CZ.xaml",
                                               "Tabs/ManageVpn/Language/language.ar-SA.xaml",
                                               "Tabs/ManageVpn/Language/language.bg-BG.xaml",
                                               "Tabs/ManageVpn/Language/language.dk-DK.xaml",
                                               "Tabs/ManageVpn/Language/language.el-GR.xaml",
                                               "Tabs/ManageVpn/Language/language.fa-IR.xaml",
                                               "Tabs/ManageVpn/Language/language.fi-FI.xaml",
                                               "Tabs/ManageVpn/Language/language.he-IL.xaml",
                                               "Tabs/ManageVpn/Language/language.hi-IN.xaml",
                                               "Tabs/ManageVpn/Language/language.hr-HR.xaml",
                                               "Tabs/ManageVpn/Language/language.hu-HU.xaml",
                                               "Tabs/ManageVpn/Language/language.ko-KR.xaml",
                                               "Tabs/ManageVpn/Language/language.nn-NO.xaml",
                                               "Tabs/ManageVpn/Language/language.se-SE.xaml",
                                               "Tabs/ManageVpn/Language/language.tr-TR.xaml",
                                               "Tabs/ManageVpn/Language/language.zh-CN.xaml"
                                           };

            foreach (var aLangfile in dictionaryFiles)
                _LangDictionary.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(aLangfile, UriKind.Relative) });

            #endregion

            loadCredentialBoxes();
            loadWindowsVpnConnections();
            loadVpnConnections();
        }
        #endregion

        #region Properties
        #region general

        private ObservableCollection<VpnBase> _VpnList = new ObservableCollection<VpnBase>();

        public ObservableCollection<VpnBase> VpnList
        {
            get { return _VpnList; }
            set 
            {
                _VpnList = value;
                RaisePropertyChanged("VpnList");
            }
        }

        private VpnBase _SelectedVpn;

        public VpnBase SelectedVpn
        {
            get { return _SelectedVpn; }
            set
            {
                _SelectedVpn = value;
                RaisePropertyChanged("SelectedVpn");

                //if (SelectedVpn != null)
                //    cmbMethod.SelectedIndex = SelectedVpn.TypeId;
            }
        }

        public ObservableCollection<VpnType> VpnTypeList = new ObservableCollection<VpnType>();

        public List<UserCredential> VpnUserCredentials { get; set; }
        #endregion

        #region OpenVpn
        private string _OpenVpnConfigPath = "";

        public string OpenVpnConfigPath
        {
            get { return (_OpenVpnConfigPath); }
            set
            {
                _OpenVpnConfigPath = value;
                RaisePropertyChanged("OpenVpnConfigPath");
            }
        }

        private string _OpenVpnExePath = "";

        public string OpenVpnExePath
        {
            get { return (_OpenVpnExePath); }
            set
            {
                _OpenVpnExePath = value;
                RaisePropertyChanged("OpenVpnExePath");
            }
        }
        #endregion

        #region CiscoVPN
        private string _CiscoVpnConfigName = "";

        public string CiscoVpnConfigName
        {
            get { return (_CiscoVpnConfigName); }
            set
            {
                _CiscoVpnConfigName = value;
                RaisePropertyChanged("CiscoVpnConfigName");
            }
        }

        private string _CiscoVpnExePath = @"C:\Program Files (x86)\Cisco Systems\VPN Client\vpnclient.exe";

        public string CiscoVpnExePath
        {
            get { return (_CiscoVpnExePath); }
            set
            {
                _CiscoVpnExePath = value;
                RaisePropertyChanged("CiscoVpnExePath");
            }
        }

        public bool CiscoVpnShowLogin { get; set; }
        public UserCredential CiscoVpnSelectedCredential { get; set; }
        #endregion

        #region WindowsVPN
        private ObservableCollection<string> _WindowsVpnList = new ObservableCollection<string>();
        public ObservableCollection<string> WindowsVpnList
        {
            get { return _WindowsVpnList; }
            set
            {
                _WindowsVpnList = value;
                RaisePropertyChanged("WindowsVpnList");
            }
        }
        #endregion
        #endregion        

        #region Methods

        #region Received from Context-Ribbon
        /// <summary>
        /// Adds a new VPN Connection to the VPN-List
        /// </summary>
        /// <param name="vpnType"></param>
        public void AddVpn(int vpnType)
        {
            StorageCore.Core.AddUserVpnConnection(vpnType, "", "", "", "", "", "", "", "", "", "", _LangDictionary["NewVpnConnection"].ToString());
            loadVpnConnections();
            RaisePropertyChanged("VpnList");
        }

        public void RemoveVpn()
        {
            if (SelectedVpn == null)
                return;
            
            StorageCore.Core.DeleteUserVpnConnection(SelectedVpn.Id);
            loadVpnConnections();
        }

        public void Save()
        {
            if (VpnList == null)
                return;

            //Validate Data
            List<VpnBase> toRemove = new List<VpnBase>();
            foreach (VpnBase aVpn in VpnList)
            {
                if (aVpn.Type == VpnType.Undefined)
                {
                    toRemove.Add(aVpn);
                    continue;
                }

                if (aVpn.Validate(true) == false)
                    return;
            }

            //Remove Undefined VPNs
            foreach (VpnBase aVpn in toRemove)
            {
                StorageCore.Core.DeleteUserVpnConnection(aVpn.Id);                
                VpnList.Remove(aVpn);
            }

            //Save Data
            foreach (VpnBase aVpn in VpnList)
            {
                StorageCore.Core.EditUserVpnConnection(
                    aVpn.Id,
                    aVpn.TypeId,
                    aVpn.Parameter1,
                    aVpn.Parameter2,
                    aVpn.Parameter3,
                    aVpn.Parameter4,
                    aVpn.Parameter5,
                    aVpn.Parameter6,
                    aVpn.Parameter7,
                    aVpn.Parameter8,
                    aVpn.Parameter9,
                    aVpn.Parameter10,
                    aVpn.Name);
            }
        }

        public void Test()
        {
            if (SelectedVpn == null)
                return;

            if (SelectedVpn.Type == VpnType.Undefined)
            {
                MessageBox.Show(_LangDictionary["ConfigureFirst"].ToString(), _LangDictionary["ConfigureFirstTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            switch (SelectedVpn.Type)
            {
                #region CiscoVpn
                case VpnType.CiscoVpn:
                    if (SelectedVpn == null)
                        return;

                    var cVpn = new CiscoVpn();
                    cVpn.ClientPath = SelectedVpn.Parameter2;
                    cVpn.ConfigName = SelectedVpn.Parameter1;
                    cVpn.ShowAuthenticationWindow = Convert.ToBoolean(SelectedVpn.Parameter3);
                    cVpn.CredentialId = Convert.ToInt32(SelectedVpn.Parameter4);

                    if (cVpn.Connect())
                    {
                        MessageBox.Show(_LangDictionary["CiscoVpnTestStep1"].ToString(), _LangDictionary["CiscoVpnTestStep1Title"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                        cVpn.Disconnect();
                    }
                    else
                    {
                        MessageBox.Show(_LangDictionary["CiscoVpnTestStep1Fail"].ToString(), _LangDictionary["CiscoVpnTestStep1FailTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                #endregion
                #region OpenVpn
                case VpnType.OpenVpn:
                    if (SelectedVpn == null)
                        return;

                    var oVpn = new OpenVpn();
                    oVpn.ConfigPath = SelectedVpn.Parameter2; //exe file
                    oVpn.ConfigName = SelectedVpn.Parameter1; //Config file

                    if (oVpn.Connect())
                    {
                        MessageBox.Show(_LangDictionary["OvVpnTestStep1"].ToString(), _LangDictionary["OvVpnTestStep1Title"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                        oVpn.Disconnect();
                    }
                    else
                    {
                        MessageBox.Show(_LangDictionary["OvVpnTestStep1Fail"].ToString(), _LangDictionary["OvVpnTestStep1FailTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                #endregion
                #region ShrewSoftVpn
                case VpnType.ShrewSoftVpn:
                    if (SelectedVpn == null)
                        return;

                    var sVpn = new ShrewSoftVPN();
                    sVpn.ConfigName = SelectedVpn.Parameter1;
                    sVpn.ConfigPath = SelectedVpn.Parameter2;
                    sVpn.Parameter3 = SelectedVpn.Parameter3;
                    sVpn.CredentialId = Convert.ToInt32(SelectedVpn.Parameter4);
                    if (sVpn.Connect() == true)
                        MessageBox.Show(_LangDictionary["SsVpnTestStep1"].ToString(), _LangDictionary["SsVpnTestStep1Title"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show(_LangDictionary["SsVpnTestStep1Fail"].ToString(), _LangDictionary["SsVpnTestStep1FailTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                #endregion
                #region WindowsVpn
                case VpnType.WindowsVpn:
                    var wVpn = new WindowsVPN();
                    wVpn.ConfigName = SelectedVpn.Parameter1;
                    wVpn.CredentialId = Convert.ToInt32(SelectedVpn.Parameter4);
                    if (wVpn.Connect() == true)
                    {
                        MessageBox.Show(_LangDictionary["WinVpnTestStep1"].ToString(), _LangDictionary["WinVpnTestStep1Title"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                        wVpn.Disconnect();
                    }
                    else
                    {
                        MessageBox.Show(_LangDictionary["WinVpnTestStep1Fail"].ToString(), _LangDictionary["WinVpnTestStep1FailTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                #endregion
            }
        }

        #endregion

        private void loadCredentialBoxes()
        {
            VpnUserCredentials = StorageCore.Core.GetUserCredentialsAll();
            RaisePropertyChanged("VpnUserCredentials");
        }

        private void loadVpnConnections()
        {
            VpnList = new ObservableCollection<VpnBase>(_VpnMgr.GetVpnList());
            RaisePropertyChanged("VpnList");
        }

        private void loadWindowsVpnConnections()
        {
            var wVpn = new WindowsVPN();
            WindowsVpnList = new ObservableCollection<string>(wVpn.GetVpnConnections());
        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        /// <summary>
        /// Helper for Triggering PropertyChanged
        /// </summary>
        /// <param name="triggerControl">The Name of the Property to update</param>
        private void RaisePropertyChanged(string triggerControl)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(triggerControl));
            }
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Disposes all Data of this Tab
        /// </summary>
        public void Dispose()
        {
            _VpnList.Clear();
            _VpnMgr = null;
            SelectedVpn = null;
            VpnTypeList.Clear();
            VpnUserCredentials.Clear();
            CiscoVpnSelectedCredential = null;
            WindowsVpnList.Clear();
        }
        #endregion

    }
}
