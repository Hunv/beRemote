using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Threading;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.Controls.TreeView;
using beRemote.Core.Common.Helper;
using beRemote.GUI.Controls.ViewModel;
using Hardcodet.Wpf.GenericTreeView;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Media3D;


namespace beRemote.GUI.Controls
{
    /// <summary>
    /// Interaction logic for beTreeView.xaml
    /// </summary>
    public partial class beTreeView :  INotifyPropertyChanged
    {
        public beTreeView()
        {
            InitializeComponent();
        }

        #region Private Internal Variables
        private IEnumerable<ConnectionItem> _FullConnectionTree; //If filters are applied, the full tree will be stored here
        private string _PreFilterState = ""; //Saves the State of the control before filters were applyed

        private ConnectionModel _ConList = new ConnectionModel(); //the ConnectionList
        #endregion

        #region Events
        public delegate void AddFolderEventHandler(object sender, RoutedEventArgs e);
        public event AddFolderEventHandler AddFolderTriggered;

        public delegate void AddConnectionEventHandler(object sender, RoutedEventArgs e);
        public event AddConnectionEventHandler AddConnectionTriggered;

        public delegate void DeleteItemEventHandler(object sender, RoutedEventArgs e);
        public event DeleteItemEventHandler DeleteItemTriggered;

        public delegate void EditItemEventHandler(object sender, RoutedEventArgs e);
        public event EditItemEventHandler EditItemTriggered;

        public delegate void SortUpItemEventHandler(object sender, RoutedEventArgs e);
        public event SortUpItemEventHandler SortUpItemTriggered;

        public delegate void SortDownItemEventHandler(object sender, RoutedEventArgs e);
        public event SortDownItemEventHandler SortDownItemTriggered;

        public delegate void AddFavoritesItemEventHandler(object sender, RoutedEventArgs e);
        public event AddFavoritesItemEventHandler AddFavoritesItemTriggered;

        public delegate void DoubleClickItemEventHandler(object sender, RoutedEventArgs e);
        public event DoubleClickItemEventHandler DoubleClickItemTriggered;

        public delegate void SetDefaultProtocolEventHandler(object sender, RoutedEventArgs e);
        public event SetDefaultProtocolEventHandler SetDefaultProtocolItemTriggered;

        public delegate void SetDefaultFolderEventHandler(object sender, RoutedEventArgs e);
        public event SetDefaultFolderEventHandler SetDefaultFolderTriggered;

        public delegate void ConnectWithoutCredentialsEventHandler(object sender, RoutedEventArgs e);
        public event ConnectWithoutCredentialsEventHandler ConnectWithoutCredentialsTriggered;

        //To Update the complete itemlsit at once
        public delegate void ItemsUpdateEventHandler(object sender, RoutedEventArgs e);
        public event ItemsUpdateEventHandler ItemsUpdateTriggered;

        //When an Item matches filter
        public delegate void FilterItemMatchEventHandler(object sender, RoutedEventArgs e);
        public event FilterItemMatchEventHandler FilterItemMatchTriggered;

        //When an Item was moved by Drag-n-Drop
        public delegate void DragDropEventHandler(object sender, beTreeViewDragDropEventArgs e);
        public event DragDropEventHandler DragDropMoved;

        //Trigger Scrolling, if there is a ScrollViewer around this control        
        public event MouseWheelEventHandler MouseWheelScroll;

        #endregion
        
        #region Public custom Methods
        /// <summary>
        /// Adds a new Item to the TreeView
        /// </summary>        
        /// <param name="newItem">The new Item itself</param>
        public void AddItem(ConnectionItem newItem)
        {
            if (newItem.ConnectionParent != null) //If it is not a root-Element
            {
                foreach (ConnectionItem conIt in tvMain.Items)
                {
                    //Get the whole Branch of the tree up to the root folder include the new Item
                    ConnectionItem workItem = addItemHelper(conIt, newItem) ;
                    if (workItem != null)
                    {
                        //Add the new Item
                        tvMain.GetParentItem(newItem).SubConnections.Add(newItem);              
                        break;
                    }
                }
            }
            else //It is a root-Element
            {
                tvMain.Items = tvMain.Items.Select(cat => cat).Concat(Enumerable.Repeat(newItem, 1));
                //ConList.Connections = new ObservableCollection<ConnectionItem>(tvMain.Items.Select(cat => cat).Concat(Enumerable.Repeat(newItem, 1)));
                tvMain.Refresh(tvMain.GetTreeLayout());
            }

            try
            {
                //make sure the parent is expanded
                tvMain.TryFindNode(newItem.ConnectionParent).IsExpanded = true;
            }
            catch { }
        }

        /// <summary>
        /// Sets the whole tree
        /// </summary>
        /// <param name="conTree"></param>
        public void SetItems(IEnumerable<ConnectionItem> conTree)
        {
            _FullConnectionTree = null; //Reset cached full tree (used for filtering)
            //tvMain.Items = conTree;
            //ConList.Connections = new ObservableCollection<ConnectionItem>(conTree);
            Items = new ObservableCollection<ConnectionItem>(conTree);            
        }

