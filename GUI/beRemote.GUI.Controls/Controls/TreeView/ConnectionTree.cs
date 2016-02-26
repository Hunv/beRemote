using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Hardcodet.Wpf.GenericTreeView;
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.Controls
{
    public class ConnectionTree : TreeViewBase<ConnectionItem>
    {
        /// <summary>
        /// Generates a unique identifier for a given item that is represented as a node of the tree.
        /// </summary>
        /// <param name="item">An item which is represented by a tree node.</param>
        /// <returns>A unique key that represents the item.</returns>
        public override string GetItemKey(ConnectionItem item)
        {
            //return item.ConnectionType.ToString() + ":" + item.ConnectionID + ":" + item.ConnectionName; //Fixing #323
            return item.ConnectionType.ToString() + ":" + item.ConnectionID;
        }


        /// <summary>
        /// Gets all child items of a given parent item. 
        /// </summary>
        /// <param name="parent">A currently processed item that is being represented as a node of the tree.</param>
        /// <returns>All child items to be represented by the tree. </returns>    
        public override ICollection<ConnectionItem> GetChildItems(ConnectionItem parent)
        {
            return parent.SubConnections;
        }

        /// <summary>
        /// Gets the parent of a given item, if available. If the item is a top-level element, this method is supposed to return a null reference.
        /// </summary>
        /// <param name="item">The currently processed item.</param>
        /// <returns>The parent of the item, if available.</returns>
        public override ConnectionItem GetParentItem(ConnectionItem item)
        {
            return item.ConnectionParent;
        }

    }
}