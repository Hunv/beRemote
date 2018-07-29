using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.GUI.Tabs.ManageVpn.ViewModel.Commands
{
    public class CmdVpnTabUnloadedImpl : BaseCommand
    {
        public override void Execute(object parameter)
        {
            ((TabManageVpn)parameter).ChangeContextRibbon(null, new List<string> { "ribTabVpn" });
        }
    }
}
