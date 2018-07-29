using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.GUI.Plugins;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdPluginButtonImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            var plugin = (UiPlugin)sender;

            var evArgs = new AddTabEventArgs();
            evArgs.Tab = plugin.GetTabControl();

            OnAddTab(evArgs);

            plugin.ButtonAction(sender, new System.Windows.RoutedEventArgs());
        }
    }
}
