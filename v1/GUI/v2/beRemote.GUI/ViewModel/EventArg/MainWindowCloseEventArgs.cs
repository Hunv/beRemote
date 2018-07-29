using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class MainWindowCloseEventArgs : RoutedEventArgs
    {
        public MainWindow View { get; set; }
    }
}
