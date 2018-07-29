using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using beRemote.GUI.Controls.Classes;

namespace beRemote.GUI.Controls
{
    /// <summary>
    /// Interaction logic for ImagedConnectionTreeViewControl.xaml
    /// </summary>
    public partial class ImagedConnectionTreeViewControl : System.Windows.Controls.TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ImagedConnectionTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ImagedConnectionTreeViewItem;
        }
    }
}
