using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.Core.Definitions.Classes;
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class ConnectEventArgs : RoutedEventArgs
    {
        public bool IgnoreCredentials { get; set; }
        public ConnectionItem ConnectionItem { get; set; }
    }
}
