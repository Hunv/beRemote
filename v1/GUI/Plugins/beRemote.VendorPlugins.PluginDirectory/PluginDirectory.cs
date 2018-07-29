using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using beRemote.Core;
using beRemote.Core.Common.PluginBase;
using beRemote.GUI.Plugins;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Media;
using beRemote.VendorPlugins.PluginDirectory.UI;
using Newtonsoft.Json;
using Plugin = beRemote.Services.PluginDirectory.Library.Objects.Plugin;

namespace beRemote.VendorPlugins.PluginDirectory
{
    [PluginMetadata(
       PluginName = "beRemote Plugin Directory",
       PluginFullQualifiedName = "beRemote.VendorPlugins.PluginDirectory.PluginDirectory",
       PluginDescription = "Plugin for browsing through our online plugin database",
       PluginAuthor = "Benedikt Kroening",
       PluginAuthorMail = "benediktkroening@beremote.net",
       PluginWebsite = "www.beremote.net",
       PluginType = PluginType.UiToolsAndFeatures,
       PluginConfigFolder = "beRemote.VendorPlugins.PluginDirectory",
       PluginIniFile = "config.ini"

       )]
    [Export(typeof(UiPlugin))]
    public class PluginDirectory : UiPlugin, INotifyPropertyChanged
    {
        private Boolean _uiDisplayInstalledPlugins = false;

        public Boolean UiDisplayInstalledPlugins
        {
            get
            {
                return _uiDisplayInstalledPlugins;
            }
            set
            {
                if (PluginList != null)
                {
                    foreach (var plg in PluginList)
                    {
                        if (!value && plg.IsInstalled)
                            plg.IsObjectVisible = false;
                        else
                        {
                            plg.IsObjectVisible = true;
                        }
                        //if(value && plg.IsInstalled)
                        //    plg.IsObjectVisible = true;

                        //if (value && !plg.IsInstalled)
                        //    plg.IsObjectVisible = true;

                        //if (!value && !plg.IsInstalled)
                        //    plg.IsObjectVisible = true;

                        
                    }
                }
                SetField(ref _uiDisplayInstalledPlugins, value, "UiDisplayInstalledPlugins");
            }
        }

        private UI.PluginList _pluginListUi;

        private List<Plugin> _pluginList = null;
        public List<Plugin> PluginList 
        {
            get { return _pluginList; }
            private set { SetField(ref _pluginList, value, "PluginList"); } 
        }

        public delegate void UpdatePluginDirectoryFinishedEventHandler();
        public event UpdatePluginDirectoryFinishedEventHandler OnUpdateDirectoryFinished;

        public delegate void UpdatePluginDirectoryStateChangedEventHandler(String stateMessage);
        public event UpdatePluginDirectoryStateChangedEventHandler OnUpdateDirectoryStateChanged;

        public delegate void UpdatePluginDirectoryFailedEventHandler(String message, Exception ex);
        public event UpdatePluginDirectoryFailedEventHandler OnUpdateDirectoryFailed;

        public PluginDirectory()
        {
           
        }

        public override Control GetTabControl()
        {
            return _pluginListUi ?? (_pluginListUi = new PluginList(this));
        }
        public override void ButtonAction(object sender, System.Windows.RoutedEventArgs e)
        {
            this.TriggerOnUiPluginOpenTab(this);
        }
        public override Icon Icon
        {
            get { return Properties.Resources.Plugin; }
        }

        public String BaseUrl
        {
            get { return Config.GetValue("ServiceClient", "ServiceUrl"); }
        }

        private String _updateListStateMessage = "";
        public String UpdateListStateMessage
        {
            get { return _updateListStateMessage; }
            set
            {
                SetField(ref _updateListStateMessage, value, "UpdateListStateMessage");
            }
        }

