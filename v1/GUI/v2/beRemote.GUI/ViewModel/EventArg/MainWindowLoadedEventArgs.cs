using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.AvalonDock;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class MainWindowLoadedEventArgs : RoutedEventArgs
    {
        public DockingManager DockMgr
        { get; set; }
    }
}
