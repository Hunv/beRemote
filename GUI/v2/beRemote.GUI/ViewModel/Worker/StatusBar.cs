using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.GUI.ViewModel.Worker
{
    public class StatusBar
    {
        public ObservableCollection<ViewModelStatusBarBase> GetStatusBarElements()
        {
            var returnCollection = new ObservableCollection<ViewModelStatusBarBase>();


            return (returnCollection);
        }
    }
}
