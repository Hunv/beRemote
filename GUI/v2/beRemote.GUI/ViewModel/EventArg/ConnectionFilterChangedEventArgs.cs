using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.Core.Definitions.Classes;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class ConnectionFilterChangedEventArgs : RoutedEventArgs
    {
        public FilterSet NewFilter { get; set; }
    }
}
