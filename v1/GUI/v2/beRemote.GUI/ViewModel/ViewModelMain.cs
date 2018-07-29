using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using beRemote.Core;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.Definitions.EventArgs;
using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Kernel;
using beRemote.Core.KernelHelper;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.Services;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.Plugins;
using beRemote.GUI.ViewModel.Command;
using beRemote.GUI.ViewModel.EventArg;
using beRemote.GUI.Notification;
using beRemote.GUI.ViewModel.Worker;
using Xceed.Wpf.AvalonDock;
using ConnectEventArgs = beRemote.GUI.ViewModel.EventArg.ConnectEventArgs;
using Protocol = beRemote.Core.ProtocolSystem.ProtocolBase.Protocol;
using beRemote.GUI.Tabs.ManageVpn.ViewModel.Commands;

namespace beRemote.GUI.ViewModel
{
    public class ViewModelMain : DependencyObject, INotifyPropertyChanged
    {
        #region Command Implementation

        public CmdAddConnectionImpl CmdAddConnection { get; set; }
        public CmdAddFavoritesItemImpl CmdAddFavoritesItem { get; set; }
        public CmdAddFolderImpl CmdAddFolder { get; set; }
        public CmdConnectWithoutCredentialsImpl CmdConnectWithoutCredentials { get; set; }
        public CmdConTreeDoubleClickItemImpl CmdConTreeDoubleClickItem { get; set; }
        public CmdConTreeDragDropMovedImpl CmdConTreeDragDropMoved { get; set; }
        public CmdCreateAccountImpl CmdCreateAccount { get; set; }
        public CmdDeleteItemImpl CmdDeleteItem { get; set; }
        public CmdEditItemImpl CmdEditItem { get; set; }
        public CmdLoginCancelImpl CmdLoginCancel { get; set; }
        public CmdLoginImpl CmdLogin { get; set; }
        public CmdSetDefaultFolderImpl CmdSetDefaultFolder { get; set; }
        public CmdSetDefaultProtocolImpl CmdSetDefaultProtocol { get; set; }
        public CmdSortItemDownImpl CmdSortItemDown { get; set; }
        public CmdSortItemUpImpl CmdSortItemUp { get; set; }

        public CmdMainWindowLoadedImpl CmdMainWindowLoaded { get; set; }
        public CmdMainWindowClosingImpl CmdMainWindowClosing { get; set; }
        public CmdMainWindowCloseImpl CmdMainWindowClose { get; set; }
        public CmdUserProfileOpenImpl CmdUserProfileOpen { get; set; }
        public CmdQuickConnectImpl CmdQuickConnect { get; set; }
        public CmdConnectionFilterChangedImpl CmdConnectionFilterChanged { get; set; }
        public CmdFirstRunWizardFinishedImpl CmdFirstRunWizardFinished { get; set; }
        public CmdWizardMessageImpl CmdWizardMessage { get; set; }

        public CmdAddFilterImpl CmdAddFilter { get; set; }
        public CmdFavoriteClickedImpl CmdFavoriteClicked { get; set; }

        public CmdChPwCancelImpl CmdChPwCancel { get; set; }
        public CmdChPwSaveImpl CmdChPwSave { get; set; }

        public CmdCreateAccountSaveImpl CmdCreateAccountSave { get; set; }
        public CmdCreateAccountCancelImpl CmdCreateAccountCancel { get; set; }

        public CmdTabCloseImpl CmdTabClose { get; set; }

        public CmdRibBtnClickImpl CmdRibBtnClick { get; set; }
        public CmdPluginButtonImpl CmdPluginButton { get; set; }

        public CmdAddVpnImpl CmdAddVpn { get; set; }
        public CmdRemoveVpnImpl CmdRemoveVpn { get; set; }
        public CmdTestVpnImpl CmdTestVpn { get; set; }
        public CmdSaveVpnImpl CmdSaveVpn { get; set; }

        #endregion

        #region Constructor

        public ViewModelMain()
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

            #region Initialize Command Bindings

            #region TreeView

            CmdAddConnection = new CmdAddConnectionImpl();
            CmdAddFavoritesItem = new CmdAddFavoritesItemImpl();
            CmdAddFolder = new CmdAddFolderImpl();
            CmdConnectWithoutCredentials = new CmdConnectWithoutCredentialsImpl();
            CmdConTreeDoubleClickItem = new CmdConTreeDoubleClickItemImpl();
            CmdConTreeDragDropMoved = new CmdConTreeDragDropMovedImpl();
            CmdDeleteItem = new CmdDeleteItemImpl();
            CmdEditItem = new CmdEditItemImpl();
            CmdSetDefaultFolder = new CmdSetDefaultFolderImpl();
            CmdSetDefaultProtocol = new CmdSetDefaultProtocolImpl();
            CmdSortItemDown = new CmdSortItemDownImpl();
            CmdSortItemUp = new CmdSortItemUpImpl();

            CmdAddConnection.ConnectionAdd += CmdAddTab;
            CmdConTreeDoubleClickItem.Connect += CmdConnect;
            CmdDeleteItem.ItemRemove += CmdItemRemove;
            CmdAddFavoritesItem.ReloadFavorites += CmdReloadFavorites;
            CmdAddFolder.AddTab += CmdAddTab;
            CmdConnectWithoutCredentials.Connect += CmdConnect;
            CmdConTreeDragDropMoved.ReloadConnectionList += CmdReloadConnectionList;
            CmdEditItem.AddTab += CmdAddTab;
            CmdSortItemDown.ReloadConnectionList += CmdReloadConnectionList;
            CmdSortItemUp.ReloadConnectionList += CmdReloadConnectionList;
            #endregion

            #region Login

            CmdLoginCancel = new CmdLoginCancelImpl();
            CmdLogin = new CmdLoginImpl();
            CmdCreateAccount = new CmdCreateAccountImpl();

            CmdLogin.LoginClick += CmdLogin_LoginClick;
            CmdLoginCancel.LoginCancelClick += CmdLoginCancel_LoginCancelClick;
            CmdCreateAccount.Click += CmdCreateAccount_Click;

            #endregion

            #region MainWindow

            CmdMainWindowLoaded = new CmdMainWindowLoadedImpl();
            CmdMainWindowClosing = new CmdMainWindowClosingImpl();
            CmdMainWindowClose = new CmdMainWindowCloseImpl();

            CmdUserProfileOpen = new CmdUserProfileOpenImpl();
            CmdQuickConnect = new CmdQuickConnectImpl();
            CmdConnectionFilterChanged = new CmdConnectionFilterChangedImpl();

            CmdAddFilter = new CmdAddFilterImpl();
            CmdFavoriteClicked = new CmdFavoriteClickedImpl();

            CmdFirstRunWizardFinished = new CmdFirstRunWizardFinishedImpl();
            CmdWizardMessage = new CmdWizardMessageImpl();

            CmdMainWindowLoaded.ApplicationLoaded += CmdMainWindowLoaded_ApplicationLoaded;
            CmdMainWindowClosing.ApplicationClosing += CmdMainWindowClosing_ApplicationClosing;
            CmdMainWindowClose.ApplicationClose += CmdMainWindowClose_ApplicationClose;

            CmdQuickConnect.Connect += CmdConnect;
            CmdQuickConnect.ReloadConnectionList += CmdReloadConnectionList;
            CmdConnectionFilterChanged.ApplyFilter += CmdApplyFilter;

