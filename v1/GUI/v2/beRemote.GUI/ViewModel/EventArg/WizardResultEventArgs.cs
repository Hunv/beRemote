using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class WizardResultEventArgs : RoutedEventArgs
    {
        public string WizardName { get; set; }
        public Boolean Result { get; set; }
        public string ResultMessage { get; set; }
    }
}
