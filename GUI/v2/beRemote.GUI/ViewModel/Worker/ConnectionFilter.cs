using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.Core.Definitions.Enums.Filter;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Worker
{
    public class ConnectionFilter
    {
        public static void AddFilter(ObservableCollection<ViewModelTabBase> tabs, FilterType filterTypeToAdd)
        {
            foreach (var aTab in tabs)
            {
                if (aTab.Content.GetType() == typeof (Tabs.ManageFilter.TabManageFilter))
                {
                    var theTab = (Tabs.ManageFilter.TabManageFilter) aTab.Content;
                    theTab.AddFilter(filterTypeToAdd);
                    break;
                }
            }
        }
    }
}
