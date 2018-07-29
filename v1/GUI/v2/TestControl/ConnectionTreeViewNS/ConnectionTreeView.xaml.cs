using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using beRemote.Core.Definitions;
using beRemote.GUI.Control.ConnectionTreeViewNS.ViewModel;

namespace beRemote.GUI.Control.ConnectionTreeViewNS
{
    public partial class ConnectionTreeView
    {
        public ConnectionTreeView()
        {
            InitializeComponent();
        }

        #region Bindable Properties
        #region Item-Property
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                "Items",
                typeof(ObservableCollection<ConnectionItem>),
                typeof(ConnectionTreeView)
                //,new PropertyMetadata(new ObservableCollection<string>())
                );

        /// <summary>
        /// Gets the current Items
        /// </summary>
        public ObservableCollection<ConnectionItem> Items
        {
            get { return (ObservableCollection<ConnectionItem>)GetValue(ItemsProperty); }
            set{SetValue(ItemsProperty, value);}
        }

        #endregion
        #endregion
    }
}
