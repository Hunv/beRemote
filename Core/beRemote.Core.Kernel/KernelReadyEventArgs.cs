using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace beRemote.Core
{
    public class KernelReadyEventArgs : RoutedEventArgs
    {
        public bool CancelInitialisation { get; set; }
    }
}
