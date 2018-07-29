using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using beRemote.Core.Common.PluginBase;
using beRemote.GUI.Plugins;
using beRemote.VendorPlugins.DatabaseConverter.UI;

namespace beRemote.VendorPlugins.DatabaseConverter
{
    [PluginMetadata(
        PluginName = "beRemote Database Converter",
        PluginFullQualifiedName = "beRemote.VendorPlugins.DatabaseConverter.DatabaseConverter",
        PluginDescription = "Converts Databases pre Version 0.0.3 to the current Database-Format",
        PluginAuthor = "Kristian Reukauff",
        PluginAuthorMail = "kristianreukauff@beremote.net",
        PluginWebsite = "www.beremote.net",
        PluginType = PluginType.UiToolsAndFeatures,
        PluginConfigFolder = "beRemote.VendorPlugins.DatabaseConverter",
        PluginIniFile = ""
        )]

    [Export(typeof (UiPlugin))]
    public class DatabaseConverter : UiPlugin, INotifyPropertyChanged
    {
        public DatabaseConverter()
        {
        }

        private DatabaseConverterControl _UiContent;

        public override Control GetTabControl()
        {
            //If the Control is not initialized, initialize it and return it. Else return the existing control.
            return _UiContent ?? (_UiContent = new DatabaseConverterControl());
        }

        public override void ButtonAction(object sender, System.Windows.RoutedEventArgs e)
        {
            TriggerOnUiPluginOpenTab(this);
        }

        public override Icon Icon
        {
            get
            {
                var icn = new Icon("plugins\\ui\\beRemote.VendorPlugins.DatabaseConverter\\res\\databaseconverter.ico");
                return icn;
            }
        }


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
    }

}
