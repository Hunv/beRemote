using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdSortItemUpImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            var currentItem = (ConnectionItem)sender;
            var sortIndex = currentItem.ConnectionParent.SubConnections.IndexOf(currentItem);

            if (currentItem.ConnectionType == ConnectionTypeItems.connection)
            {
                StorageCore.Core.ModifyConnection(currentItem.ConnectionID, sortIndex, true);
            }
            else if (currentItem.ConnectionType == ConnectionTypeItems.folder)
            {
                StorageCore.Core.ModifyFolderSortOrder(currentItem.ConnectionID, sortIndex, true);
            }

            //todo: Rework this after TreeView was redeveloped
            var evArgs = new ReloadConnectionListEventArgs();
            evArgs.ReloadFromDatabase = true;

            OnReloadConnectionList(evArgs);
        }
    }
}
