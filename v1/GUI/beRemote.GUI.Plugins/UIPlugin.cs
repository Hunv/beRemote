using System.IO;
using System.Windows.Interop;
using beRemote.Core.Common.PluginBase;
using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace beRemote.GUI.Plugins
{
    public abstract class UiPlugin : Plugin
    {
        public delegate void UiPluginOpenTabEventHandler(UiPlugin sender);
        public event UiPluginOpenTabEventHandler OnUiPluginOpenTab;

        //public abstract String IcoPath { get; }
        public abstract Icon Icon { get; }
        public void TriggerOnUiPluginOpenTab(UiPlugin sender)
        {
            if (MetaData.PluginType == PluginType.UiToolsAndFeatures)
            {
                if (OnUiPluginOpenTab != null)
                    OnUiPluginOpenTab(sender);
            }
            else
            {
                throw new beRemoteException(beRemoteExInfoPackage.MajorInformationPackage, "It is not supported that non tools and features plugins open tabs!");
            }
        }

        /// <summary>
        /// If plugin is of type ToolsAndFeatures this method has to be overriden and handled by usercode
        /// </summary>        
        public virtual Control GetTabControl() { return null; }

        /// <summary>
        /// The image that is used to display the basic trigger button for this plugin. Displayed in the Tools and feature ribbon
        /// </summary>
        public ImageSource ButtonIcon
        {
            get { return GetImageFromIco(Icon, 32); }
        }
        /// <summary>
        /// The image that is used to display as icon in any tab header
        /// </summary>
        public ImageSource TabIcon
        {
            get { return GetImageFromIco(Icon, 16); }
        }


        private Dictionary<int, ImageSource> _iconCache = new Dictionary<int, ImageSource>();

        public ImageSource GetImageFromIco(Icon icon, int size)
        {
            if (false == _iconCache.ContainsKey(size))
            {
                var imageSource = Imaging.CreateBitmapSourceFromHIcon(
                    new Icon(icon, size, size).Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                _iconCache.Add(size, imageSource);
            }
            return _iconCache[size];
            
        }

        /// <summary>
        /// Method to execute on click action (in tools and features area)
        /// </summary>
        public abstract void ButtonAction(object sender, RoutedEventArgs e);

        public String PluginFolder { get; private set; }

        public void InitiateUiPlugin(String pluginFolder)
        {
           
            PluginFolder = pluginFolder;
            if (MetaData.PluginType == PluginType.UiToolsAndFeatures)
            {
                //if(ButtonIcon == null)
                //    throw new PluginException(beRemoteExInfoPackage.SignificantInformationPackage, String.Format("Plugin not useable due to missing icon"));

            }
            else
            {
                throw new PluginException(beRemoteExInfoPackage.SignificantInformationPackage, String.Format("Plugin of type '{0}' is not implemented in beRemote", MetaData.PluginType.ToString()));
            }
        }

        public override string GetPluginIdentifier()
        {
            return this.MetaData.PluginFullQualifiedName;
        }
    }
}
