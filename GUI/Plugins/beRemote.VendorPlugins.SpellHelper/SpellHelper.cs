using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Common.PluginBase;
using System.ComponentModel.Composition;
using beRemote.GUI.Plugins;
using System.ComponentModel;
using System.Windows.Controls;
using System.Drawing;
using beRemote.VendorPlugins.SpellHelper.UI;

namespace beRemote.VendorPlugins.SpellHelper
{
    [PluginMetadata(
    PluginName = "beRemote Spell Helper",
    PluginFullQualifiedName = "beRemote.VendorPlugins.SpellHelper.SpellHelper",
    PluginDescription = "Helps you to spell words by displaying words, that begins with this letter in several languages.",
    PluginAuthor = "Kristian Reukauff",
    PluginAuthorMail = "kristianreukauff@beremote.net",
    PluginWebsite = "www.beremote.net",
    PluginType = PluginType.UiToolsAndFeatures,
    PluginConfigFolder = "beRemote.VendorPlugins.SpellHelper",
    PluginIniFile = ""
    )]

    [Export(typeof(UiPlugin))]
    public class SpellHelper : UiPlugin, INotifyPropertyChanged
    {
        public SpellHelper()
        {
        }

        private SpellHelperControl _UiContent;
        
        public override Control GetTabControl()
        {
            //If the Control is not initialized, initialize it and return it. Else return the existing control.
            return _UiContent ?? (_UiContent = new SpellHelperControl());
        }

        public override void ButtonAction(object sender, System.Windows.RoutedEventArgs e)
        {
            TriggerOnUiPluginOpenTab(this);
        }
        public override Icon Icon
        {
            get
            {
                var icn = new Icon("plugins\\ui\\beRemote.VendorPlugins.SpellHelper\\res\\SpellHelper.ico");
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
