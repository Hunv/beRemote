using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using beRemote.Core;
using beRemote.Core.Definitions.Classes;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdAddConnectionImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            var tabEventArg = new AddTabEventArgs();

            if (sender == null)
                tabEventArg.Tab = new Tabs.ManageConnection.TabManageConnection(null);

            //Executed by the TreeView with a given Parent-Folder-ID
            if (sender is ConnectionItem &&
                ((ConnectionItem) sender).ConnectionType == ConnectionTypeItems.folder) 
            {
                tabEventArg.Tab = new Tabs.ManageConnection.TabManageConnection(((ConnectionItem) sender).ConnectionID); //Parameter = FolderId
            }
            //Executes by the Splitbutton with a given Protocol-String-ID
            else if (sender is string &&
                Kernel.GetAvailableProtocols().ContainsKey(sender.ToString())) 
            {
                tabEventArg.Tab = new Tabs.ManageConnection.TabManageConnection(sender.ToString()); //Parameter == Protocol-Identifier
            }
            //Executed by the TreeView on adding a new Protocol to an existing connection
            else if (sender is ConnectionItem &&
                     ((ConnectionItem) sender).ConnectionType == ConnectionTypeItems.connection)
            {
                var para = new object[2] {((ConnectionItem) sender).ConnectionID, true};

                tabEventArg.Tab = new Tabs.ManageConnection.TabManageConnection(para); //Parameter = ConnectionId, IsNewProtocolOnConnection
            }
            //sender is something else
            else 
            {
                tabEventArg.Tab = new Tabs.ManageConnection.TabManageConnection(null);
            }

            OnConnectionAdd(tabEventArg);
        }
    }
}
