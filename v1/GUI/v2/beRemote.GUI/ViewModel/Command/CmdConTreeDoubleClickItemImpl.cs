using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.Command;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdConTreeDoubleClickItemImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            if (sender == null)
                return;
            
            var CEa = new ConnectEventArgs();
            CEa.IgnoreCredentials = false;
            CEa.ConnectionItem = (ConnectionItem)sender;
            OnConnectEvent(CEa);
        }
    }
}