        public void UpdatePluginDirectory()
        {
            try
            {
                NotifyUpdateStateChanged("Starting list update...", true);
                List<Plugin> t_list = new List<Plugin>();
                using (var client = new PluginDirectoryClient(BaseUrl))
                {
                    client.Login();

                    t_list = client.GetAllPluginInformation();

                    NotifyUpdateStateChanged(String.Format("Got {0} plugins from server...\r\nVerifying installed plugins...", t_list.Count), true);
                }

                foreach (var pluginInfo in t_list)
                {
                    //if (Kernel.GetAvailableProtocols().ContainsKey(pluginInfo.FullQuallifiedAssemblyName))
                    //{
                    //    pluginInfo.IsInstalled = true;
                    //}

                    //if (Kernel.GetAvailableUIPlugins().ContainsKey(pluginInfo.FullQuallifiedAssemblyName))
                    //{
                    //    pluginInfo.IsInstalled = true;
                    //}
                    var list = Kernel.GetAvailablePlugins();
                    if (list.ContainsKey(pluginInfo.FullQuallifiedAssemblyName))
                    {
                        var plugin = list[pluginInfo.FullQuallifiedAssemblyName];
                        
                        pluginInfo.IsInstalled = true;

                        var assemblyVersion = plugin.GetType().Assembly.GetName().Version;
                        if (assemblyVersion.Major < pluginInfo.PluginVersion.Major)
                            pluginInfo.UpdateAvailable = true;
                        else if (assemblyVersion.Minor < pluginInfo.PluginVersion.Minor)
                            pluginInfo.UpdateAvailable = true;
                        else if (assemblyVersion.Build < pluginInfo.PluginVersion.Build)
                            pluginInfo.UpdateAvailable = true;
                        else if (assemblyVersion.Revision < pluginInfo.PluginVersion.Revision)
                            pluginInfo.UpdateAvailable = true;

                        // Modification times will override the assembly version
                        var assemblyDate = plugin.GetLastModificationDate();
                        var pluginDate = pluginInfo.LastChanged;

                        var result = DateTime.Compare(assemblyDate, pluginDate);
                        // == >0 (1) means assembly is newer, e.g. developer build and not commited to svn
                        // == 0 equal 
                        // == <0 (-1) means server is newer, we will update the shit out of it
                        if (result < 0)
                            pluginInfo.UpdateAvailable = true;

                    }


                }

                PluginList = t_list;

                NotifyUpdateFinished(true);
            }
            catch (Exception ex)
            {
                NotifyUpdateFailed("A problem occured while updating the plugin list.", ex);
            }
           
        }

        private void NotifyUpdateFailed(string p, Exception ex)
        {
            if (OnUpdateDirectoryFailed != null)
            {
                if (_pluginListUi != null)
                    _pluginListUi.Dispatcher.BeginInvoke(new Action(() => OnUpdateDirectoryFailed(p, ex)));
                else
                    OnUpdateDirectoryFailed(p, ex);
            }
        }

        private void NotifyUpdateStateChanged(string message, bool uiThread)
        {
            UpdateListStateMessage = message;

            if (OnUpdateDirectoryStateChanged != null)
            {
                if (uiThread && _pluginListUi != null)
                    _pluginListUi.Dispatcher.BeginInvoke(new Action(() => OnUpdateDirectoryStateChanged(message)));
                else
                    OnUpdateDirectoryStateChanged(message);
            }
        }
        private void NotifyUpdateFinished(bool uiThread)
        {
            if (OnUpdateDirectoryFinished != null)
            {
                if (uiThread && _pluginListUi != null)
                    _pluginListUi.Dispatcher.BeginInvoke(new Action(() => OnUpdateDirectoryFinished()));
                else
                    OnUpdateDirectoryFinished();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}



//public void UpdateLocalPluginDirectory()
//{
//    List<Plugin> plugins;
//    using (var client = new PluginDirectoryClient("https://svc.beremote.net/services/plg-dir-svc"))
//    {
//        client.Login();

//        plugins = client.GetAllPluginInformation();

//        client.Logout();
//    }
//}