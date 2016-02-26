using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using beRemote.Core;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Classes;

namespace beRemote.GUI.Tabs.ManageConnection
{
    /// <summary>
    /// Interaction logic for ContentTabAbout.xaml
    /// </summary>
    public partial class TabManageConnection
    {
        private long _EditConnectionSettingId;
        private long _EditConnectionId;
        private readonly string _PreselectedProtocol = "";
        private readonly long _PreselectedFolderId;

        private ConnectionHost _ConnectionHostCache = new ConnectionHost(); //Contains the currently visible ConnectionHost, that are not saved.
        private Dictionary<long, ConnectionProtocol> _ConnectionProtocolCache = new Dictionary<long, ConnectionProtocol>(); //Contains the currently visible ConnectionProtocol, that are not saved.
        private readonly Dictionary<long, Dictionary<string, object>> _ConnectionOptionCache = new Dictionary<long, Dictionary<string, object>>(); //Contraint the options for the ConnectionProtocols
        

        public TabManageConnection(object parameter)
        {
            InitializeComponent();
            
            if (parameter != null)
            {
                if (parameter.GetType() == typeof(object[])) //Preselected Connection (Edit-Dialog)
                {
                    if (((object[])parameter).Length == 2) 
                    {
                        if (((object[])parameter)[0] is long &&
                            ((object[])parameter)[1] is long) //connectionid (long), protocolid (long) - Edit triggered on Protocol
                        {
                            _EditConnectionId = Convert.ToInt64(((object[])parameter)[0]);
                            _EditConnectionSettingId = Convert.ToInt64(((object[])parameter)[1]);
                        }
                        else if (((object[])parameter)[0] is long &&
                            ((object[])parameter)[1] is bool) //connectionid (long), onlyConnection (bool) - Add new protocol to existing connection
                        {
                            _EditConnectionId = Convert.ToInt64(((object[]) parameter)[0]);
                        }
                        else //protocoltype (string), folderId (long) - Edit triggered on Connection
                        {
                            _PreselectedProtocol = ((object[])parameter)[0].ToString();
                            _PreselectedFolderId = Convert.ToInt64(((object[])parameter)[1]);
                            //ccbFolder.SelectedValue = new Classes.ImagedConnectionTreeViewItem(Convert.ToInt64(((object[])parameter)[1]), Classes.ImagedConnectionTreeViewDatatype.Folder);
                        }
                    }
                    else if (((object[])parameter).Length == 3) //protocoltype (string), connectionId (long), protocolId (long) - Edit triggered on Protocol
                    {
                        _PreselectedProtocol = ((object[])parameter)[0].ToString();
                        _EditConnectionId = Convert.ToInt64(((object[])parameter)[1]);
                        _EditConnectionSettingId = Convert.ToInt64(((object[])parameter)[2]);

                        //loadConnection(Convert.ToInt64(((object[])parameter)[1]), Convert.ToInt64(((object[])parameter)[2]));
                    }
                }
                else if (parameter is string) //Preselected Protocol
                {
                    _PreselectedProtocol = parameter.ToString();
                }
                else if (parameter is long) //folderId
                {
                    _PreselectedFolderId = Convert.ToInt64(parameter);
                    //ccbFolder.SelectedValue = new Classes.ImagedConnectionTreeViewItem(Convert.ToInt64(parameter), Classes.ImagedConnectionTreeViewDatatype.Folder);
                }
                else
                {
                    throw new Exception("Unknown parameter for ContentTabManageConnection");
                }
            }
        }

