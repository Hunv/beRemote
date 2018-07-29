using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.Tabs.ManageVpn.ViewModel.Commands
{
    public class CmdVpnTabLoadedImpl : BaseCommand
    {
        public override void Execute(object parameter)
        {
            ((TabManageVpn)parameter).ChangeContextRibbon(new List<string> { "ribTabVpn" }, null);

            OnTabLoaded(new RoutedEventArgs());
        }
    }
}
