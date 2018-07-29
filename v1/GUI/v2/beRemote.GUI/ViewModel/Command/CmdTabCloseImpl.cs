using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdTabCloseImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            if (!(sender is DocumentClosedEventArgs))
                return;

            if (!(((DocumentClosedEventArgs)sender).Document.Content is ViewModelTabBase))
                return;

            var sendingContent = ((ViewModelTabBase)((DocumentClosedEventArgs)sender).Document.Content).Content;

            OnCloseTab(sendingContent, new System.Windows.RoutedEventArgs());
        }
    }
}
