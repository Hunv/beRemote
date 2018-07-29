using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using beRemote.GUI.Controls;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.Command;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdConnectWithoutCredentialsImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            if (sender == null)
                return;

            var CEa = new ConnectEventArgs();
            CEa.IgnoreCredentials = true;
            CEa.ConnectionItem = (ConnectionItem)sender;
            OnConnectEvent(CEa);
        }
    }
}
