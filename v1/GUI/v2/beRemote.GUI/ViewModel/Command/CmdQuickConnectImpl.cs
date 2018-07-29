using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using beRemote.GUI.ViewModel.EventArg;
using beRemote.GUI.ViewModel.Worker;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdQuickConnectImpl : BaseCommand
    {
        public override void Execute(object parameter)
        {
            if (parameter == null)
                return;

            var para = (QuickConnectEventArgs) parameter;

            if (string.IsNullOrEmpty(para.Text) || para.Key == null)
                return;

            //Was the return-Key pressed?
            if (para.Key == Key.Enter)
            {   
                //Create and get new QuickConnect-Connection
                var qcItem = QuickConnect.DoQuickConnect(para.Text, (para.SelectedProtocol == null ? "" : para.SelectedProtocol.GetProtocolIdentifer()));

                //Open connection
                var evArgs = new ConnectEventArgs();
                evArgs.ConnectionItem = qcItem;
                OnConnectEvent(evArgs);

                //Refresh ConnectionTreeView
                var evArgs2 = new ReloadConnectionListEventArgs();
                evArgs2.AddedConnection = qcItem;
                OnReloadConnectionList(evArgs2);
            }
        }
    }
}
