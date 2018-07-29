using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.Tabs.Import.Command
{
    public class CmdImportImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnStartImport(new RoutedEventArgs());
        }
    }
}
