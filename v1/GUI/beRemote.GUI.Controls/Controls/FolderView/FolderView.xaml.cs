using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for FolderView.xaml
    /// </summary>
    public partial class FolderView : INotifyPropertyChanged
    {
        public FolderView()
        {
            InitializeComponent();
        }

        #region Properties
        #region ItemList
        public static readonly DependencyProperty ItemListProperty =
            DependencyProperty.Register(
                "ItemList",
                typeof(ObservableCollection<ConnectionItem>),
                typeof(FolderView)
                );

        /// <summary>
        /// Gets the Item, that is right-clicked, if not right clicked, selected
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ConnectionItem> ItemList
        {
            get
            {
                return ((ObservableCollection<ConnectionItem>)GetValue(ItemListProperty));
            }
            set
            {
                SetValue(ItemListProperty, value);
            }
        }
        #endregion

        #region SelectedValueProperty
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register(
                "SelectedValue",
                typeof(ConnectionItem),
                typeof(FolderView)
                );

        /// <summary>
        /// Gets the Item, that is right-clicked, if not right clicked, selected
        /// </summary>
        /// <returns></returns>
        public ConnectionItem SelectedValue
        {
            get
            {
                return ((ConnectionItem)GetValue(SelectedValueProperty));
            }
            set
            {
                SetValue(SelectedValueProperty, value);
            }
        }
        #endregion

        #region StartupValueProperty
        public static readonly DependencyProperty StartupValueProperty =
            DependencyProperty.Register(
                "StartupValue",
                typeof(ConnectionItem),
                typeof(FolderView)
                );

        /// <summary>
        /// Gets the Item, that is right-clicked, if not right clicked, selected
        /// </summary>
        /// <returns></returns>
        public ConnectionItem StartupValue
        {
            get
            {
                return ((ConnectionItem)GetValue(StartupValueProperty));
            }
            set
            {
                SetValue(StartupValueProperty, value);
            }
        }
        #endregion
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

        private void FolderListItemControl_IsCollapsedChanged(object sender, RoutedEventArgs e)
        {
            var senderId = ((FolderListItemControl) sender).Value.ConnectionID; //Contains the ID of the root-Item that collapses the childs
            var newState = ((FolderListItemControl) sender).IsCollapsed ? Visibility.Collapsed : Visibility.Visible; //Contains the new State of the childs
            var collapsingStarted = -1; //The ID of the item, that execs the collaps; 0 if there is no collapsing item in the list until this point
            var jumplevel = -1; //if there are collapsed items in a uncollapsing area, this items will be jumped
            
            //Check each item until there are no more changes expected
            foreach (var conItem in ItemList)
            {
                //Start of state-changing
                if (conItem.ConnectionID == senderId)
                {
                    collapsingStarted = conItem.RootLevel;
                    continue;
                }

                //No action, if no collapsing-Changes are estimated
                if (collapsingStarted == -1)
                    continue;
                
                //If it is a subitem of the collapsing-item, set new state
                if (collapsingStarted < conItem.RootLevel)
                {
                    //if state went to uncollapse and a child is still in collapse state: stay in this state
                    if (newState == Visibility.Visible)
                    {
                        //Childs of the underlying collapsed items
                        if (jumplevel != -1 && jumplevel < conItem.RootLevel)
                            continue;

                        //The "root" of underlying collapsed Items
                        if (conItem.IsCollapsed)
                        {
                            jumplevel = conItem.RootLevel;
                        }
                            //reset the jumplevel on next item on Rootlevel (if it is not collapsed too)
                        else if (jumplevel != -1 && jumplevel == conItem.RootLevel)
                            jumplevel = -1;
                    }
                    else //newState == Visibility.Collapsed
                    {
                        if (SelectedValue != null && conItem.ConnectionID == SelectedValue.ConnectionID)
                            SelectedValue = null;
                    }

                    conItem.Visibility = newState;
                }
                //if it is in the same level of the collapsing item, all actions are done
                else if (collapsingStarted >= conItem.RootLevel)
                {
                    collapsingStarted = -1;
                    break;
                }
            }
        }
    }
}