        /// <summary>
        /// Load a connection for Edit-Mode
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="connectionSettingId"></param>
        public void loadConnection(long connectionId, long connectionSettingId)
        {
            _EditConnectionId = connectionId;
            _EditConnectionSettingId = connectionSettingId;

            _ConnectionHostCache = StorageCore.Core.GetConnection(connectionId);
            _ConnectionProtocolCache = StorageCore.Core.GetConnectionSettings(connectionId).ToDictionary(p => p.getId());
            _LastSelectedExistingProtocol = 0;

            if (_EditConnectionSettingId != 0 && _ConnectionOptionCache.Count == 0) //Only do this, if a Setting (=Protocol) is preselected and it is the first load
            {
                if (_ConnectionOptionCache.ContainsKey(connectionSettingId) == false)
                {
                    _ConnectionOptionCache.Add(connectionSettingId, new Dictionary<string, object>());
                }

                foreach (var cpo in StorageCore.Core.GetConnectionOptions(connectionSettingId))
                {
                    _ConnectionOptionCache[connectionSettingId].Add(cpo.getSettingname(), cpo.getSettingvalue());
                }
            }

            // ## Load Connectiondata ##
            //Hostinformation
            if (connectionId != 0) lblHostId.Content = "Hostinformation (ID: " + connectionId + ")";
            txtName.Text = _ConnectionHostCache.Name;
            txtHostname.Text = _ConnectionHostCache.Host;
            txtDescription.Text = _ConnectionHostCache.Description;
            ccbFolder.SelectedValue = new ImagedConnectionTreeViewItem(_ConnectionHostCache.Folder, ImagedConnectionTreeViewDatatype.Folder);
            chkPublic.IsChecked = _ConnectionHostCache.IsPublic;
            for (int i = 0; i < cmbOperatingSystem.Items.Count; i++)
            {
                if ((int)((ComboBoxItem)cmbOperatingSystem.Items[i]).Tag == _ConnectionHostCache.OS)
                {
                    cmbOperatingSystem.SelectedIndex = i;
                    break;
                }
            }

            //Protocolinformation
            foreach (ConnectionProtocol cp in _ConnectionProtocolCache.Values)
            {
                if (Kernel.GetAvailableProtocols().ContainsKey(cp.Protocol))
                {
                    var cmbi = new ComboBoxItem();
                    cmbi.Tag = cp.getId();
                    cmbi.Content = Kernel.GetAvailableProtocols()[cp.Protocol].GetProtocolName();

                    cmbExistingProtocols.Items.Add(cmbi);
                }
            }

            if (cmbExistingProtocols.Items.Count > 0)            
                cmbExistingProtocols.SelectedIndex = 0;
            

            //Select the selected Protocol, if a protocol should be edited
            if (_EditConnectionSettingId != 0)
            {
                string protocolType = StorageCore.Core.GetConnectionSetting(_EditConnectionSettingId).Protocol;
                if (protocolType != null)
                {
                    for (int i = 0; i < cmbExistingProtocols.Items.Count; i++)
                    {
                        if (((ComboBoxItem)cmbExistingProtocols.Items[i]).Tag.ToString() == protocolType)
                        {
                            cmbExistingProtocols.SelectedIndex = i;
                            break;
                        }
                    }
                }
                cmbVpn.SelectedValue = _ConnectionHostCache.Vpn;
            }
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkCredentials.IsChecked != null && (chkCredentials.IsChecked.Value && cmbCredentials.SelectedValue == null))
            {
                MessageBox.Show("Please select a credential or disable the credential-setting", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ccbFolder.SelectedValue.ID == 0)
            {
                MessageBox.Show("Please select a valid folder before saving", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            saveProperties(); //Save the current Propertygrid

            // ##### Save the cache #####

            // ## Save Hostcache ##
            long newId = 0;
            if (_EditConnectionId == 0) //New Host
                //Get the added ID by adding the connection to use it for handling the Protocol- and Option-Cache
                newId = StorageCore.Core.AddConnection(
                    txtHostname.Text, 
                    txtName.Text, 
                    txtDescription.Text, 
                    (int)((ComboBoxItem)cmbOperatingSystem.SelectedItem).Tag, 
                    ccbFolder.SelectedValue.ID, 
                    StorageCore.Core.GetUserId(), 
                    chkPublic.IsChecked != null && chkPublic.IsChecked.Value, 
                    (int)cmbVpn.SelectedValue);
            else //Existing Host
            {
                StorageCore.Core.ModifyConnection(
                    _EditConnectionId, 
                    txtHostname.Text, 
                    txtName.Text, 
                    txtDescription.Text, 
                    (int)((ComboBoxItem)cmbOperatingSystem.SelectedItem).Tag, 
                    StorageCore.Core.GetUserId(), 
                    chkPublic.IsChecked != null && chkPublic.IsChecked.Value, 
                    (int)cmbVpn.SelectedValue);
                StorageCore.Core.ModifyConnection(_EditConnectionId, ccbFolder.SelectedValue.ID);
            }

            #region Save
            var newProtocolId = new Dictionary<long, long>(); //firstlong = tempID, second = new "official" ID

            // ## Save Protocolcache ##
            if (_EditConnectionId == 0) //New Settings
            {
                foreach (ConnectionProtocol conProt in _ConnectionProtocolCache.Values)
                {
                    newProtocolId.Add(conProt.Id, StorageCore.Core.AddConnectionSetting(newId, conProt.Protocol, conProt.Port, conProt.UserCredentialId));
                }
            }
            else //Existing Settings
            {
                foreach (ConnectionProtocol conProt in _ConnectionProtocolCache.Values)
                {
                    if (conProt.Id > 0) //Existing Protocol
                        StorageCore.Core.ModifyConnectionSetting(conProt.Id, conProt.Port, conProt.UserCredentialId);
                    else //New Protocol
                        StorageCore.Core.AddConnectionSetting(_EditConnectionId, conProt.Protocol, conProt.Port, conProt.UserCredentialId);
                }
            }

            // ## Save Optioncache ##
            if (_EditConnectionId == 0) //New Options
            {
                //          ProtocolID v      Settingname v      v Settingvalue
                foreach (KeyValuePair<long, Dictionary<string, object>> kvp in _ConnectionOptionCache)
                {
                    foreach (KeyValuePair<string, object> kvp2 in kvp.Value)
                    {
                        StorageCore.Core.ModifyConnectionOption(kvp2.Value, kvp2.Key, newProtocolId[kvp.Key]);
                    }
                }
            }
            else //Existing Options
            {
                loadConnection(_EditConnectionId, _EditConnectionSettingId);

                foreach (var kvp in _ConnectionOptionCache)
                {
                    foreach (var kvp2 in kvp.Value)
                    {
                        StorageCore.Core.ModifyConnectionOption(kvp2.Value, kvp2.Key, kvp.Key);
                    }
                }
            }

            RefreshConnectionList();
            CloseTab();
            #endregion


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            #region Load Controlcontent
            //Fill Protocol-List
            foreach (var protocol in Kernel.GetAvailableProtocols().Values)
            {
                var cmbI = new ComboBoxItem();

                cmbI.Content = protocol.GetProtocolName(); //Set the displayed text                
                cmbI.Tag = protocol.GetProtocolIdentifer(); //Set the identification-Tag
                
                cmbProtocol.Items.Add(cmbI);
            }            

            //Fill Operating System-List and select unknown, if it is not edit-mode
            foreach (var Os in StorageCore.Core.GetOperatingSystemList())
            {
                var cmbI = new ComboBoxItem();

                cmbI.Content = Os.DisplayText;
                cmbI.Tag = Os.getId();

                cmbOperatingSystem.Items.Add(cmbI);
            }            
            
            //Select Default Folder
            if (ccbFolder.SelectedValue == null)
            {
                if (_PreselectedFolderId == 0)
                {
                    long defaultFolder = StorageCore.Core.GetUserDefaultFolder();

                    //Defaultfolder exists? (Maybe it was deleted)
                    if (StorageCore.Core.GetFolder(defaultFolder) == null)
                    {
                        //Select first found Folder as the default folder, if there are folders
                        if (StorageCore.Core.GetFolders().Count > 0)
                            defaultFolder = StorageCore.Core.GetFolders()[0].Id;
                        else
                            defaultFolder = 0;
                    }
                        
                    //Is the folder Public?
                    if (defaultFolder != 0 && StorageCore.Core.GetFolder(defaultFolder).IsPublic)
                        chkPublic.IsChecked = true;

                    //Select folder, if there is a folder
                    if (defaultFolder != 0)
                        ccbFolder.SelectedValue = new ImagedConnectionTreeViewItem(defaultFolder, ImagedConnectionTreeViewDatatype.Folder);
                }
                else
                {
                    if (StorageCore.Core.GetFolder(_PreselectedFolderId).IsPublic)
                        chkPublic.IsChecked = true;

                    ccbFolder.SelectedValue = new ImagedConnectionTreeViewItem(_PreselectedFolderId, ImagedConnectionTreeViewDatatype.Folder);
                }
            }

            //Load Credentials and set for Credentials-Combobox
            List<UserCredential> userCreds = StorageCore.Core.GetUserCredentialsAll();
            cmbCredentials.DisplayMemberPath = "Description";
            cmbCredentials.SelectedValuePath = "Id";
            cmbCredentials.ItemsSource = userCreds;

            //Set Credentials for the Property-Grid
            pogProtocolSettings.Credentials = userCreds;
                        
            //Load VPNs
            Dictionary<int, string> conVpnList = StorageCore.Core.GetUserVpnConnectionsShort();
            conVpnList.Add(0, "Undefined");
            cmbVpn.ItemsSource = conVpnList;
            cmbVpn.SelectedValue = 0;

            txtHostname.Focus();
            #endregion
                        
            //Select "unknown"
            cmbOperatingSystem.SelectedIndex = 0;

            #region Select default-Procotol or preselected Protocol; if preselected protocol not available, select default
            //Select Preselected Protocol, if there is a preselection
            if (_PreselectedProtocol != "")
            {                
                foreach (ComboBoxItem cmbi in cmbProtocol.Items)
                {
                    if (cmbi.Tag.ToString() == _PreselectedProtocol)
                    {
                        cmbProtocol.SelectedItem = cmbi;
                        break;
                    }
                }
            }

            //If no Item is selected
            if (cmbProtocol.SelectedItem == null)
            {
                string userDefProt = StorageCore.Core.GetUserDefaultProtocol();
                foreach (ComboBoxItem cmbi in cmbProtocol.Items)
                {
                    if (cmbi.Tag.ToString() == userDefProt)
                    {
                        cmbProtocol.SelectedItem = cmbi;
                        break;
                    }
                }
            }
            else if (_PreselectedProtocol != "")
            {
                btnAdd_Click(this, new RoutedEventArgs());
            }
            #endregion

            //Load a connection, if a connection should be edited
            if (_EditConnectionId != 0)
            {
                loadConnection(_EditConnectionId, _EditConnectionSettingId);
            }

            //Prevent Hostname-Complete, if there is already a Defined hostname
            if (txtDescription.Text.Length != 0)
            {
                _AutoCompleteHostname = false;
            }
        }
        
        private void chkCredentials_Checked(object sender, RoutedEventArgs e)
        {
            if (chkCredentials.IsChecked != null) 
                btnAddCredentials.IsEnabled = cmbCredentials.IsEnabled = chkCredentials.IsChecked.Value;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseTab();
        }

        void ccbFolder_Expanded(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(ccbFolder, Int32.MaxValue);
        }
        
        private void ccbFolder_MenuContract(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(ccbFolder, Int32.MaxValue -1);
        }

        private long _LastSelectedExistingProtocol;
       
        private void cmbExistingProtocols_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbExistingProtocols.Items.Count == 0) //Prevent calling, if no Item exists and/or a kind of Preview-Calling happens
                return;

            saveProperties();

            //Load Protocolsettings
            if (cmbExistingProtocols.SelectedItem != null)
            {
                var selectedProtocolId = Convert.ToInt64(((ComboBoxItem)cmbExistingProtocols.SelectedItem).Tag);

                //set the Port in the Port-Textbox
                nudPort.Value = _ConnectionProtocolCache[selectedProtocolId].Port;

                //Set the Credentials                
                if (_ConnectionProtocolCache[selectedProtocolId].UserCredentialId != 0) //Check if there are settet credentials
                {
                    chkCredentials.IsChecked = true;
                    cmbCredentials.SelectedValue = _ConnectionProtocolCache[selectedProtocolId].UserCredentialId;
                }

                //Load the Propertygrid
                //Load the Default Settings
                var protocolOptions = Kernel.GetAvailableProtocols()[_ConnectionProtocolCache[(long)((ComboBoxItem)cmbExistingProtocols.SelectedItem).Tag].Protocol].GetProtocolSettings();       
                pogProtocolSettings.SetProtocolOptions(protocolOptions);

                //If Settings existing, load them:
                if (_ConnectionOptionCache.ContainsKey(selectedProtocolId))
                { 
                    foreach(KeyValuePair<string, object> kvp in _ConnectionOptionCache[selectedProtocolId])
                    {
                        pogProtocolSettings.SetSingleValue(kvp.Key, kvp.Value);    
                    }                    
                }
                
                //Resize the Control               
                gbProtocols.Height = pogProtocolSettings.Margin.Top + pogProtocolSettings.Height + pogProtocolSettings.Margin.Bottom + 6 + (gbProtocols.Height - grdProtocolsettings.Height);
            }
            
            if (cmbExistingProtocols.SelectedItem != null)
                _LastSelectedExistingProtocol = (long)((ComboBoxItem)cmbExistingProtocols.SelectedItem).Tag;
        }