        /// <summary>
        /// Removes an existing Item
        /// </summary>
        /// <param name="remItem">The Item to remove</param>
        public void RemoveItem(ConnectionItem remItem)
        {
            foreach (ConnectionItem conItem in tvMain.Items)
            {
                if (conItem == remItem) //Remove root-Item (if it should be removed)
                {
                    tvMain.Items = tvMain.Items.Except(Enumerable.Repeat(remItem, 1));
                }
                else if (removeItemHelper(tvMain.GetItemKey(remItem), conItem) == true)
                {
                    break;
                }
            }

            tvMain.Refresh(tvMain.GetTreeLayout()); //Refresh view
        }

        /// <summary>
        /// Helper for ItemRemove
        /// </summary>
        /// <param name="remItemKey">The Connectionitem-Key to remove</param>
        /// <param name="checkItem">The ConnectionItem where to check Subitems for Remove</param>
        /// <returns></returns>
        private bool removeItemHelper(string remItemKey, ConnectionItem checkItem)
        {           
            for (int i = 0; i < checkItem.SubConnections.Count; i++)
            {
                if (tvMain.GetItemKey(checkItem.SubConnections[i]) == remItemKey)
                {
                    checkItem.SubConnections.RemoveAt(i);
                    return true;
                }
                else
                {
                    if (removeItemHelper(remItemKey, checkItem.SubConnections[i]) == true)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the postion of an Item to another order
        /// </summary>
        /// <param name="srtItem">The Item to sort</param>
        /// <param name="position">The new Position in the current list</param>
        public void SortItem(ConnectionItem srtItem, int position)
        {
            foreach (ConnectionItem conIt in tvMain.Items)
            {
                if (sortItemHelper(tvMain.GetItemKey(srtItem), conIt, position) == true)
                    break;
            }

            tvMain.Refresh(tvMain.GetTreeLayout()); //Refresh view
        }

        /// <summary>
        /// Helper for sorting Items
        /// </summary>
        /// <param name="sortItemKey">The Key of the Item to Sort</param>
        /// <param name="checkItem">The currently checked Parent for the child</param>
        /// <param name="position">The new position of the Item</param>
        /// <returns></returns>
        private bool sortItemHelper(string sortItemKey, ConnectionItem checkItem, int position)
        {
            int folderDif = 0;

            for (int i = 0; i < checkItem.SubConnections.Count; i++)
            {
                if (checkItem.SubConnections[i].ConnectionType == ConnectionTypeItems.folder)
                    folderDif++;

                if (tvMain.GetItemKey(checkItem.SubConnections[i]) == sortItemKey)
                {
                    //Only sort, if there are more than 1 Items
                    if (checkItem.SubConnections.Count > 1)
                    {
                        try
                        {
                            //Resort Item
                            checkItem.SubConnections.Move(i, (position + folderDif > checkItem.SubConnections.Count ? checkItem.SubConnections.Count - 1 : position + folderDif));
                        }
                        catch (Exception){}
                    }
                    return true;
                }
                else
                {
                    if (sortItemHelper(sortItemKey, checkItem.SubConnections[i], position) == true)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the Node of a ConnectionItem as a TreeViewItem
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreeViewItem TryFindNode(ConnectionItem node)
        {
            TreeViewItem ret = tvMain.TryFindNode(node);
            return (ret);
        }

        /// <summary>
        /// Save the state
        /// </summary>
        /// <returns>Semicolon seperated expanded NodeIds</returns>
        public string SaveState()
        {
            var ret = "";

            var tl = tvMain.GetTreeLayout();
            foreach (var exNode in tl.ExpandedNodeIds)
                ret += exNode + ";";

            State = ret;
            return (ret);            
        }

        /// <summary>
        /// Sets the Treestate and its expanded nodes
        /// </summary>
        /// <param name="state"></param>
        public void SetState(string state)
        {
            string[] arState = state.Split(';');

            TreeLayout tl = tvMain.GetTreeLayout();
            tl.ExpandedNodeIds.Clear(); //Collapse all Nodes

            //Expand Nodes, that should be expanded
            foreach (string exNode in arState)            
                tl.ExpandedNodeIds.Add(exNode);
            

            tvMain.Refresh(tl);
        }

        /// <summary>
        /// Get all Items, that are currently available in the TreeView
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ConnectionItem> GetItems()
        {
            return (tvMain.Items);
        }
        #endregion

        #region Command Bindings

        #region Commands
        public static readonly RoutedUICommand Edit = new RoutedUICommand("Edit", "Edit", typeof(beTreeView), new InputGestureCollection(){new KeyGesture(Key.F2)});
        public static readonly RoutedUICommand SortUp = new RoutedUICommand("Sort Up", "SortUp", typeof(beTreeView), new InputGestureCollection() { new KeyGesture(Key.Divide, ModifierKeys.Control), new KeyGesture(Key.OemMinus, ModifierKeys.Control) });
        public static readonly RoutedUICommand SortDown = new RoutedUICommand("Sort Down", "SortDown", typeof(beTreeView), new InputGestureCollection() { new KeyGesture(Key.Multiply, ModifierKeys.Control), new KeyGesture(Key.OemPlus, ModifierKeys.Control) });
        public static readonly RoutedUICommand AddToFav = new RoutedUICommand("Add to Favorites", "AddToFav", typeof(beTreeView), new InputGestureCollection() { new KeyGesture(Key.D, ModifierKeys.Control) });
        public static readonly RoutedUICommand DefaultProtocol = new RoutedUICommand("Set to default Protocol", "DefaultProtocol", typeof(beTreeView), new InputGestureCollection() { new KeyGesture(Key.F6) });
        public static readonly RoutedUICommand DefaultFolder = new RoutedUICommand("Set to default Folder", "DefaultFolder", typeof(beTreeView), new InputGestureCollection() { new KeyGesture(Key.F7) });
        public static readonly RoutedUICommand ConnectWithoutCredentials = new RoutedUICommand("Connect without Credentials", "ConnectWithoutCredentials", typeof(beTreeView), new InputGestureCollection() { new KeyGesture(Key.F8) });
        #endregion

        #region Command Executes

        #region Edit
        /// <summary>
        /// Checks whether it is allowed to delete a category, which is only
        /// allowed for nested categories, but not the root items.
        /// </summary>
        private void EvaluateCanEdit(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ContextItem == null)
            {
                e.CanExecute = false;
                return;
            }

            e.CanExecute = this.ContextItem.ConnectionType != ConnectionTypeItems.option;
            e.Handled = true;
        }

        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        private void EditItem(object sender, ExecutedRoutedEventArgs e)
        {
            //get the processed item
            ConnectionItem parent = this.ContextItem;

            //Important - mark the event as handled
            e.Handled = true;

            if (EditItemTriggered != null)
                EditItemTriggered(parent, null);
        }
        #endregion        
        #region SortUp
        /// <summary>
        /// Checks whether it is allowed to delete a category, which is only
        /// allowed for nested categories, but not the root items.
        /// </summary>
        private void EvaluateCanSortUp(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ContextItem == null)
            {
                e.CanExecute = false;
                return;
            }

            e.CanExecute = this.ContextItem.ConnectionType == ConnectionTypeItems.folder || this.ContextItem.ConnectionType == ConnectionTypeItems.connection;
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SortUpItem(object sender, ExecutedRoutedEventArgs e)
        {            
            //get the processed item
            ConnectionItem sortItem = this.ContextItem;

            if (sortItem.ConnectionParent != null) //if it is not a root element
            {
                //Get the real parent. Not the item that has only itself in the reference of the Parent
                ConnectionItem realParent = findItem(tvMain.GetItemKey(sortItem.ConnectionParent));

                //Only Sort, if it is not the first item   
                if (realParent.SubConnections.IndexOf(sortItem) > 0)
                {
                    realParent.SubConnections.Move(realParent.SubConnections.IndexOf(sortItem), realParent.SubConnections.IndexOf(sortItem) - 1);
                }
            }
            else
            {
                int count = 0;
                foreach (ConnectionItem conIt in tvMain.Items)
                {
                    if (conIt == sortItem)
                    {
                        tvMain.Items = Helper.MoveUp(tvMain.Items, count);
                        break;
                    }
                    count++;
                }
            }
            
            

            tvMain.Refresh(tvMain.GetTreeLayout());            

            //Set the Focus
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => tvMain.TryFindNode(sortItem).Focus()));
                        
            //Important - mark the event as handled
            e.Handled = true;

            if (SortUpItemTriggered != null)
                SortUpItemTriggered(sortItem, null);
        }
        #endregion
        #region SortDown

        /// <summary>
        /// Checks whether it is allowed to delete a category, which is only
        /// allowed for nested categories, but not the root items.
        /// </summary>
        private void EvaluateCanSortDown(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ContextItem == null)
            {
                e.CanExecute = false;
                return;
            }

            e.CanExecute = this.ContextItem.ConnectionType == ConnectionTypeItems.folder || this.ContextItem.ConnectionType == ConnectionTypeItems.connection;
            e.Handled = true;
        }

        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        private void SortDownItem(object sender, ExecutedRoutedEventArgs e)
        {
            //get the processed item
            ConnectionItem sortItem = this.ContextItem;

            if (sortItem.ConnectionParent != null)
            {
                //Get the real parent. Not the item that has only itself in the reference of the Parent
                ConnectionItem realParent = findItem(tvMain.GetItemKey(sortItem.ConnectionParent));


                //Only Sort, if it is not the first item
                if (realParent.SubConnections.IndexOf(sortItem) != realParent.SubConnections.Count - 1)
                {

                    realParent.SubConnections.Move(realParent.SubConnections.IndexOf(sortItem), realParent.SubConnections.IndexOf(sortItem) + 1);
                }
            }
            else
            {
                int count = 0;
                foreach (ConnectionItem conIt in tvMain.Items)
                {
                    if (conIt == sortItem)
                    {
                        tvMain.Items = Helper.MoveDown(tvMain.Items, count);
                        break;
                    }
                    count++;
                }
            }
            

            //Force LayoutRefresh to show change            
            tvMain.Refresh(tvMain.GetTreeLayout());

            //Set the Focus
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => tvMain.TryFindNode(sortItem).Focus()));

            //Important - mark the event as handled
            e.Handled = true;

            if (SortDownItemTriggered != null)
                SortDownItemTriggered(sortItem, null);
        }
        #endregion
        #region AddToFav
        /// <summary>
        /// Checks whether it is allowed to delete a category, which is only
        /// allowed for nested categories, but not the root items.
        /// </summary>
        private void EvaluateCanAddToFav(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ContextItem == null)
            {
                e.CanExecute = false;
                return;
            }

