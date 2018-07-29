using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.Core.Definitions.Classes;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class FilterSetEventArgs : RoutedEventArgs
    {
        public FilterSetEventArgs()
        {
        }


        public FilterSetEventArgs(List<FilterSet> filterSetList)
        {
            FilterSetList = filterSetList;
        }

        public List<FilterSet> FilterSetList { get; set; }
    }
}
