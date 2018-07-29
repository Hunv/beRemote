using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using beRemote.Core;
using beRemote.Services.PluginDirectory.Library.Objects;

namespace beRemote.VendorPlugins.PluginDirectory.UI
{
    /// <summary>
    /// Interaction logic for PluginList.xaml
    /// </summary>
    public partial class PluginList
    {
        public PluginDirectory DirectoryInstance;
        
        public PluginList(PluginDirectory instance)
        {
            InitializeComponent();
            
            DirectoryInstance = instance;

            this.DataContext = instance;

            instance.OnUpdateDirectoryFinished += instance_OnUpdateDirectoryFinished;
            instance.OnUpdateDirectoryStateChanged += instance_OnUpdateDirectoryStateChanged;
            instance.OnUpdateDirectoryFailed += instance_OnUpdateDirectoryFailed;

            GrdLock.Visibility = Visibility.Visible;
            GrdLockLoader.Visibility = Visibility.Visible;
            GrdLockPluginDetails.Visibility= Visibility.Collapsed;

            this.Loaded += PluginListControl_Loaded;
        }
        private bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
        void instance_OnUpdateDirectoryFailed(string message, Exception ex)
        {
            DirectoryInstance.UpdateListStateMessage = message + "\r\n" + ex.Message;

            if (false == HasWriteAccessToFolder("plugins"))
            {
                MessageBox.Show(Application.Current.MainWindow, "You do not have write permissions in the beRemote folder. This plugin will not offer all available functionality due to his missing permissions.", "Missing permissions");
            }

            pbLoading.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void PluginListControl_Loaded(object sender, RoutedEventArgs e)
        {
            StartUpdate();
        }

        private void StartUpdate()
        {
            // Changes the order to lock the listview, and to stay over everything of the UI
            Canvas.SetZIndex(GrdLock, 102);

            new Thread(new ThreadStart(() => DirectoryInstance.UpdatePluginDirectory())).Start();
        }

        void instance_OnUpdateDirectoryStateChanged(string stateMessage)
        {
            
        }

        void instance_OnUpdateDirectoryFinished()
        {
            DirectoryInstance.UiDisplayInstalledPlugins = true;
            this.UpdateLayout();

            GrdLock.Visibility = Visibility.Collapsed;
            GrdLockLoader.Visibility = Visibility.Collapsed;
            GrdLockPluginDetails.Visibility = Visibility.Collapsed;
        }

        private void PluginItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var plugin = (Plugin)((ListViewItem)sender).Content;

            GrdLock.Visibility = Visibility.Visible;
            GrdLockLoader.Visibility = Visibility.Collapsed;
            GrdLockPluginDetails.Visibility = Visibility.Visible;

            // Changes the order to lock the listview, and to stay behind the detail view but over the listview
            Canvas.SetZIndex(GrdLock, 101);

            GrdLockPluginDetails.DataContext = plugin;
        }

        private void PluginDetailClose_Click(object sender, RoutedEventArgs e)
        {
            GrdLock.Visibility = Visibility.Collapsed;
            GrdLockLoader.Visibility = Visibility.Collapsed;
            GrdLockPluginDetails.Visibility = Visibility.Collapsed;
        }

        private void InstallPlugin_OnClick(object sender, RoutedEventArgs e)
        {
            DoInstall(true);
            //DirectoryInstance.
        }

        private void DoInstall(bool withMessage)
        {
            if (withMessage)
            {
                if (false == HasWriteAccessToFolder("plugins"))
                {
                    MessageBox.Show(Application.Current.MainWindow,
                        "You do not have write permissions in the beRemote folder. This plugin will not offer all available functionality due to his missing permissions.",
                        "Missing permissions");
                    return;
                }
            }

            // Changes the order to lock the listview, and to stay over everything of the UI
            Canvas.SetZIndex(GrdLock, 102);
            GrdLockLoader.Visibility = Visibility.Visible;

            DirectoryInstance.UpdateListStateMessage = "Preparing plugin installation...";

            var plugin = (Plugin)GrdLockPluginDetails.DataContext;

            new Thread(new ThreadStart(delegate
            {
                using (var client = new PluginDirectoryClient(DirectoryInstance.BaseUrl))
                {
                    client.Login();
                    DirectoryInstance.UpdateListStateMessage = "Downloading plugin... please be patient";
                    var fi = client.DownloadPlugin(plugin);
                    DirectoryInstance.UpdateListStateMessage = "Plugin downloaded.";
                    if (fi != null)
                    {
                        // moving it to the install-on-next-start-folder
                        try
                        {
                            Kernel.InsertPluginUpdate(fi);

                            // If we reach this, all things gone right :)
                            if(withMessage)
                                this.Dispatcher.BeginInvoke(new Action(delegate { MessageBox.Show(Application.Current.MainWindow, "The plugin installation is prepared and will be executed after beRemote is restarted.", "Installation prepared!", MessageBoxButton.OK, MessageBoxImage.Information); }));
                            this.Dispatcher.BeginInvoke(new Action(delegate
                            {

                                GrdLockLoader.Visibility = Visibility.Collapsed;
                                GrdLock.Visibility = Visibility.Collapsed;
                                GrdLockPluginDetails.Visibility = Visibility.Collapsed;
                                plugin.IsUpdateable = false;
                                plugin.IsInstalled = true;
                            }));

                        }
                        catch (Exception ex)
                        {
                            String message = ex.Message;
                            if (ex.InnerException != null)
                                message += "\r\n" + ex.InnerException.Message;
                            DirectoryInstance.UpdateListStateMessage = message;

                            this.Dispatcher.BeginInvoke(new Action(delegate { MessageBox.Show(Application.Current.MainWindow, message, "Error in plugin container", MessageBoxButton.OK, MessageBoxImage.Error); }));
                        }

                    }
                }
            })).Start();

        }

