using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.Tabs.ManageVpn.ViewModel
{
    public class BrowseFileEventArgs : RoutedEventArgs
    {
        public string BrowseSender { get; set; }
        public string BrowsePath { get; set; }
    }
}
