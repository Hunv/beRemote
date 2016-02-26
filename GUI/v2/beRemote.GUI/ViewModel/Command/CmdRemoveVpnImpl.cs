using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdRemoveVpnImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnVpnRemove(new System.Windows.RoutedEventArgs());
        }
    }
}
