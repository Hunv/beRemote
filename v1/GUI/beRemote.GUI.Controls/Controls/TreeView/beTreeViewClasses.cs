using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.Controls.TreeView
{
    public class beTreeViewDragDropEventArgs : RoutedEventArgs
    {
        private ConnectionItem _Target;
        private ConnectionItem _Source;

        public ConnectionItem Target { get { return _Target; } set { _Target = value; } }
        public ConnectionItem Source { get { return _Source; } set { _Source = value; } }
    }

    public class beTreeViewOrphanItem
    {
        private long _ParentId = 0;
        private ConnectionItem _OrphanItem;

        public long ParentId
        {
            get { return _ParentId; }
            set { _ParentId = value; }
        }

        public ConnectionItem OrphanItem
        {
            get { return _OrphanItem; }
            set { _OrphanItem = value; }
        }
    }
}
