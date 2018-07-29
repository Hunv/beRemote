using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.ViewModel.Command;

namespace beRemote.GUI.Tabs.ManageProfile.Commands
{
    public class CmdCancelClickImpl : BaseCommand
    {
        public override void Execute(object parameter)
        {
            OnCloseTab(new RoutedEventArgs());
        }
    }
}
