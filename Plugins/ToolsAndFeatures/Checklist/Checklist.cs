using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using beRemote.GUI.Plugins;
using beRemote.Core.Common.PluginBase;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Media;
using beRemote.VendorPlugins.Checklist.ViewModel;

namespace beRemote.VendorPlugins.Checklist
{
    [PluginMetadata(
       PluginName = "Checklist", //The Displayname of the Plugin
       PluginFullQualifiedName = "beRemote.VendorProtocols.Checklist", //The internal Plugin of the Plugin
       PluginDescription = "With the checklist-plugin you are able to manage your daily work and share your checklists with your collegues or print it", //A short description
       PluginAuthor = "beRemote Team", //The Auther-Name
       PluginAuthorMail = "support@beremote.net", //A Contact-Mail-Address
       PluginWebsite = "www.beremote.net", //A Homepage
       PluginType = PluginType.UiToolsAndFeatures, //The Type of a Plugin. For Tools and Features choose UiToolsAndFeatures
       PluginConfigFolder = "beRemote.VendorProtocols.Checklist", //The Folder where Resources for this plugins are stored (i.e. the ini-File)
       PluginIniFile = "config.ini" //The Name of the inifile. Maybe you don't need one. You can delete the Ini-File but don't delete this string.
       )]
    [Export(typeof(UiPlugin))]
    public class Checklist : UiPlugin, INotifyPropertyChanged
    {
        public Checklist()
        {
            _Icon = new Icon("Images\\icon.ico");
        }

        #region Required Methods
        private MainTab _UiContent;

        /// <summary>
        /// The control, that will be shown in the Tab
        /// </summary>
        /// <returns></returns>        
        public override Control GetTabControl()
        {
            //If the Control is not initialized, initialize it and return it. Else return the existing control.
            return _UiContent ?? (_UiContent = new MainTab());
        }

        /// <summary>
        /// When the button in the beRemote Ribbonbar was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ButtonAction(object sender, System.Windows.RoutedEventArgs e)
        {
            TriggerOnUiPluginOpenTab(this);
        }

        private Icon _Icon;
        /// <summary>
        /// Get the Icon of this Plugin
        /// </summary>
        public override Icon Icon
        {
            get{return _Icon;}
        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


    }
}