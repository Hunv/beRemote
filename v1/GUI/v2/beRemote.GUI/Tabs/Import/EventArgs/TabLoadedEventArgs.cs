using beRemote.GUI.Controls.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.Tabs.Import.EventArgs
{
    public class TabLoadedEventArgs : RoutedEventArgs
    {
        public ObservableCollection<ConnectionItem> FolderList { get; set; }
    }
}