        private void UninstallPlugin_OnClick(object sender, RoutedEventArgs e)
        {
            DoUninstall(true);
        }

        private void DoUninstall(Boolean withMessage)
        {
            if (withMessage)
            {
                if (false == HasWriteAccessToFolder("plugins"))
                {
                    MessageBox.Show(Application.Current.MainWindow,
                        "You do not have write permissions in the beRemote folder. This plugin will not offer all available functionality due to his missing permissions.",
                        "Missing permissions");
                    return;
                }
            }
            var plugin = (Plugin)GrdLockPluginDetails.DataContext;

            Canvas.SetZIndex(GrdLock, 102);
            GrdLockLoader.Visibility = Visibility.Visible;

            DirectoryInstance.UpdateListStateMessage = "Preparing plugin removal...";

            new Thread(new ThreadStart(delegate
            {
                Kernel.RemovePlugin(plugin.FullQuallifiedAssemblyName);

                if (withMessage)
                    this.Dispatcher.BeginInvoke(new Action(delegate { MessageBox.Show(Application.Current.MainWindow, "The plugin removal is prepared and will be executed upon the next beRemote restart.", "Removal prepared!", MessageBoxButton.OK, MessageBoxImage.Information); }));

                this.Dispatcher.BeginInvoke(new Action(delegate
                {

                    GrdLockLoader.Visibility = Visibility.Collapsed;
                    GrdLock.Visibility = Visibility.Collapsed;
                    GrdLockPluginDetails.Visibility = Visibility.Collapsed;
                    plugin.IsUpdateable = false;
                    plugin.IsInstalled = true; // will be set to installed until beRemote restart
                }));

            })).Start();
        }


        private void PrepareUpdate(object sender, RoutedEventArgs e)
        {
            if (false == HasWriteAccessToFolder("plugins"))
            {
                MessageBox.Show(Application.Current.MainWindow,
                    "You do not have write permissions in the beRemote folder. This plugin will not offer all available functionality due to his missing permissions.",
                    "Missing permissions");
                return;
            }
            #region remove

            var plugin = (Plugin)GrdLockPluginDetails.DataContext;
            Canvas.SetZIndex(GrdLock, 102);
            GrdLockLoader.Visibility = Visibility.Visible;
            DirectoryInstance.UpdateListStateMessage = "Preparing plugin removal...";
            new Thread(new ThreadStart(delegate
            {
                Kernel.RemovePlugin(plugin.FullQuallifiedAssemblyName);

            })).Start();
            
            #endregion
            new Thread(new ThreadStart(delegate
            {
                using (var client = new PluginDirectoryClient(DirectoryInstance.BaseUrl))
                {
                    client.Login();
                    DirectoryInstance.UpdateListStateMessage = "Downloading plugin... please be patient";
                    var fi = client.DownloadPlugin(plugin);
                    DirectoryInstance.UpdateListStateMessage = "Plugin downloaded.";
                    if (fi != null)
                    {
                        // moving it to the install-on-next-start-folder
                        try
                        {
                            Kernel.InsertPluginUpdate(fi);

                            // If we reach this, all things gone right :)
                            this.Dispatcher.BeginInvoke(new Action(delegate { MessageBox.Show(Application.Current.MainWindow, "The plugin update is prepared and will be executed upon the next beRemote restart.", "Update prepared!", MessageBoxButton.OK, MessageBoxImage.Information); }));

                            this.Dispatcher.BeginInvoke(new Action(delegate
                            {

                                GrdLockLoader.Visibility = Visibility.Collapsed;
                                GrdLock.Visibility = Visibility.Collapsed;
                                GrdLockPluginDetails.Visibility = Visibility.Collapsed;
                                plugin.IsUpdateable = false;
                                plugin.IsInstalled = true;
                            }));

                        }
                        catch (Exception ex)
                        {
                            String message = ex.Message;
                            if (ex.InnerException != null)
                                message += "\r\n" + ex.InnerException.Message;
                            DirectoryInstance.UpdateListStateMessage = message;

                            this.Dispatcher.BeginInvoke(new Action(delegate { MessageBox.Show(Application.Current.MainWindow, message, "Error in plugin container", MessageBoxButton.OK, MessageBoxImage.Error); }));
                        }

                    }
                }
            })).Start();

           
        }

        private void CmdRefreshData_OnClick(object sender, RoutedEventArgs e)
        {
            StartUpdate();
        }
    }
}
