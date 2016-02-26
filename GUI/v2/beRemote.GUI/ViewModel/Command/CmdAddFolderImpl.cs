using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.Command;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdAddFolderImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            var evArgs = new AddTabEventArgs();
            evArgs.Tab = new Tabs.ManageFolder.TabManageFolder((ConnectionItem) sender);

            OnAddTab(evArgs);
        }
    }
}