            CmdAddFilter.FilterAdd += CmdAddFilter_FilterAdd;
            CmdFavoriteClicked.Connect += CmdConnect;

            CmdFirstRunWizardFinished.WizardFinished += CmdFirstRunWizardFinished_WizardFinished;
            CmdWizardMessage.WizardMessage += CmdWizardMessage_WizardMessage;
            #endregion

            #region Tabs

            CmdTabClose = new CmdTabCloseImpl();
            CmdTabClose.CloseTab += TabContent_Close;

            #endregion

            #region Change Password

            CmdChPwCancel = new CmdChPwCancelImpl();
            CmdChPwSave = new CmdChPwSaveImpl();
            #endregion

            #region Create Account

            CmdCreateAccountSave = new CmdCreateAccountSaveImpl();
            CmdCreateAccountCancel = new CmdCreateAccountCancelImpl();

            CmdCreateAccountSave.SaveClick += CmdCreateAccountSave_SaveClick;
            CmdCreateAccountCancel.CancelClick += CmdCreateAccountCancel_CancelClick;

            #endregion

            #region Ribbon-Buttons

            CmdRibBtnClick = new CmdRibBtnClickImpl();
            CmdPluginButton = new CmdPluginButtonImpl();

            CmdRibBtnClick.AddTab += CmdAddTab;
            CmdPluginButton.AddTab += CmdAddTab;


            CmdAddVpn = new CmdAddVpnImpl();
            CmdRemoveVpn = new CmdRemoveVpnImpl();
            CmdTestVpn = new CmdTestVpnImpl();
            CmdSaveVpn = new CmdSaveVpnImpl();

            CmdAddVpn.VpnAdd += CmdAddVpn_VpnAdd;
            CmdRemoveVpn.VpnRemove += CmdRemoveVpn_VpnRemove;
            CmdTestVpn.VpnTest += CmdTestVpn_VpnTest;
            CmdSaveVpn.VpnSave += CmdSaveVpn_VpnSave;
            #endregion

            #endregion

            #region Abo Login-Class-events

            _Login.LoginFailed += LoginLoginFailed;
            _Login.LoginProcessing += LoginLoginProcessing;
            _Login.ShowMessage += _Login_ShowMessage;
            _Login.UserNotExists += LoginUserNotExists;
            _Login.LoginSuccessful += LoginLoginSuccessful;
            #endregion

            #region Kernel-Events

            Kernel.OnNewConnectionTriggered += Kernel_OnNewConnectionTriggered;

            #endregion

        }

        void CmdSaveVpn_VpnSave(object sender, RoutedEventArgs e)
        {
            Worker.Vpn.SaveVpn(ContentTabs);
        }

        void CmdTestVpn_VpnTest(object sender, RoutedEventArgs e)
        {
            Worker.Vpn.TestVpn(ContentTabs);
        }

        void CmdRemoveVpn_VpnRemove(object sender, RoutedEventArgs e)
        {
            Worker.Vpn.RemoveVpn(ContentTabs);
        }

        void CmdAddVpn_VpnAdd(object sender, VpnAddEventArgs e)
        {
            Worker.Vpn.AddVpn(ContentTabs, e.VpnType);
        }

        /// <summary>
        /// Exectuted, if a wizard wants to show a Message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CmdWizardMessage_WizardMessage(object sender, WizardMessageEventArgs e)
        {
            MessageBox.Show(e.Message, e.Title, MessageBoxButton.OK, e.MessageImage);
        }

