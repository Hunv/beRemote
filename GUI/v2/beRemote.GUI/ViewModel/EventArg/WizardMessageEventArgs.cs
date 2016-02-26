using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class WizardMessageEventArgs : RoutedEventArgs
    {
        public string Message { get;set;}
        public MessageBoxImage MessageImage { get; set; }
        public string Title { get; set; }
    }
}
