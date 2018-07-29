using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.Core.Definitions.Enums.Filter;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class FilterAddEventArgs : RoutedEventArgs
    {
        public FilterType FilterType { get; set; }
    }
}
