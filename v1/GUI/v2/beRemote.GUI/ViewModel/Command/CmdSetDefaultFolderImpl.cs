using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdSetDefaultFolderImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            if (sender == null)
                return;

            var currentItem = (ConnectionItem)sender;

            if (currentItem.ConnectionType == ConnectionTypeItems.folder) //Just to be sure a folder is selected
                StorageCore.Core.ModifyUserDefaultFolder(StorageCore.Core.GetUserId(), currentItem.ConnectionID);
        }
    }
}
