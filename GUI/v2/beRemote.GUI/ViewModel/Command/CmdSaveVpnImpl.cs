using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdSaveVpnImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnVpnSave(new System.Windows.RoutedEventArgs());
        }
    }
}