        /// <summary>
        /// Save the Propertygrid
        /// </summary>
        private void saveProperties()
        {
            //Save set Settings
            if (_LastSelectedExistingProtocol != 0)
            {
                if (chkCredentials.IsChecked == true)
                    _ConnectionProtocolCache[_LastSelectedExistingProtocol].UserCredentialId = Convert.ToInt32(cmbCredentials.SelectedValue);
                else
                    _ConnectionProtocolCache[_LastSelectedExistingProtocol].UserCredentialId = 0;

                _ConnectionProtocolCache[_LastSelectedExistingProtocol]. Port = nudPort.Value;

                //Save the Propertygrid
                if (!_ConnectionOptionCache.ContainsKey(_LastSelectedExistingProtocol))
                    _ConnectionOptionCache.Add(_LastSelectedExistingProtocol, new Dictionary<string, object>());

                //_ConnectionOptionCache[_LastSelectedExistingProtocol] = pogProtocolSettings.GetProtocolOptions(); 

                Dictionary<string, object> gridSettings = pogProtocolSettings.GetProtocolOptions();
                List<string> gridSettingsKeys = gridSettings.Keys.ToList();
                List<object> gridSettingsValues = gridSettings.Values.ToList();
                for (int i = 0; i < gridSettingsKeys.Count; i++)
                {
                    if (!_ConnectionOptionCache[_LastSelectedExistingProtocol].ContainsKey(gridSettingsKeys[i]))
                        _ConnectionOptionCache[_LastSelectedExistingProtocol].Add(gridSettingsKeys[i], gridSettingsValues[i]);
                    else
                        _ConnectionOptionCache[_LastSelectedExistingProtocol][gridSettingsKeys[i]] = gridSettingsValues[i];
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbProtocol.SelectedValue == null)
                return;

            //Adds a new Protocol with a negative ID as identificator
            var rnd = new Random(DateTime.Now.Millisecond);
            var rndId = rnd.Next(-32000,-1);

            Logger.Log(LogEntryType.Verbose, "Adding new protocol to this connection...");
            Logger.Log(LogEntryType.Verbose, "Temporary ID: " + rndId);
            Logger.Log(LogEntryType.Verbose, "Getting new Protocol for " + ((ComboBoxItem)cmbProtocol.SelectedValue).Tag + " and port " + Kernel.GetAvailableProtocols()[((ComboBoxItem)cmbProtocol.SelectedValue).Tag.ToString()].GetDefaultProtocolPort());
            Logger.Log(LogEntryType.Verbose, "ProtocolCacheLenght: " + (_ConnectionProtocolCache == null ? "NULL" : _ConnectionProtocolCache.Count.ToString()));

            var conProtNew = 
                new ConnectionProtocol(rndId, 0, ((ComboBoxItem)cmbProtocol.SelectedValue).Tag.ToString(), 
                    Kernel.GetAvailableProtocols()[((ComboBoxItem)cmbProtocol.SelectedValue).Tag.ToString()].GetDefaultProtocolPort());
            _ConnectionProtocolCache.Add(conProtNew.getId(), conProtNew);

            Logger.Log(LogEntryType.Verbose, "Adding Connectionoptions to propertygrid....");

            //Add connectionoptions
            SortedList<string, ProtocolSetting> protocolOptions = Kernel.GetAvailableProtocols()[((ComboBoxItem)(cmbProtocol.SelectedItem)).Tag.ToString()].GetProtocolSettings();
            _ConnectionOptionCache.Add(rndId, new Dictionary<string,object>());
            foreach (KeyValuePair<string, ProtocolSetting> kvp in protocolOptions)
            {
                _ConnectionOptionCache[rndId].Add(kvp.Key, kvp.Value.Value.Value);
            }

            Logger.Log(LogEntryType.Verbose, "Reloading existing protocols....");
            reloadExistingProtocols();

            Logger.Log(LogEntryType.Verbose, "Selecting added protocol");
            cmbExistingProtocols.SelectedIndex = cmbExistingProtocols.Items.Count - 1;
        }

        private void reloadExistingProtocols()
        {
            cmbExistingProtocols.Items.Clear();

            foreach (var conProt in _ConnectionProtocolCache.Values)
            {
                if (Kernel.GetAvailableProtocols().ContainsKey(conProt.Protocol)) //Ignore non available protocols
                {
                    var cmbI = new ComboBoxItem();

                    cmbI.Content = Kernel.GetAvailableProtocols()[conProt.Protocol].GetProtocolName(); //Set the displayed text                
                    cmbI.Tag = conProt.getId(); //Set the identification-Tag

                    cmbExistingProtocols.Items.Add(cmbI);
                }
            }
        }

        private void btnAddCredentials_Click(object sender, RoutedEventArgs e)
        {
            if (txtCredDesc.Text != "")
            {
                var newCreds = StorageCore.Core.AddUserCredentials(
                    txtCredUser.Text,
                    Helper.EncryptStringToBytes(pwCredPass.SecurePassword, Helper.GetHash1(StorageCore.Core.GetUserSalt1()), Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid()), StorageCore.Core.GetUserSalt3()),
                    txtCredDom.Text, 
                    txtCredDesc.Text);

                //Reload Credentials
                var userCreds = StorageCore.Core.GetUserCredentialsAll();
                cmbCredentials.DisplayMemberPath = "Description";
                cmbCredentials.SelectedValuePath = "Id";
                cmbCredentials.ItemsSource = userCreds;

                foreach (var credItem in cmbCredentials.Items)
                {
                    if (((UserCredential)credItem).Id == newCreds)
                    {
                        cmbCredentials.SelectedValue = newCreds;
                        break;
                    }
                }
                cmbCredentials.Focus();

                txtCredUser.Clear();
                txtCredDom.Clear();
                txtCredDesc.Clear();
                pwCredPass.Clear();
                txtCredDesc.Background = Brushes.White;

                spCredentials.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtCredDesc.Background = Brushes.LightPink;
                ((ToolTip)txtCredDesc.ToolTip).IsOpen = true;
            }
        }

        private void btnAddCredentials_Click_1(object sender, RoutedEventArgs e)
        {
            //btnAddCredentials.ContextMenu.PlacementTarget = btnAddCredentials;
            //btnAddCredentials.ContextMenu.IsOpen = true;

            spCredentials.Visibility = Visibility.Visible;
        }

        private bool _AutoCompleteHostname = true; // Is the Hostname autocompleted with the given name? True until manual change of txtHostname
        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            _AutoCompleteHostname = false;
        }

        private void txtHostname_KeyUp(object sender, KeyEventArgs e)
        {
            if (_AutoCompleteHostname)
                txtName.Text = txtHostname.Text;
        }

        public override void Dispose()
        {
            base.Dispose();
            _ConnectionHostCache = null;
            _ConnectionOptionCache.Clear();
            _ConnectionProtocolCache.Clear();

        }
    }
}
