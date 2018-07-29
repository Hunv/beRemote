using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.Control.ConnectionTreeViewNS.ViewModel;

namespace beRemote.GUI.Control.ConnectionTreeViewNS
{
    public class ConnectionTreeViewBase : TreeViewBase<ShopCategory>
    {
        /// <summary>
        /// Generates a unique identifier for a given
        /// item that is represented as a node of the
        /// tree.
        /// </summary>
        /// <param name="item">An item which is represented
        /// by a tree node.</param>
        /// <returns>A unique key that represents the item.</returns>
        public override string GetItemKey(ShopCategory item)
        {
            return item.CategoryName;
        }


        /// <summary>
        /// Gets all child items of a given parent item. The
        /// tree needs this method to properly traverse the
        /// logic tree of a given item.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override ICollection<ShopCategory> GetChildItems(ShopCategory parent)
        {
            return parent.SubCategories;
        }

        /// <summary>
        /// Gets the parent of a given item, if available. If
        /// the item is a top-level element, this method is supposed
        /// to return a null reference.
        /// </summary>
        /// <param name="item">The currently processed item.</param>
        /// <returns>The parent of the item, if available.</returns>
        public override ShopCategory GetParentItem(ShopCategory item)
        {
            return item.ParentCategory;
        }
    }

}
