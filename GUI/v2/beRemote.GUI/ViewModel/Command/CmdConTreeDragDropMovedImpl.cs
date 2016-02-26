using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdConTreeDragDropMovedImpl : BaseCommand
    {
        public override void Execute(object eventArgs)
        {
            //Get the EventArgs and save the Changes
            var e = (GUI.Controls.TreeView.beTreeViewDragDropEventArgs) eventArgs;
            switch (e.Source.ConnectionType)
            {
                case ConnectionTypeItems.folder:
                    StorageCore.Core.ModifyFolderParent(e.Source.ConnectionID, e.Target.ConnectionID);
                    break;
                case ConnectionTypeItems.connection:
                    StorageCore.Core.ModifyConnection(e.Source.ConnectionID, e.Target.ConnectionID);
                    break;
            }

            //Populate the changes to the GUI
            var evArgs = new ReloadConnectionListEventArgs();
            evArgs.ReloadFromDatabase = true;
            OnReloadConnectionList(evArgs);
        }
    }
}
