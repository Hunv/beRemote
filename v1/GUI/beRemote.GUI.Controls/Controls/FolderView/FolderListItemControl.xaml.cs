using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.Controls.FolderView
{
    /// <summary>
    /// Interaction logic for FolderListItem.xaml
    /// </summary>
    public partial class FolderListItemControl : INotifyPropertyChanged
    {
        public FolderListItemControl()
        {
            InitializeComponent();
        }

        #region Value
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(ConnectionItem),
                typeof(FolderListItemControl)
                );

        /// <summary>
        /// Gets the Item, that is right-clicked, if not right clicked, selected
        /// </summary>
        /// <returns></returns>
        public ConnectionItem Value
        {
            get
            {
                return ((ConnectionItem)GetValue(ValueProperty));
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        #endregion

        #region IsCollapsed
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register(
                "IsCollapsed",
                typeof(bool),
                typeof(FolderListItemControl)
                );

        /// <summary>
        /// Gets the Item, that is right-clicked, if not right clicked, selected
        /// </summary>
        /// <returns></returns>
        public bool IsCollapsed
        {
            get
            {
                return ((bool)GetValue(IsCollapsedProperty));
            }
            set
            {
                SetValue(IsCollapsedProperty, value);
            }
        }
        #endregion

        #region HasSubItems
        public static readonly DependencyProperty HasSubItemsProperty =
            DependencyProperty.Register(
                "HasSubItems",
                typeof(bool),
                typeof(FolderListItemControl)
                );

        /// <summary>
        /// Gets the Item, that is right-clicked, if not right clicked, selected
        /// </summary>
        /// <returns></returns>
        public bool HasSubItems
        {
            get
            {
                return ((bool)GetValue(HasSubItemsProperty));
            }
            set
            {
                SetValue(HasSubItemsProperty, value);
            }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsCollapsed = !IsCollapsed;
            RaisePropertyChanged("IsCollapsed");
            OnIsCollapsedChanged(new RoutedEventArgs());
        }

        #region Events

        public delegate void IsCollapsedChangedEventHandler(object sender, RoutedEventArgs e);

        public event IsCollapsedChangedEventHandler IsCollapsedChanged;

        protected virtual void OnIsCollapsedChanged(RoutedEventArgs e)
        {
            var Handler = IsCollapsedChanged;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        /// <summary>
        /// Helper for Triggering PropertyChanged
        /// </summary>
        /// <param name="triggerControl">The Name of the Property to update</param>
        private void RaisePropertyChanged(string triggerControl)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(triggerControl));
            }
        }
        #endregion
    }
}
