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
    public class CmdSetDefaultProtocolImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            if (sender == null)
                return;

            var currentItem = (ConnectionItem)sender;

            if (currentItem.ConnectionType == ConnectionTypeItems.protocol) //Just to be sure a protocol is selected
                StorageCore.Core.ModifyUserDefaultProtocol(StorageCore.Core.GetConnectionSetting(currentItem.ConnectionID).getProtocol());
        }
    }
}
