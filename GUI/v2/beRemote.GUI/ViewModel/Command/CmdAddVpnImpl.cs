using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdAddVpnImpl:BaseCommand
    {
        public override void Execute(object sender)
        {
            var ev = new EventArg.VpnAddEventArgs();
            ev.VpnType = sender.ToString();
            OnVpnAdd(ev);            
        }
    }
}