        /// <summary>
        /// Exeuted, if the first-run-wizard finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CmdFirstRunWizardFinished_WizardFinished(object sender, WizardResultEventArgs e)
        {
            if (e.Result)
            {
                ShowWizardFirstRun = false;
            }
            else
            {
                MessageBox.Show(e.ResultMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Executed by the Ribbon, if a Filter should be added to the ManageFilter-Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CmdAddFilter_FilterAdd(object sender, FilterAddEventArgs e)
        {
            ConnectionFilter.AddFilter(ContentTabs, e.FilterType);
        }

        /// <summary>
        /// Executed, if a Command triggers a Reload or Update of the Connectionlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdReloadConnectionList(object sender, ReloadConnectionListEventArgs e)
        {
            if (e.ReloadFromDatabase) //Force Resync
                LoadConnectionListAsync(0);
            else //Apply changes
            {
                LoadConnectionListAsync(0);
                //throw new NotImplementedException("Not implementent until a rebuild of the ConnectionTree");
            }
        }

        /// <summary>
        /// Executed, if the Favorite-Collection in the Ribbonbar changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdReloadFavorites(object sender, ReloadFavoritesEventArgs e)
        {
            if (e.ReloadFromDatabase) //A sync with DB is forced
                LoadFavoritesButtons(StorageCore.Core.GetUserVisuals().Favorites);
            else //Just update the local information
                LoadFavoritesButtons(e.NewFavorites);
        }

        /// <summary>
        /// Executed, if the Selected Filter changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdApplyFilter(object sender, ConnectionFilterChangedEventArgs e)
        {
            LoadConnectionListAsync(e.NewFilter.Id);
        }

        #endregion

        #region private Methods

        /// <summary>
        /// Sets the Window-Position and size
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        private void SetWindowPosition(int width, int height, int left, int top)
        {
            var evArgs = new WindowPropertiesChangedEventArgs(width, height, left, top);
            OnWindowPropertiesChanged(evArgs);

            //Dataholding for the Backend
            UiWindowSizeWidth = width;
            UiWindowSizeHeight = height;
            UiWindowPositionLeft = left;
            UiWindowPositionTop = top;
        }

        /// <summary>
        /// Loads the Filtersets Asynchronious
        /// </summary>
        private async void LoadConnectionFilterSetsAsync()
        {
            var myTask = Task.Run(
                () =>
                {
                    while (Kernel.IsKernelReady() == false)
                        System.Threading.Thread.Sleep(100);

                    return StorageCore.Core.GetFilterSetBasics();
                });

            ConnectionFilterList = await myTask;
        }

        /// <summary>
        /// Load the Protocol-Buttons, shown in the Add-Button-Menu
        /// </summary>
        private void LoadProtocolButtons()
        {
            while (Kernel.IsKernelReady() == false)
                System.Threading.Thread.Sleep(100);

            RaisePropertyChanged("ProtocolList");
        }

        /// <summary>
        /// Triggers a Refresh of the Plugin-Button-List
        /// </summary>
        private void LoadPluginButtons()
        {
            while (Kernel.IsKernelReady() == false)
                System.Threading.Thread.Sleep(100);

            RaisePropertyChanged("ToolsAndFeatures");
        }

        /// <summary>
        /// Loads the ConnectionList
        /// </summary>
        private async void LoadConnectionListAsync(long filterSetId)
        {
            var myTask = Task.Run(
                () =>
                {
                    return (_ConnectionListWorker.GetConnectionList(filterSetId));
                });

            ConnectionViewItems = await myTask;
        }

        /// <summary>
        /// Loads the History behind the ConnectionList
        /// </summary>
        private async void LoadHistoryAsync()
        {
            var myTask = Task.Run(
                () =>
                {
                    return (ConnectionHistoryList.GetHistoryList(25));
                });

            ConnectionHistory = await myTask;
        }

        /// <summary>
        /// Loads the History behind the ConnectionList
        /// </summary>
        private async void LoadWatermarkAsync()
        {
            var myTask = Task.Run(
                () =>
                {
                    return (SaveLoad.LoadRibbonWatermark());
                });

            var watermark = await myTask;
            if (watermark != null)
            {
                var opacity = StorageCore.Core.GetSetting("ribbonimageopacity");
                if (opacity != "") //Older Version compability; only exists since 0.0.3
                    RibbonWatermarkOpacity = Convert.ToDouble(opacity) / 100;
                RibbonWatermark = watermark;
            }
        }

        /// <summary>
        /// Loads the Favorite-Buttons
        /// </summary>
        /// <param name="favoriteItems"></param>
        private async void LoadFavoritesButtons(string favoriteItems)
        {
            var myTask = Task.Run(
                () =>
                {
                    return (Favorite.GetFavorites(favoriteItems));
                });

            FavoriteButtons = await myTask;
        }

        /// <summary>
        /// Adds a new Tab to the ContentTab-Collection
        /// </summary>
        /// <param name="content">The Usercontrol, that is shown in the content-area; to use all Features, use the TabBase-Class</param>
        /// <param name="contentId">The ContentID of the tab. If no GUID is set, a new one will be generated</param>
        private TabBase AddTab(object content, Guid contentId)
        {
            TabBase tabContent = null;
            if (content is TabBase)
                tabContent = content as TabBase;

            //Check if there is a valid contentId. If not, generate a new one
            if (contentId == Guid.Empty)
                contentId = new Guid();

            //if the tab is not of type tabBase, add it as a UserControl without TabBase-Features
            if (tabContent == null)
            {
                //Set basic information
                var vmtp = new ViewModelTabBase();
                vmtp.Content = (UserControl)content;
                vmtp.ContentId = contentId;
                vmtp.IsSelected = true;
                vmtp.IsActive = true;

                Logger.Log(LogEntryType.Info, "A Tab not of type TabBase was added. Not all functions are available. (i.e. Tabtitle, Tabicon, Close-Function etc.)");

                ContentTabs.Add(vmtp);
            }
            else // It is a Tab of type TabBase
            {
                //Only add tab, if the tab is allowed to open
                if ((tabContent).IsMultiTab == false &&
                    ((tabContent).IsMultiTab || TabExists(content.GetType())))
                    return null;

                //Set basic information
                var vmtp = new ViewModelTabBase();
                vmtp.Content = tabContent;
                vmtp.ContentId = contentId;
                vmtp.ToolTip = tabContent.TabToolTip;
                vmtp.IsSelected = true;
                vmtp.IsActive = true;
                vmtp.Title = tabContent.Header;
                vmtp.IconSource = tabContent.IconSource;

                //Add the CLose-Event
                ((TabBase)vmtp.Content).Close += TabContent_Close;
                ((TabBase)vmtp.Content).ConnectionListChanged += TabContent_ConnectionListChanged;
                ((TabBase)vmtp.Content).FavoritesChanged += TabContent_FavoritesChanged;
                ((TabBase)vmtp.Content).ContextRibbonVisibileChange += TabContent_ContextRibbonVisibileChange;
                ((TabBase)vmtp.Content).ConnectionFilterChanged += TabContent_ConnectionFilterChanged;

                //Add the Tab to the ContentTab-Collection
                ContentTabs.Add(vmtp);
                ActiveDocument = vmtp;
            }

            return tabContent;
        }

        void TabContent_ConnectionFilterChanged(object sender, RoutedEventArgs e)
        {
            LoadConnectionFilterSetsAsync();
        }

        /// <summary>
        /// Checks if a Tab of the given Type exists
        /// </summary>
        /// <param name="tabType">The Type of the tab</param>
        /// <returns></returns>
        private bool TabExists(Type tabType)
        {
            foreach (var aTab in ContentTabs)
            {
                if (aTab.Content.GetType() == tabType)
                    return (true);
            }
            return (false);
        }

        #endregion

        #region Command Eventtriggers
        /// <summary>
        /// Executed if the cancel-Button in the Login-Grid was pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdLoginCancel_LoginCancelClick(object sender, RoutedEventArgs e)
        {
            //Close this Window
            var Window = Application.Current.Windows[0];
            if (Window != null) Window.Close();
        }

        /// <summary>
        /// Executed, iif the login-Button was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdLogin_LoginClick(object sender, RoutedEventArgs e)
        {
            _Login.ExecuteLogin(LoginUsername, e.Source as SecureString);
        }

        /// <summary>
        /// Executed, after the Form was initialized (equivalent to "Loaded"-Event)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdMainWindowLoaded_ApplicationLoaded(object sender, MainWindowLoadedEventArgs e)
        {
            // Register KernelEvents
            Kernel.OnKernelReady += Kernel_OnKernelReady;
            Kernel.OnKernelFailed += Kernel_OnKernelFailed;

            // Register ServiceClient Events (in order to notify user about web communication by animating tray icon)
            AbstractBeRemoteServiceClient.OnRequestStarted += AbstractBeRemoteServiceClient_OnRequestStarted;
            AbstractBeRemoteServiceClient.OnRequestFinished += AbstractBeRemoteServiceClient_OnRequestFinished;

            //Create Required Resources Like Folders, Files and Registry-Entries
            var StartCheckResult = Startup.CheckResources();
            if (StartCheckResult.Item1 == false)
            {
                MessageBox.Show(StartCheckResult.Item2);
                Kernel.Shutdown();
                OnCloseApplication(new RoutedEventArgs());
                return;
            }

            //No Database exists
            if (StorageCore.Core.GetSetting("version") == "")
            {
                //Show Wizard
                ShowWizardFirstRun = true;
            }
        }

        void AbstractBeRemoteServiceClient_OnRequestFinished(AbstractBeRemoteServiceClient sender)
        {
            TrayIcon.TrayIconInstance.SetIconState(TrayIconState.LoggedIn);
        }

        void AbstractBeRemoteServiceClient_OnRequestStarted(AbstractBeRemoteServiceClient sender)
        {
            TrayIcon.TrayIconInstance.SetIconState(TrayIconState.AnimateIcon);
        }


        /// <summary>
        /// If the Create Account-Button was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateAccount = true;
            CreateAccountUsername = Environment.UserDomainName + "\\" + Environment.UserName;
            SetDefaultDisplayname();
        }

        /// <summary>
        /// Sets the Default Displayname, if it is nio
        /// </summary>
        private async void SetDefaultDisplayname()
        {
            var myTask = Task.Run(
                () =>
                {
                    try
                    {
                        var dispName = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
                        if (CreateAccountDisplayname == "")
                            CreateAccountDisplayname = dispName;
                    }
                    catch (UnauthorizedAccessException ea)
                    {
                        Logger.Log(LogEntryType.Info, "Cannot access DirectoryService-Information to get displayname. This usally happens, if you don't have access to it such as if you use a guest-account", ea);
                    }
                    catch (Exception ea)
                    {
                        Logger.Log(LogEntryType.Warning, "Error on getting DirectoryService-Information", ea);
                    }
                });
            
            //Wait for delete
            await myTask;
        }

        /// <summary>
        /// If th Cancel-Button in the account creation dialog clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdCreateAccountCancel_CancelClick(object sender, RoutedEventArgs e)
        {
            CreateAccountUsername = String.Empty;
            CreateAccountDisplayname = String.Empty;
            CreateAccountPassword1 = null;
            CreateAccountPassword2 = null;

            ShowCreateAccount = false;
        }

        /// <summary>
        /// If the Save-Button in the account creation dialog clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdCreateAccountSave_SaveClick(object sender, RoutedEventArgs e)
        {
            //If Account Creation was successful, hide the Dialog.
            if (_Login.CreateAccount(CreateAccountUsername, CreateAccountDisplayname, CreateAccountPassword1, CreateAccountPassword2))
                ShowCreateAccount = false;
        }

        /// <summary>
        /// Executed on MainWindow Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdMainWindowClosing_ApplicationClosing(object sender, MainWindowClosingEventArgs e)
        {
            //Only save information, if there is a logged in user
            if (StorageCore.Core.GetUserId() != 0)
            {
                //Save the Gridlayout
                SaveLoad.SaveSettingsDock(e.DockMgr);

                //Save the Quick Access Toolbar of the Ribbon
                //SaveLoad.SaveSettingsQatRibbon(QuickAccessButtons);

                //Delete Quick Connections (if activated)
                ConnectionManagement.RemoveQuickConnects(false);

                //Save expanded TreeView-Nodes
                SaveLoad.SaveSettingsTreeViewState(ConnectionViewItemsExpanded);

                //Save Window-State
                SaveLoad.SaveWindowState(MainWindowState);

                //Save Window Position
                if (MainWindowState != WindowState.Maximized)
                    SaveLoad.SaveWindowPosition(UiWindowPositionLeft, UiWindowPositionTop, UiWindowSizeWidth, UiWindowSizeHeight);
            }

            // Shutting down the kernel module
            Kernel.Shutdown();
        }

        /// <summary>
        /// If a doubleclick was done on a treeview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdConnect(object sender, ConnectEventArgs e)
        {
            try
            {


                //Check if there was an Item selected
                if (e.ConnectionItem == null)
                    return;

                QuickConnectText = String.Empty;

                //Check for special modification
                var connectWithoutCredentials = e.IgnoreCredentials;

                //var conHost = new ConnectionHost(0, "", "", "", 0, 0, 0, 0, true, 0);
                var conProt = new ConnectionProtocol(0, 0, "", 0);

                //ImageSource tabIcon = null;

                switch (e.ConnectionItem.ConnectionType)
                {
                        //For Folders
                    default:
                        return;

                        //For Connections; open default protocol
                    case ConnectionTypeItems.connection:
                        var conHost = StorageCore.Core.GetConnection(e.ConnectionItem.ConnectionID);
                        var conProts = StorageCore.Core.GetConnectionSettings(e.ConnectionItem.ConnectionID);
                        var userSettings = StorageCore.Core.GetUserSettings();

                        //If the owner is a foreign user, connect without credentials because this credentials are not available
                        if (conHost.Owner != StorageCore.Core.GetUserId())
                            connectWithoutCredentials = true;

                        foreach (var prot in conProts)
                        {
                            //Check if it is default protocol; use it, else use the first entry
                            if (prot.Protocol == userSettings.DefaultProtocol)
                            {
                                conProt = prot;
                            }
                        }

                        //Checks if the default Protocol was configured. If not: Take first entry. If no entrys available: cancel
                        if (conProt.Id == 0 && conProts.Count > 0)
                        {
                            foreach (var aProtocol in conProts)
                            {
                                if (!Kernel.GetAvailableProtocols().ContainsKey(aProtocol.Protocol))
                                    continue;

                                conProt = aProtocol;
                                break;
                            }
                        }
                        else if (conProts.Count == 0)
                            return;

                        break;

                        //For Protocol; connect using this protocol
                    case ConnectionTypeItems.protocol:
                        conProt = StorageCore.Core.GetConnectionSetting(e.ConnectionItem.ConnectionID);
                        conHost = StorageCore.Core.GetConnection(conProt.ConnectionId);

                        //If the owner is a foreighn user, connect without credentials because this credentials are not available
                        if (conHost.Owner != StorageCore.Core.GetUserId())
                            connectWithoutCredentials = true;

                        break;
                }

                //Establish connection
                var host = Host.LoadConnection(conProt.ConnectionId);

                var session = host.NewSession(conProt.Id);

                //Apply modifcations
                if (connectWithoutCredentials)
                    session.CleanSessionCredentials();

                //Create new Tab
                var tabBase = AddTab(session.GetSessionWindow(), session.GetSessionID());

                tabBase.ConnectionOpening += session.OnConnectionOpening;
                tabBase.ConnectionClosing += session.OnConnectionClosing;

                tabBase.TriggerConnectionOpeningEvent();

                StorageCore.Core.AddUserHistoryEntry(conProt.Id);

                ConnectionSession.OpenConnectionThreadedAsync(session);

            }
            catch (BadImageFormatException badImageFormatException)
            {
                TrayIcon.TrayIconInstance.ShowNotification("The plugin that is used for this connection has unresolvable errors. Try reinstalling it or contact the support.", badImageFormatException);
            }
            catch (Exception exception)
            {
                TrayIcon.TrayIconInstance.ShowNotification("An unexpected error occured.\n" + exception.Message, exception);
                //throw new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                //    "Unknown error while trying to connect to a remote system.", exception);
            }
        }


        /// <summary>
        /// Executed, if a general AddTab-Button was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdAddTab(object sender, AddTabEventArgs e)
        {
            //Add the new Tab
            AddTab(e.Tab, e.ContentId);
        }

        /// <summary>
        /// Executed, if an Item should be removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CmdItemRemove(object sender, ItemRemoveEventArgs e)
        {
            var myTask = Task.Run(
                () => (ConnectionManagement.RemoveItem(e.Item)));

            //Wait for delete
            await myTask;

            //Reload the ConnectionList
            LoadConnectionListAsync(0);

            //Reload Favorites
            LoadFavoritesButtons(StorageCore.Core.GetUserVisuals().Favorites);
        }

        /// <summary>
        /// Executed, if the Mainwindow gets the command to close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CmdMainWindowClose_ApplicationClose(object sender, MainWindowCloseEventArgs e)
        {
            e.View.Close();
        }

        #endregion

        #region Non-Command Eventtriggers

        /// <summary>
        /// Triggered, whenever a connection will be opend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connid"></param>
        private void Kernel_OnNewConnectionTriggered(object sender, long connid)
        {
            if (null != StorageCore.Core.GetConnection(connid))
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    CmdConnect(null, new ConnectEventArgs
                    {
                        ConnectionItem = new ConnectionItem(
                            "", ConnectionTypeItems.connection, null, null, null, connid, "", "", "")
                    });
                }));
            }
            else
            {
                TrayIcon.TrayIconInstance.ShowNotification(String.Format("The supplied connection (id: {0}) does not exist.", connid));
            }
           
        }

        /// <summary>
        /// Executed, when the IsKernelReady-Property changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kernel_OnKernelReady(object sender, KernelReadyEventArgs e)
        {
            // Activating the tray icon
            Dispatcher.BeginInvoke(new Action(delegate // Use Dispather to keep notifyicon existent, even if other threads go nuts
            {
                TrayIcon.TrayIconInstance.SetIconState(TrayIconState.LoginScreen);
                TrayIcon.TrayIconInstance.Show();
                
            }));

            //check, if the Database is up to date
            StorageCore.UpdateDatabase();

            //Get RibbonWatermark
            LoadWatermarkAsync();

            // Register for InterComm events if necessary
            Kernel.InterCommServer.Events.UiFocusMainWindow += InterCommEvent_UiFocusMainWindow;

            //Check, if it is Single-User-Mode
            if (StorageCore.Core.GetSetting("singleusermode") == "1")
            {
                //Application-parameter-implementation is required to do this
                _Login.ExecuteLogin("User", Helper.ConvertToSecureString(StorageCore.Core.GetSetting("singleuserpara")));
            }

            try
            {
                //If something went wrong, the cancel-Tag can be set; no GUI will be shown
                if (e.CancelInitialisation)
                {
                    Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    Application.Current.Shutdown();
                }
                else //Show GUI
                {
                    #region Check Arguments
                    try
                    {
                        try
                        {
                            //todo BK
                            //App.ParsedOptions.ParseArguments(Environment.GetCommandLineArgs());
                            if (false == String.IsNullOrEmpty(Kernel.ParsedOptions.Username))
                            {
                                _Login.LoginUsername = Kernel.ParsedOptions.Username;
                            }

                            if (null != Kernel.ParsedOptions.Password)
                            {
                                _Login.ExecuteLogin(_Login.LoginUsername, Kernel.ParsedOptions.Password);
                            }

                        }
                        catch (Exception ea)
                        {
                            Logger.Log(LogEntryType.Exception, "Error in startupprocess. Did you run the correct build for your architecture?", ea);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Problem during startup: " + ex);
                    }
                    #endregion

                    //todo BK
                    //todo evalutate parsed options
                    //if (App.ParsedOptions.Username != null && App.ParsedOptions.Password != null) //Autologin
                    //{
                    //    var vals = new Dictionary<string, Object>();
                    //    vals.Add("username", App.ParsedOptions.Username);
                    //    vals.Add("password", App.ParsedOptions.Password);
                    //    vals.Add("delay", 1000);
                    //    UiLoginButtonEnabled = false;
                    //    new Thread(StartLoginProcess).Start(vals);

                    //}
                    //else if (App.ParsedOptions.Username != null) //Set user-Field
                    //{
                    //    txtUsername.Text = App.ParsedOptions.Username;
                    //    UiLoginButtonEnabled = true;
                    //}
                    //else
                    //    UiLoginButtonEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem in first phase of KernelReady \n\r" + ex);
            }

            //Enable Login-Button
            UiLoginButtonEnabled = true;
        }

        void InterCommEvent_UiFocusMainWindow()
        {
            Application.Current.MainWindow.Dispatcher.BeginInvoke(new Action(delegate
            {
                Application.Current.MainWindow.BringIntoView();
                Application.Current.MainWindow.Focus();
            }));


        }

        /// <summary>
        /// If the Kernel-Initialization fails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kernel_OnKernelFailed(object sender, KernelFailedEventArgs e)
        {

            Application.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
                if (e.Exception != null)
                {
                    if (e.Exception.GetType() == typeof(ApplicationAlreadyRunningException))
                    {
                        CmdMainWindowClose.Execute(this);
                        return;
                    }
                }

                new UIExceptionWindow(e.Exception, true).ShowDialog();
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Kernel.Shutdown();

                Application.Current.Shutdown();
            }));


        }

        /// <summary>
        /// Executed, if a Login was successful
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginLoginSuccessful(object sender, LoginSuccessfulEventArgs e)
        {
            StatusbarDisplayName = "Welcome to beRemote, " + e.UserSettings.Name; //Update Statusbar

            //Set the property i.e. to enable Ribbons
            IsUserLoggedIn = true;

            //If the user has no visual settings
            if (e.UserSettings.VisualSettings != null)
            {

                SetWindowPosition(
                    e.UserSettings.VisualSettings.MainWindowWidth,
                    e.UserSettings.VisualSettings.MainWindowHeight,
                    e.UserSettings.VisualSettings.MainWindowPosX,
                    e.UserSettings.VisualSettings.MainWindowPosY);

                MainWindowState = SaveLoad.LoadWindowState(e.UserSettings.VisualSettings.MainWindowMax);

                #region Load Avalon Gridlayout (not MVVM conform)

                Dispatcher.BeginInvoke(new Action(() =>
                                                  {
                                                      var mainDock = Application.Current.Windows[0].FindName("adMainDock") as DockingManager;
                                                      SaveLoad.LoadSettingsDock(mainDock);
                                                      ConnectionViewItemsExpanded = SaveLoad.LoadSettingsTreeViewState();
                                                  }
                    ));

                #endregion

                #region Load Ribbonstate

                //Set the Ribbonstate. Old versions will be ignored, because Length is larger than 1
                if (e.UserSettings.VisualSettings.RibbonState.Length == 1)
                {
                    //Set the Minimized-Property
                    IsRibbonMinimized = (e.UserSettings.VisualSettings.RibbonState == "1");
                }

                #endregion

                //Load Favorites-Buttons
                LoadFavoritesButtons(e.UserSettings.VisualSettings.Favorites);


                #region Initial Window Properties
                _UiWindowSizeWidth = _DefaultWindowWidth;
                _UiWindowSizeHeight = _DefaultWindowHeight;
                UiWindowPositionLeft = Convert.ToInt32((SystemParameters.PrimaryScreenWidth - UiWindowSizeWidth) / 2);
                UiWindowPositionTop = Convert.ToInt32((SystemParameters.PrimaryScreenHeight - UiWindowSizeHeight) / 2);
                #endregion

                // Lets start this in another thread in order to wait till the ui is rendered completely
                new Thread(new ThreadStart(delegate
                {
                    if (false == String.IsNullOrEmpty(Kernel.ParsedOptions.ConnectionSettingId))
                    {
                        Thread.Sleep(1000);

                        Kernel.TriggerNewConnection(long.Parse(Kernel.ParsedOptions.ConnectionSettingId));
                    }
                })).Start();

              
            }

            #region Superadmin-Stuff

            //Set if the User is superadmin
            IsUserSuperadmin = e.IsUserSuperadmin;

            #endregion

            //Load the ConnectionFilters
            LoadConnectionFilterSetsAsync();

            //Load the Add-Button-Menu-Items
            LoadProtocolButtons();

            //Load the Plugin-Buttons (i.e. in "Tools and Features")
            LoadPluginButtons();

            //Set the TrayIcon
            TrayIcon.TrayIconInstance.SetIconState(TrayIconState.LoggedIn);

            //Not used in the GUI_V1
            //AbstractBeRemoteServiceClient.OnRequestStarted += AbstractBeRemoteServiceClient_OnRequestStarted;
            //AbstractBeRemoteServiceClient.OnRequestFinished += AbstractBeRemoteServiceClient_OnRequestFinished;

            //Start Heartbeat
            _HeartbeatWorker.StartHeartbeat();

            //Load the ConnectionList
            LoadConnectionListAsync(0);

            //Load the Overview-History
            LoadHistoryAsync();

            //Hide the Login-Grid
            ShowLoginGrid = false;

            Dispatcher.BeginInvoke(new Action(() =>
                                              {
                                                  var x = new ViewModelStatusBarBase();
                                                  x.Element = new StatusBar.Time.SbTime();
                                                  StatusBarElements.Add(x);
                                              }
                ))
            ;

           
        }

        /// <summary>
        /// Execute, if an unknown Username was entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginUserNotExists(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(GetText("MsgUserNotExistsText"), GetText("MsgUserNotExistsTitle"), MessageBoxButton.OK, MessageBoxImage.Exclamation);
            LoginProcessing = false;
        }

        /// <summary>
        /// Show a generic Message by using the Login-Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Login_ShowMessage(object sender, ShowMessageEventArgs e)
        {
            MessageBox.Show(e.Message, e.Title, MessageBoxButton.OK, e.Image);
        }

        /// <summary>
        /// Executed if the Loginproces starts after a successful authentication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginLoginProcessing(object sender, LoginProcessingEventArgs e)
        {
            LoginProcessing = e.IsLoginProcessed;
        }

        /// <summary>
        /// If the wrong password was entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginLoginFailed(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(GetText("MsgAccessDeniedText"), GetText("MsgAccessDeniedTitle"), MessageBoxButton.OK, MessageBoxImage.Error);

            Dispatcher.BeginInvoke(new Action(delegate
            {
                if (LoginDelay < 1) //First attemps wait 1 sec
                {
                    LoginDelay++;
                }
                else //Increase after the first attemps
                {
                    LoginDelay *= 2;
                }

                //wait until LoginDelay expires
                System.Threading.Thread.Sleep(LoginDelay * 1000);

                //Renableing the Login-Interface
                LoginProcessing = false;
            }));
        }

        /// <summary>
        /// Executed, if a tab raises the Close-Event
        /// </summary>
        /// <param name="sender">The Content of the Tab, that Raises the Close-Event</param>
        /// <param name="e"></param>
        void TabContent_Close(object sender, RoutedEventArgs e)
        {
            //Find the Tab and remove it of the ContentTab-Collection
            for (var i = 0; i < ContentTabs.Count; i++)
            {
                if (!Equals(ContentTabs[i].Content, sender))
                    continue;

                //Let the Content-Control cleanup memory, if it is of type TabBase
                if (ContentTabs[i].Content is TabBase)
                    ((TabBase)ContentTabs[i].Content).Dispose();

                //Cleanup the ViewModel
                ContentTabs[i].Dispose();                

                //Remove the Tab of the visual
                ContentTabs.RemoveAt(i);
                break;
            }
        }

        /// <summary>
        /// Executed, if a Tab raises the FavoritesChanged-Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabContent_FavoritesChanged(object sender, FavoriteChangedEventArgs e)
        {
            LoadFavoritesButtons(e.Favorites);
        }

        /// <summary>
        /// Execute, if a Tab Raises the ConnectionListChanged-Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabContent_ConnectionListChanged(object sender, RoutedEventArgs e)
        {
            LoadConnectionListAsync(0);
        }

        /// <summary>
        /// Raised, if a Tab wants to change the visiblity of a context-ribbon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabContent_ContextRibbonVisibileChange(object sender, ContextRibbonVisibileChangeEventArgs e)
        {
            if (e.HideContextRibbon != null)
            {
                foreach (var aHide in e.HideContextRibbon)
                {
                    switch (aHide)
                    {
                        case "ribTabFilter":
                            IsContextRibbonFilterVisible = false;                            
                            break;
                        case "ribTabVpn":
                            IsContextRibbonVpnVisible = false;                            
                            break;
                    }
                }
                SelectedRibbonIndex = 0;
            }
            
            if (e.ShowContextRibbon != null)
            {
                foreach (var aHide in e.ShowContextRibbon)
                {
                    switch (aHide)
                    {
                        case "ribTabFilter":
                            IsContextRibbonFilterVisible = true;
                            if (e.ShowContextRibbon.IndexOf("ribTabFilter") == 0)
                                IsContextRibbonFilterSelected = true;
                            break;
                        case "ribTabVpn":
                            IsContextRibbonVpnVisible = true;
                            if (e.ShowContextRibbon.IndexOf("ribTabVpn") == 0)
                                IsContextRibbonVpnSelected = true;
                            break;
                    }
                }
            }
        }

        #endregion

        #region Public Properties

        #region Login Region
        private bool _LoginProcessing;

        /// <summary>
        /// The Name, typed into the Loginform
        /// </summary>
        public string LoginUsername
        {
            get { return (_Login.LoginUsername); }
            set
            {
                _Login.LoginUsername = value;
                RaisePropertyChanged("LoginUsername");
            }
        }

        /// <summary>
        /// Get if there is currently a Login processed
        /// </summary>
        public bool LoginProcessing
        {
            get { return _LoginProcessing; }
            private set
            {
                _LoginProcessing = value;
                RaisePropertyChanged("LoginProcessing");
            }
        }

        /// <summary>
        /// The Delay of the reenabling of the Login-Interface after a failed attempt
        /// </summary>
        public int LoginDelay
        {
            get;
            private set;
        }


        private bool _ShowChangePassword;
        /// <summary>
        /// Is the ChangePassword-Dialog visible?
        /// </summary>
        public bool ShowChangePassword
        {
            get { return _ShowChangePassword; }
            set
            {
                _ShowChangePassword = value;
                RaisePropertyChanged("ShowChangePassword");
            }
        }

        private bool _ShowCreateAccount;
        /// <summary>
        /// Is the CreateAccount-Dialog visible?
        /// </summary>
        public bool ShowCreateAccount
        {
            get { return _ShowCreateAccount; }
            set
            {
                _ShowCreateAccount = value;
                RaisePropertyChanged("ShowCreateAccount");
            }
        }

        #endregion

        #region CreateAccount Region

        /// <summary>
        /// Gets/Sets the Password of the CreateAccount-Control
        /// </summary>
        public SecureString CreateAccountPassword1 { get; set; }

        /// <summary>
        /// Gets/Sets the Password repeatation of the CreateAccount-Control
        /// </summary>
        public SecureString CreateAccountPassword2 { get; set; }


        private string _CreateAccountUsername = "";
        /// <summary>
        /// Gets/Sets the Username/Loginname of a new user
        /// </summary>
        public string CreateAccountUsername
        {
            get { return _CreateAccountUsername; }
            set
            {
                _CreateAccountUsername = value;
                RaisePropertyChanged("CreateAccountUsername");
            }
        }


        private string _CreateAccountDisplayname = "";
        /// <summary>
        /// Gets/sets the Displayname of a new user
        /// </summary>
        public string CreateAccountDisplayname
        {
            get { return _CreateAccountDisplayname; }
            set
            {
                _CreateAccountDisplayname = value;
                RaisePropertyChanged("CreateAccountDisplayname");
            }
        }

        #endregion

        #region Window Style

        #region UiWindowSizeWidth
        private int _UiWindowSizeWidth;

        /// <summary>
        /// The Width of the MainWindow
        /// </summary>
        public int UiWindowSizeWidth
        {
            get { return _UiWindowSizeWidth; }
            set
            {
                //Ignore unwanted callbacks
                if (value == _DefaultWindowWidth) return;

                _UiWindowSizeWidth = value;
                RaisePropertyChanged("UiWindowSizeWidth");
            }
        }
        #endregion

        #region UiWindowSizeHeight
        private int _UiWindowSizeHeight;

        /// <summary>
        /// The Height of the MainWindow
        /// </summary>
        public int UiWindowSizeHeight
        {
            get { return _UiWindowSizeHeight; }
            set
            {
                //Ignore unwanted callbacks
                if (value == _DefaultWindowHeight) return;

                _UiWindowSizeHeight = value;
                RaisePropertyChanged("UiWindowSizeHeight");
            }
        }
        #endregion

        #region UiWindowPositionLeft
        private int _UiWindowPositionLeft;

        /// <summary>
        /// The Left-Position of the MainWindow
        /// </summary>
        public int UiWindowPositionLeft
        {
            get { return _UiWindowPositionLeft; }
            set
            {
                _UiWindowPositionLeft = value;
                RaisePropertyChanged("UiWindowPositionLeft");
            }
        }
        #endregion

        #region UiWindowPositionTop
        private int _UiWindowPositionTop;

        /// <summary>
        /// The Top Position of the MainWindow
        /// </summary>
        public int UiWindowPositionTop
        {
            get { return _UiWindowPositionTop; }
            set
            {
                _UiWindowPositionTop = value;
                RaisePropertyChanged("UiWindowPositionTop");
            }
        }
        #endregion

        #region UiWindowSizeMaximized
        private WindowState _UiWindowSizeMaximized;

        /// <summary>
        /// The Width of the MainWindow
        /// </summary>
        public WindowState UiWindowSizeMaximized
        {
            get { return _UiWindowSizeMaximized; }
            set
            {
                _UiWindowSizeMaximized = value;
                RaisePropertyChanged("UiWindowSizeMaximized");
            }
        }
        #endregion

        #region IsRibbonMinimized

        private bool _IsRibbonMinimized;

        /// <summary>
        /// Is the Ribbon minimized, so the Ribbonbar has to be expanded before use?
        /// </summary>
        public bool IsRibbonMinimized
        {
            get { return _IsRibbonMinimized; }
            set
            {
                _IsRibbonMinimized = value;
                RaisePropertyChanged("IsRibbonMinimized");
            }
        }

        #endregion

        #region ShowLoginGrid

        private bool _ShowLoginGrid = true;

        public bool ShowLoginGrid
        {
            get { return _ShowLoginGrid; }
            set
            {
                _ShowLoginGrid = value;
                RaisePropertyChanged("ShowLoginGrid");
            }
        }
        #endregion

        #region RibbonWatermark

        private ImageSource _RibbonWatermark = BitmapFrame.Create(new Uri("pack://application:,,,/Images/beremotebanner_watermark.png", UriKind.RelativeOrAbsolute));

        public ImageSource RibbonWatermark
        {
            get { return _RibbonWatermark; }
            set
            {
                _RibbonWatermark = value;
                RaisePropertyChanged("RibbonWatermark");
            }
        }

        #endregion

        #region RibbonWatermarkOpacity

        private double _RibbonWatermarkOpacity = 0.5;

        /// <summary>
        /// Defines the Opacity of the Ribbon Watermark
        /// </summary>
        public double RibbonWatermarkOpacity
        {
            get { return _RibbonWatermarkOpacity; }
            set
            {
                _RibbonWatermarkOpacity = value;
                RaisePropertyChanged("RibbonWatermarkOpacity");
            }
        }

        #endregion

        #region UiLoginButtonEnabled

        private bool _UiLoginButtonEnabled;

        /// <summary>
        /// Is the Login-Button Enabled?
        /// </summary>
        public bool UiLoginButtonEnabled
        {
            get { return _UiLoginButtonEnabled; }
            set
            {
                _UiLoginButtonEnabled = value;
                RaisePropertyChanged("UiLoginButtonEnabled");
            }
        }
        #endregion

        #region MainWindowState

        private WindowState _MainWindowState;

        /// <summary>
        /// The state of the Main Window
        /// </summary>
        public WindowState MainWindowState
        {
            get { return _MainWindowState; }
            set
            {
                _MainWindowState = value;
                RaisePropertyChanged("MainWindowState");
            }
        }
        #endregion

        #endregion

        #region StatusbarDisplayName

        private string _StatusbarDisplayName = "";

        public string StatusbarDisplayName
        {
            get { return _StatusbarDisplayName; }
            set
            {
                _StatusbarDisplayName = value;
                RaisePropertyChanged("StatusbarDisplayName");
            }

        }

        #endregion

        #region ConnectionFilter

        private List<FilterSet> _ConnectionFilterList = new List<FilterSet>();

        /// <summary>
        /// Contains the ConnectionFilters
        /// </summary>
        public List<FilterSet> ConnectionFilterList
        {
            get { return _ConnectionFilterList; }
            set
            {
                _ConnectionFilterList = value;
                RaisePropertyChanged("ConnectionFilterList");
            }
        }

        #endregion

        #region ConnectionFilterSelected

        private FilterSet _ConnectionFilterSelected;

        /// <summary>
        /// The Filter that is selected in the ConnectinFilter Combobox
        /// </summary>
        public FilterSet ConnectionFilterSelected
        {
            get { return _ConnectionFilterSelected; }
            set
            {
                _ConnectionFilterSelected = value;
                RaisePropertyChanged("ConnectionFilterSelected");
            }
        }
        #endregion

        #region FavoriteButtons

        private List<FavoriteItem> _FavoriteButtons = new List<FavoriteItem>();

        /// <summary>
        /// The Favorite-Buttons in the Server-Ribbon
        /// </summary>
        public List<FavoriteItem> FavoriteButtons
        {
            get { return _FavoriteButtons; }
            set
            {
                _FavoriteButtons = value;
                RaisePropertyChanged("FavoriteButtons");
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

        #region ToolsAndFeatures

        /// <summary>
        /// Gets all available Tools and Feature-Buttons
        /// </summary>
        public IList<UiPlugin> ToolsAndFeatures
        {
            get
            {
                if (Kernel.IsKernelReady())
                {
                    var buttons = Kernel.GetAvailableUIPlugins().Values;


                    return buttons;
                }

                return (new List<UiPlugin>());
            }
        }

        #endregion

        #region ActiveContent

        private ViewModelTabBase _ActiveDocument;

        public object ActiveDocument
        {
            get
            {
                return _ActiveDocument;
            }
            set
            {
                if (value == null)
                    _ActiveDocument = null;


                if (value == null || value.GetType() != typeof(ViewModelTabBase))
                    return;

                _ActiveDocument = (ViewModelTabBase)value;
                RaisePropertyChanged("ActiveDocument");
            }
        }
        #endregion

        #region ContentTabs

        private readonly ObservableCollection<ViewModelTabBase> _ContentTabs = new ObservableCollection<ViewModelTabBase>();

        /// <summary>
        /// Contains all Tabs, shown in the Content-Area
        /// </summary>
        public ObservableCollection<ViewModelTabBase> ContentTabs
        {
            get { return _ContentTabs; }
            //private set
            //{
            //    _ContentTabs = value;
            //    RaisePropertyChanged("ContentTabs");
            //}
        }

        #endregion

        #region ConnectionViewItems
        private ObservableCollection<ConnectionItem> _ConnectionViewItems = new ObservableCollection<ConnectionItem>();

        /// <summary>
        /// The ConnectionTreeView-Content
        /// </summary>
        public ObservableCollection<ConnectionItem> ConnectionViewItems
        {
            get { return (_ConnectionViewItems); }
            set
            {
                _ConnectionViewItems = value;
                RaisePropertyChanged("ConnectionViewItems");
            }
        }
        #endregion

        #region ConnectionHistory

        private DataTable _ConnectionHistory = new DataTable();

        /// <summary>
        /// The last connected Connections
        /// </summary>
        public DataTable ConnectionHistory
        {
            get { return _ConnectionHistory; }
            private set
            {
                _ConnectionHistory = value;
                RaisePropertyChanged("ConnectionHistory");
            }
        }

        #endregion

        #region ConnectionViewItemsExpanded

        private string _ConnectionViewItemsExpanded = "";

        /// <summary>
        /// Contains the currently expanded Notes of the connectiontreeview
        /// </summary>
        public string ConnectionViewItemsExpanded
        {
            get { return _ConnectionViewItemsExpanded; }
            set
            {
                _ConnectionViewItemsExpanded = value;
                RaisePropertyChanged("ConnectionViewItemsExpanded");
            }
        }

        #endregion

        #region QuickAccessButtons

        private ObservableCollection<Object> _QuickAccessButtons = new ObservableCollection<Object>();

        public ObservableCollection<Object> QuickAccessButtons
        {
            get { return _QuickAccessButtons; }
            set
            {
                _QuickAccessButtons = value;
                RaisePropertyChanged("QuickAccessButtons");
            }
        }

        #endregion

        #region QuickConnectText

        private string _QuickConnectText = string.Empty;

        public string QuickConnectText
        {
            get { return _QuickConnectText; }
            set
            {
                _QuickConnectText = value;

                if (_QuickConnectText.Length > 0)
                    IsQuickConnectDescriptionVisible = false;
                else
                    IsQuickConnectDescriptionVisible = true;

                RaisePropertyChanged("QuickConnectText");
            }
        }
        #endregion

        #region IsQuickConnectDescriptionVisible

        private bool _IsQuickConnectDescriptionVisible = true;

        public bool IsQuickConnectDescriptionVisible
        {
            get { return _IsQuickConnectDescriptionVisible; }
            set
            {
                if (_IsQuickConnectDescriptionVisible == value)
                    return;

                _IsQuickConnectDescriptionVisible = value;
                RaisePropertyChanged("IsQuickConnectDescriptionVisible");
            }
        }
        #endregion

        #region IsUserSuperadmin
        private bool _IsUserSuperadmin;

        /// <summary>
        /// Is a logged in User Superadmin
        /// </summary>
        public bool IsUserSuperadmin
        {
            get { return (_IsUserSuperadmin); }
            private set
            {
                _IsUserSuperadmin = value;
                RaisePropertyChanged("IsUserSuperadmin");
            }
        }
        #endregion

        #region IsContextRibbonFilterVisible

        private bool _IsContextRibbonFilterVisible;

        /// <summary>
        /// Is the ContextRibbon for Fiterset-Management visible?
        /// </summary>
        public bool IsContextRibbonFilterVisible
        {
            get { return _IsContextRibbonFilterVisible; }
            set
            {
                _IsContextRibbonFilterVisible = value;
                RaisePropertyChanged("IsContextRibbonFilterVisible");
            }
        }

        #endregion

        #region IsContextRibbonVpnVisible

        private bool _IsContextRibbonVpnVisible;

        /// <summary>
        /// Is the ContextRibbon for Fiterset-Management visible?
        /// </summary>
        public bool IsContextRibbonVpnVisible
        {
            get { return _IsContextRibbonVpnVisible; }
            set
            {
                _IsContextRibbonVpnVisible = value;
                RaisePropertyChanged("IsContextRibbonVpnVisible");
            }
        }

        #endregion

        #region IsContextRibbonFilterSelected

        private bool _IsContextRibbonFilterSelected;

        /// <summary>
        /// The currently RibbonIndex of the Contextual VPN-Ribbon
        /// </summary>
        public bool IsContextRibbonFilterSelected
        {
            get { return _IsContextRibbonFilterSelected; }
            set
            {
                if (value == true)
                    IsContextRibbonFilterVisible = true;

                _IsContextRibbonFilterSelected = value;
                RaisePropertyChanged("IsContextRibbonFilterSelected");
            }
        }
        #endregion

        #region IsContextRibbonVpnSelected

        private bool _IsContextRibbonVpnSelected;

        /// <summary>
        /// The currently RibbonIndex of the Contextual VPN-Ribbon
        /// </summary>
        public bool IsContextRibbonVpnSelected
        {
            get { return _IsContextRibbonVpnSelected; }
            set
            {
                if (value == true)
                    IsContextRibbonVpnVisible = true;

                _IsContextRibbonVpnSelected = value;
                RaisePropertyChanged("IsContextRibbonVpnSelected");
            }
        }
        #endregion

        #region SelectedRibbonIndex

        private int _SelectedRibbonIndex;

        /// <summary>
        /// The currently selected RibbonIndex
        /// </summary>
        public int SelectedRibbonIndex
        {
            get { return _SelectedRibbonIndex; }
            set
            {
                _SelectedRibbonIndex = value;
                RaisePropertyChanged("SelectedRibbonIndex");
            }
        }
        #endregion
        
        #region ShowForcePwChangeGrid
        private bool _ShowWizardFirstRun;

        /// <summary>
        /// Is the Force-Password-Change-Grid visible?
        /// </summary>
        public bool ShowWizardFirstRun
        {
            get { return (_ShowWizardFirstRun); }
            set
            {
                _ShowWizardFirstRun = value;
                RaisePropertyChanged("ShowWizardFirstRun");
            }
        }

        #endregion

        #region StatusbarElements

        private ObservableCollection<ViewModelStatusBarBase> _StatusBarElements = new ObservableCollection<ViewModelStatusBarBase>();

        /// <summary>
        /// The Elements that are Shown in the StatusBar
        /// </summary>
        public ObservableCollection<ViewModelStatusBarBase> StatusBarElements
        {
            get { return _StatusBarElements; }
            set
            {
                _StatusBarElements = value;
                RaisePropertyChanged("StatusBarElements");
            }
        }
        #endregion

        #region IsUserLoggedIn

        private bool _IsUserLoggedIn;

        /// <summary>
        /// Is a User logged in?
        /// </summary>
        public bool IsUserLoggedIn
        {
            get { return _IsUserLoggedIn; }
            set
            {
                _IsUserLoggedIn = value;
                RaisePropertyChanged("IsUserLoggedIn");
            }
        }
        #endregion
       
        #endregion

        #region Private Variables
        private readonly Login _Login = new Login(); //Contains Methods for the login-process
        private readonly Heartbeat _HeartbeatWorker = new Heartbeat(); //Contains Backgroundworking Methods like the Heartbeat
        private readonly ConnectionList _ConnectionListWorker = new ConnectionList(); //Worker for ConnectionList-Loading, Searching and Filtering
        private readonly ResourceDictionary _LangDictionary = new ResourceDictionary(); //Contains the Language-Variables
        private const int _DefaultWindowWidth = 800; //Default size if the Mainwindow
        private const int _DefaultWindowHeight = 600; //Default size if the Mainwindow

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

        #region Helper

        /// <summary>
        /// Gets the Text of a LanguageResource
        /// </summary>
        /// <param name="resourceName">The name as defined in the language(.xxx).xaml</param>
        /// <returns></returns>
        private string GetText(string resourceName)
        {
            if (!_LangDictionary.Contains(resourceName))
                throw new Exception("The Language-Resource " + resourceName + " doesn't exists.");

            return (_LangDictionary[resourceName].ToString());
        }

        #endregion

        #region Events (for triggering things in the Frontend)

        #region WindowProperties
        public delegate void WindowPropertiesChangedEventHandler(object sender, WindowPropertiesChangedEventArgs e);

        public event WindowPropertiesChangedEventHandler WindowPropertiesChanged;

        protected virtual void OnWindowPropertiesChanged(WindowPropertiesChangedEventArgs e)
        {
            var Handler = WindowPropertiesChanged;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region CloseApplication
        public delegate void CloseApplicationEventHandler(object sender, RoutedEventArgs e);

        public event CloseApplicationEventHandler CloseApplication;

        protected virtual void OnCloseApplication(RoutedEventArgs e)
        {
            var Handler = CloseApplication;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion


        #endregion
    }
}
