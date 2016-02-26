using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.GUI.Tabs.Import.Command
{
    public class CmdBrowseImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnStartBrowsing(new System.Windows.RoutedEventArgs());
        }
    }
}