            e.CanExecute = this.ContextItem.ConnectionType == ConnectionTypeItems.protocol;
            e.Handled = true;
        }

        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        private void AddToFavItem(object sender, ExecutedRoutedEventArgs e)
        {
            //get the processed item
            ConnectionItem parent = this.ContextItem;

            //Important - mark the event as handled
            e.Handled = true;

            if (AddFavoritesItemTriggered != null)
                AddFavoritesItemTriggered(parent, null);
        }
        #endregion
        #region AddFolder
        /// <summary>
        /// Checks whether it is allowed to delete a category, which is only
        /// allowed for nested categories, but not the root items.
        /// </summary>
        private void EvaluateCanAddFolder(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ContextItem == null)
            {
                e.CanExecute = false;
                return;
            }

            e.CanExecute = this.ContextItem.ConnectionType == ConnectionTypeItems.folder;
            e.Handled = true;
        }

        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        private void AddFolder(object sender, ExecutedRoutedEventArgs e)
        {
            //get the processed item
            ConnectionItem parent = this.ContextItem;

            ////create a sub category
            //string name = "New Dir";
            //ConnectionItem subCategory = new ConnectionItem(name, parent);
            //parent.SubConnections.Add(subCategory);

            ////make sure the parent is expanded
            //tvMain.TryFindNode(parent).IsExpanded = true;
            
            //Important - mark the event as handled
            e.Handled = true;

            if (AddFolderTriggered != null)
                AddFolderTriggered(parent, null);
                //AddFolderTriggered(subCategory, null);
        }
        #endregion
        #region AddConnection
        /// <summary>
        /// Checks whether it is allowed to delete a category, which is only
        /// allowed for nested categories, but not the root items.
        /// </summary>
        private void EvaluateCanAddConnection(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ContextItem == null)
            {
                e.CanExecute = false;
                return;
            }

            e.CanExecute = this.ContextItem.ConnectionType != ConnectionTypeItems.protocol && this.ContextItem.ConnectionType != ConnectionTypeItems.option;
            e.Handled = true;
        }

        /// <summary>
        /// Adds a new Connection
        /// </summary>
        private void AddConnection(object sender, ExecutedRoutedEventArgs e)
        {
            //get the processed item
            ConnectionItem parent = this.ContextItem;

            //create a sub category
            //string name = "New Connection";
            //ConnectionItem subCategory = new ConnectionItem(name, ConnectionTypeItems.connection, parent);
            //parent.SubConnections.Add(subCategory);

            ////make sure the parent is expanded
            //tvMain.TryFindNode(parent).IsExpanded = true;

            //Important - mark the event as handled
            e.Handled = true;

            if (AddConnectionTriggered != null)
                AddConnectionTriggered(parent, null);
        }
        #endregion
        #region DeleteItem
        /// <summary>
        /// Checks whether it is allowed to delete a category, which is only
        /// allowed for nested categories, but not the root items.
        /// </summary>
        private void EvaluateCanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        
        /// <summary>
        /// Deletes the currently processed item. This can be a right-clicked
        /// item (context menu) or the currently selected item, if the user
        /// pressed delete.
        /// </summary>
        private void DeleteFolder(object sender, ExecutedRoutedEventArgs e)
        {
            //get item
            ConnectionItem item = this.ContextItem;

            //Don't remove really, because it has to be confimed first. Just send a Delete-Request
            //if (item.ConnectionParent != null)
            //    //remove from parent
            //    item.ConnectionParent.SubConnections.Remove(item);
            //else
            //    removeRootItem(item);

            //mark event as handled
            e.Handled = true;

            if (DeleteItemTriggered != null)
                DeleteItemTriggered(item, null);
        }
        #endregion
        #region Doubleclick
        private void EvaluateCanDoubleClick(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        private void DoubleClickItem(object sender, MouseButtonEventArgs e)
        {
            //get the processed item
            var menu = tvMain.NodeContextMenu;
            if (menu.IsVisible)
            {
                //a context menu was clicked
                var treeNode = (TreeViewItem)menu.PlacementTarget;
                ContextItem = (ConnectionItem)treeNode.Header;
            }
            else
            {
                //the context menu is closed - the user has pressed a shortcut
                ContextItem = tvMain.SelectedItem;
            }

            if (ContextItem == null || //If no Item is selected
                IsMouseOverFreeSpace(sender, e.GetPosition(sender as IInputElement)) ||//If a doubleclick is performed in a free space of the List 
                IsMouseOverScrollbar(sender, e.GetPosition(sender as IInputElement))) //If doubleclick was performed on a scrollbar
            {
                e.Handled = true;

                if (DoubleClickItemTriggered != null)
                    DoubleClickItemTriggered(null, null);

                return;
            }
            

            //get the processed item
            ConnectionItem parent = this.ContextItem;

            //Don't expand connections on Doubleclick
            if (parent.ConnectionType == ConnectionTypeItems.connection)
                tvMain.TryFindNode(parent).IsExpanded = false;

            //Important - mark the event as handled
            e.Handled = true;

            if (DoubleClickItemTriggered != null)
                DoubleClickItemTriggered(parent, null);
        }

        public static DependencyProperty DoubleClickCommandProperty
        = DependencyProperty.Register(
            "DoubleClickCommand",
            typeof(ICommand),
            typeof(beTreeView));

        public ICommand DoubleClickCommand
        {
            get
            {
                return (ICommand)GetValue(DoubleClickCommandProperty);
            }

            set
            {
                SetValue(DoubleClickCommandProperty, value);
            }
        }

        #endregion
        #region Default Protocol
        private void EvaluateCanDefaultProtocol(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ContextItem == null)
            {
                e.CanExecute = false;
                return;
            }

            e.CanExecute = this.ContextItem.ConnectionType == ConnectionTypeItems.protocol;
            e.Handled = true;
        }

        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        private void SetDefaultProtocol(object sender, ExecutedRoutedEventArgs e)
        {
            //get the processed item
            ConnectionItem parent = this.ContextItem;

            //Important - mark the event as handled
            e.Handled = true;

            if (SetDefaultProtocolItemTriggered != null)
                SetDefaultProtocolItemTriggered(parent, null);
        }
        #endregion
        #region Default Folder
        private void EvaluateCanDefaultFolder(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ContextItem == null)
            {
                e.CanExecute = false;
                return;
            }

            e.CanExecute = this.ContextItem.ConnectionType == ConnectionTypeItems.folder;
            e.Handled = true;
        }

        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        private void SetDefaultFolder(object sender, ExecutedRoutedEventArgs e)
        {
            //get the processed item
            ConnectionItem parent = this.ContextItem;

            //Important - mark the event as handled
            e.Handled = true;

            if (SetDefaultFolderTriggered != null)
                SetDefaultFolderTriggered(parent, null);
        }
        #endregion
        #region ConnectWithoutCredentials
        private void EvaluateCanConnectWithoutCredentials(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ContextItem == null)
            {
                e.CanExecute = false;
                return;
            }


            if (this.ContextItem.ConnectionType == ConnectionTypeItems.connection ||
                this.ContextItem.ConnectionType == ConnectionTypeItems.protocol)
                e.CanExecute = true;
            e.Handled = true;
        }

        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        private void ConnectWithoutCredentialsItem(object sender, ExecutedRoutedEventArgs e)
        {
            //get the processed item
            ConnectionItem parent = this.ContextItem;
            
            //Important - mark the event as handled
            e.Handled = true;

            if (ConnectWithoutCredentialsTriggered != null)
                ConnectWithoutCredentialsTriggered(parent, null);
        }
        #endregion

        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets the currently selected Item
        /// </summary>
        public ConnectionItem SelectedItem
        {
            get
            {
                return (tvMain.SelectedItem);
            }
            set
            {
                tvMain.SelectedItem = value;
            }
        }

        #region ContextItem
        public static readonly DependencyProperty ContextItemProperty =
            DependencyProperty.Register(
                "ContextItem",
                typeof(ConnectionItem),
                typeof(beTreeView)
                );
        
        /// <summary>
        /// Gets the Item, that is right-clicked, if not right clicked, selected
        /// </summary>
        /// <returns></returns>
        public ConnectionItem ContextItem
        {
            get
            {
                return ((ConnectionItem)GetValue(ContextItemProperty));
            }
            set
            {
                SetValue(ContextItemProperty, value);
            }
        }
        #endregion

        #region State

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(
                "State",
                typeof (string),
                typeof (beTreeView),
                new FrameworkPropertyMetadata("", OnStateChanged)
                );

        /// <summary>
        /// items, that are expanded
        /// </summary>
        /// <returns></returns>
        public string State
        {
            get
            {
                return ((string)GetValue(StateProperty));
            }
            set
            {
                SetValue(StateProperty, value);
            }
        }

        #region Event

        public delegate void StateChangedEventHandler(object sender, RoutedEventArgs e);

        public event StateChangedEventHandler StateChanged;

        public static void OnStateChanged(DependencyObject doj, DependencyPropertyChangedEventArgs dp)
        {
            var newVal = (beTreeView)doj;

            newVal.SetState(dp.NewValue.ToString());
        }
        #endregion
        #endregion

        #region Item-Property
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                "Items",
                typeof(ObservableCollection<ConnectionItem>),
                typeof(beTreeView),
                new PropertyMetadata(new ObservableCollection<ConnectionItem>(), new PropertyChangedCallback(OnItemsChanged))
                );
        
        /// <summary>
        /// Gets the current Items
        /// </summary>
        public ObservableCollection<ConnectionItem> Items
        {
            get
            {
                return ((ObservableCollection<ConnectionItem>)GetValue(ItemsProperty));
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }
        
        //Maybe for future use
        public static void OnItemsChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            //((beTreeView)target).tvMain.Refresh(((beTreeView)target).tvMain.GetTreeLayout());
        }

        #endregion
        #endregion

        #region Drag-n-Drop
        private Point _LastMouseDown;
        private bool _DragStartOnScrollbar = false;
        private ConnectionItem _DraggedItem, _Target;

        private void treeView_MouseDown (object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _LastMouseDown = e.GetPosition(tvMain);
            }
        }

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {   
            try
            {
                //IF the Mouse is over the Scrollbar
                if (IsMouseOverScrollbar(sender, e.GetPosition(sender as IInputElement)))
                {
                    _DragStartOnScrollbar = true;
                }
                else if (e.LeftButton == MouseButtonState.Pressed && _DragStartOnScrollbar == false) //If Left Mousbutton is down
                {
                    Point currentPosition = e.GetPosition(tvMain);


                    if ((Math.Abs(currentPosition.X - _LastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _LastMouseDown.Y) > 10.0))
                    {
                        _DraggedItem = tvMain.SelectedItem;

                        //of the Draggeditem is a scrollbar => null them
                        if (_DraggedItem.GetType() == typeof(ScrollBar))
                            _DraggedItem = null;

                        //If something is dragged
                        if (_DraggedItem != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(tvMain, tvMain.SelectedItem, DragDropEffects.Move);
                            //Checking target is not null and item is dragging(moving)
                            if ((finalDropEffect == DragDropEffects.Move) && (_Target != null))
                            {
                                // A Move drop was accepted
                                if (!_DraggedItem.Equals(_Target))
                                {
                                    CopyItem(_DraggedItem, _Target);
                                    _Target = null;
                                    _DraggedItem = null;
                                }
                            }
                        }                        
                    }
                }
                else if (e.LeftButton != MouseButtonState.Pressed && _DragStartOnScrollbar == true) //Reset state
                {
                    _DragStartOnScrollbar = false;
                }
            }
            catch (Exception)
            {
            }
        }
        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                Point currentPosition = e.GetPosition(tvMain);

                if ((Math.Abs(currentPosition.X - _LastMouseDown.X) > 10.0) ||
                    (Math.Abs(currentPosition.Y - _LastMouseDown.Y) > 10.0))
                {
                    // Verify that this is a valid drop and then store the drop target
                    TreeViewItem item = GetNearestContainer(e.OriginalSource as UIElement); ;
                    if (CheckDropTarget(tvMain.TryFindNode(_DraggedItem), item))
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                    }
                }
                e.Handled = true;
            }
            catch (Exception)
            {
            }
        }
        private void treeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                TreeViewItem TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (ContextItem != null && _DraggedItem != null)
                {
                    _Target = (ConnectionItem)TargetItem.Header;
                    e.Effects = DragDropEffects.Move;
                }
            }
            catch (Exception)
            {
            }
        }

        private bool CheckDropTarget(TreeViewItem _sourceItem, TreeViewItem _targetItem)
        {
            //Check whether the target item is meeting your condition
            bool _isEqual = false;
            if (!_sourceItem.Equals(_targetItem))
            {
                _isEqual = true;
            }
            return _isEqual;

        }

        /// <summary>
        /// Copies the dragged Item to the dropped Item
        /// </summary>
        /// <param name="_sourceItem"></param>
        /// <param name="_targetItem"></param>
        private void CopyItem(ConnectionItem _sourceItem, ConnectionItem _targetItem)
        {
            try
            {
                //Only move, if the source is a Folder or Host and Target is folder
                if (_sourceItem.ConnectionType == ConnectionTypeItems.option ||     //Don't allow Options
                    _sourceItem.ConnectionType == ConnectionTypeItems.protocol ||   //Don't allow Protocols
                    _targetItem.ConnectionType != ConnectionTypeItems.folder ||     //Target has to be folder
                    _sourceItem.ConnectionParent == _targetItem                    //No Drag'n'Drop in the same folder 
                    )
                    return;

                //Check if the droped Item was droped into a child
                if (dropedIntoChild(_sourceItem, _targetItem) == true)
                    return;

                //Disable observation
                tvMain.ObserveRootItems = false;

                //Remove old Node
                if (_sourceItem.ConnectionParent != null)
                {                    
                    _sourceItem.ConnectionParent.SubConnections.Remove(_sourceItem);                    
                }
                else
                {
                    removeRootItem(_sourceItem, false);                    
                }

                //Set the new Parent
                _sourceItem.ConnectionParent = _targetItem;

                //Add node at new parent
                _targetItem.SubConnections.Add(_sourceItem);
                
                //Enable observation
                tvMain.ObserveRootItems = true;

                //Force LayoutRefresh to show change
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => tvMain.Refresh(tvMain.GetTreeLayout())));
                
                if (DragDropMoved != null)
                {
                    beTreeViewDragDropEventArgs ddm = new beTreeViewDragDropEventArgs();
                    ddm.Source = _sourceItem;
                    ddm.Target = _targetItem;

                    this.DragDropMoved(null, ddm);
                }
            }
            catch
            {
            }
        }

        private TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }

        /// <summary>
        /// Checks if the Mouse is over a Scrollbar, so drag-n-drop should not working because the scrollbar is there, where it has to be...
        /// Fixes Issue #343
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        private static bool IsMouseOverScrollbar(object sender, Point mousePosition)
        {
            if (sender is Visual)
            {
                HitTestResult hit = VisualTreeHelper.HitTest(sender as Visual, mousePosition);

                if (hit == null) return false;

                DependencyObject dObj = hit.VisualHit;
                while (dObj != null)
                {
                    if (dObj is ScrollBar) return true;

                    if ((dObj is Visual) || (dObj is Visual3D)) dObj = VisualTreeHelper.GetParent(dObj);
                    else dObj = LogicalTreeHelper.GetParent(dObj);
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the Mouse is over a Scrollbar, so drag-n-drop should not working because the scrollbar is there, where it has to be...
        /// Fixes Issue #343
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        private static bool IsMouseOverFreeSpace(object sender, Point mousePosition)
        {
            if (sender is Visual)
            {
                HitTestResult hit = VisualTreeHelper.HitTest(sender as Visual, mousePosition);

                if (hit == null) return false;

                DependencyObject dObj = hit.VisualHit;
                
                if (dObj is Grid) return true; //If it is Grid, it is a free Space
            }

            return false;
        }

        /// <summary>
        /// Checks if an Item was droped into it's child, so it will not be accessible anymore
        /// </summary>
        /// <param name="_sourceItem"></param>
        /// <param name="_targetItem"></param>
        /// <returns></returns>
        private bool dropedIntoChild(ConnectionItem _sourceItem, ConnectionItem _targetItem)
        {
            foreach (ConnectionItem conItm in _sourceItem.SubConnections)
            {
                //Is the current Item the TargetItem? If so, a drop into the child was done.
                if (conItm.ConnectionID == _targetItem.ConnectionID && conItm.ConnectionType == _targetItem.ConnectionType)
                    return (true);
                else //It was not a child, so go on searching
                    if (dropedIntoChild(conItm, _targetItem) == true)
                        return (true);
            }

            return (false);
        }
        #endregion

        #region control Methods

        private void tvMain_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (MouseWheelScroll != null)
            {
                MouseWheelScroll(sender, e);
            }
        }

        #endregion

        #region Helper
        /// <summary>
        /// Removes a Node, that is in the first hyrarchy
        /// </summary>
        /// <param name="rootItem"></param>
        private void removeRootItem(ConnectionItem rootItem)
        {
            removeRootItem(rootItem, true);
        }

        /// <summary>
        /// Removes a Node, that is in the first hyrarchy
        /// </summary>
        /// <param name="rootItem"></param>
        /// <param name="disableObserveChildItems"></param>
        private void removeRootItem(ConnectionItem rootItem, bool disableObserveItems)
        {

            if (disableObserveItems) tvMain.ObserveRootItems = false;
            //ConList.Connections = new ObservableCollection<ConnectionItem>(tvMain.Items.Except(new ConnectionItem[1] { rootItem }));
            tvMain.Items = tvMain.Items.Except(new ConnectionItem[1] { rootItem });
            if (disableObserveItems) tvMain.ObserveRootItems = true;

            //Force LayoutRefresh to show change
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => tvMain.Refresh(tvMain.GetTreeLayout())));
        }

        /// <summary>
        /// Searches the parent of the new Item
        /// </summary>
        /// <param name="checkItem">The Item, where the new parent will be searched in</param>
        /// <param name="newItem">The Item to add</param>
        /// <returns></returns>
        private ConnectionItem addItemHelper(ConnectionItem checkItem, ConnectionItem newItem)
        {
            if (checkItem.ConnectionID == newItem.ConnectionParent.ConnectionID && checkItem.ConnectionType == newItem.ConnectionParent.ConnectionType)
            {
                checkItem.SubConnections.Add(newItem); //Add subconnection to current item
                return (checkItem); //this is the new items parent
            }
            else
            {
                ConnectionItem modifiedItem = null;

                foreach (ConnectionItem subItem in checkItem.SubConnections)
                {
                    ConnectionItem workItem = addItemHelper(subItem, newItem);
                    if (workItem != null) //A Subitem found the new Item parent
                    {
                        modifiedItem = workItem;
                        break;
                    }
                }

                if (modifiedItem != null)
                {
                    //Set the index of the modified item to the new value including new subconnection
                    checkItem.SubConnections[checkItem.SubConnections.IndexOf(modifiedItem)] = modifiedItem;

                    return (checkItem);
                }
            }

            return (null);//No Hit
        }

        /// <summary>
        /// Find an Item in the tree
        /// </summary>
        /// <param name="conType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private ConnectionItem findItem(string itemKey)
        {
            foreach (ConnectionItem conIt in tvMain.Items)
            {
                ConnectionItem result = findItemHelper(itemKey, conIt);
                if (result != null)
                    return (result);
            }
            return (null);
        }

        /// <summary>
        /// Helper for findItem
        /// </summary>
        /// <param name="conType"></param>
        /// <param name="id"></param>
        /// <param name="rootItem"></param>
        /// <returns></returns>
        private ConnectionItem findItemHelper(string itemKey, ConnectionItem rootItem)
        {
            if (tvMain.GetItemKey(rootItem) == itemKey)
            {
                return (rootItem);
            }

            foreach (ConnectionItem subIt in rootItem.SubConnections)
            {
                ConnectionItem result = findItemHelper(itemKey, subIt);
                if (result != null)
                    return (result);
            }
            return (null);
        }
        #endregion

        #region Filtering
        

        /// <summary>
        /// Filtering the whole tree
        /// </summary>
        /// <param name="filter">The Filter-Prase</param>
        public void Filter(string filter)
        {
            //Only filter, if there are at least 3 signs
            if (filter.Length < 3)
            {
                ClearFilter();
                return;
            }

            //Save the TreeView-View, that was configured before filtering
            if (_PreFilterState == "")
                _PreFilterState = SaveState();

            bool firstSelectionDone = false; //The first found Connection will be selected and set this to true

            //Check the current element, if it should be shown or not
            tvMain.ItemFilter = delegate(ConnectionItem obj)
            {
                //Always show all protocols of a connection
                if (obj.ConnectionType == ConnectionTypeItems.protocol)
                {
                    return (true);
                }

                //Collaps all connections, so no protocol is expanded
                if (obj.ConnectionType == ConnectionTypeItems.connection)
                {
                    TreeViewItem tvi = tvMain.TryFindNode(obj);
                    if (tvi != null)
                    {
                        tvi.IsExpanded = false;
                        if (firstSelectionDone == false)
                        {
                            tvi.IsSelected = true;
                            firstSelectionDone = true;
                        }
                    }
                }
                else if (obj.ConnectionType == ConnectionTypeItems.folder) //Expand folder
                {
                    TreeViewItem tvi = tvMain.TryFindNode(obj);
                    if (tvi != null)
                        tvi.IsExpanded = true;
                }
                    

                //Check other Connectiontypes (Folder, Connections)
                return (CheckFilter(obj, filter));
            };
        }

        /// <summary>
        /// Checks if an Item or a Subitem matches the given filter
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected bool CheckFilter(ConnectionItem obj, string filter)
        {
            //check if the item matches the filter
            if (obj.ConnectionName.ToLower().Contains(filter)) return true;

            //check child nodes
            foreach (ConnectionItem child in tvMain.GetChildItems(obj))
            {
                //Don't handle protocols here;
                if (child.ConnectionType == ConnectionTypeItems.protocol)
                    return (false);

                //recurse tree – if a child is visible, so is the parent
                bool status = CheckFilter(child, filter);
                if (status) return true;
            }

            //neither the item nor a child matches the filter
            return false;
        }

        public void ClearFilter()
        {
            if (tvMain.ItemFilter != null)
            {
                tvMain.ItemFilter = null;
                SetState(_PreFilterState);
                _PreFilterState = "";
            }
        }
        #endregion

        #region PropertyChanged
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void tvMain_SelectedItemChanged(object sender, RoutedTreeItemEventArgs<ConnectionItem> e)
        {
            SelectedItem = e.NewItem;
            ContextItem = e.NewItem;
            SaveState(); //Set the State-Property
        }

        public void OnItemRightClick(object sender, MouseButtonEventArgs e)
        {
            ((TreeViewItem) sender).IsSelected = true;            
        }
    }

    #region Converter
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //reverse conversion (false=>Visible, true=>collapsed) on any given parameter
            bool input = (null == parameter) ? (bool)value : !((bool)value);
            return (input) ? Visibility.Visible : Visibility.Collapsed;


            ConnectionItem itm = (ConnectionItem)value;
            int para = (int)parameter;

            if (itm.ConnectionType == ConnectionTypeItems.option && para >= 8)
                return (Visibility.Visible);
            else if (itm.ConnectionType == ConnectionTypeItems.protocol && para >= 4)
                return (Visibility.Visible);
            else if (itm.ConnectionType == ConnectionTypeItems.connection && para >= 2)
                return (Visibility.Visible);
            else if (itm.ConnectionType == ConnectionTypeItems.folder && para >= 1)
                return (Visibility.Visible);
            else
                return (Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    #endregion
}
