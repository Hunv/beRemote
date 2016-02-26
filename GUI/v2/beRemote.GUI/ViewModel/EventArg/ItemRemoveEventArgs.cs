using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class ItemRemoveEventArgs : RoutedEventArgs
    {
        public ItemRemoveEventArgs()
        {
        }

        public ItemRemoveEventArgs(ConnectionItem item)
        {
            Item = item;
        }

        public ConnectionItem Item { get; set; }
    }
}
