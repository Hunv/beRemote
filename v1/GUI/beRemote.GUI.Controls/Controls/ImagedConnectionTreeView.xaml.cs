using System;
using System.Collections.Generic;
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
using System.Globalization;
using beRemote.GUI.Controls.Classes;

namespace beRemote.GUI.Controls
{
    /// <summary>
    /// Interaction logic for ImagedTreeView.xaml
    /// </summary>
    public partial class ImagedConnectionTreeView : UserControl
    {
        private List<ImagedConnectionTreeViewItem> _Items;
        private Point _LastMouseDown;
        private ImagedConnectionTreeViewItem _DraggedItem, _Target;


        #region WPF-Command bindings
        //For Calling Methods from Treeview-Context-Menu
        public static readonly RoutedUICommand DeleteEntry = new RoutedUICommand("Delete Entry", "DeleteEntry", typeof(ImagedConnectionTreeView));
        public static readonly RoutedUICommand AddFolder = new RoutedUICommand("Add Folder", "AddFolder", typeof(ImagedConnectionTreeView));
        public static readonly RoutedUICommand AddSetting = new RoutedUICommand("Add Setting", "AddSetting", typeof(ImagedConnectionTreeView));
        public static readonly RoutedUICommand EditSetting = new RoutedUICommand("Edit Setting", "EditSetting", typeof(ImagedConnectionTreeView));
        public static readonly RoutedUICommand SetDefaultFolder = new RoutedUICommand("Set as default folder", "SetDefaultFolder", typeof(ImagedConnectionTreeView));
        public static readonly RoutedUICommand SetDefaultProtocol = new RoutedUICommand("Set as default protocol", "SetDefaultProtocol", typeof(ImagedConnectionTreeView));
        public static readonly RoutedUICommand Connect = new RoutedUICommand("Connect", "Connect", typeof(ImagedConnectionTreeView));
        public static readonly RoutedUICommand SortUp = new RoutedUICommand("SortUp", "SortUp", typeof(ImagedConnectionTreeView));
        public static readonly RoutedUICommand SortDown = new RoutedUICommand("SortDown", "SortDown", typeof(ImagedConnectionTreeView));
        public static readonly RoutedUICommand QuickConnectAdd = new RoutedUICommand("QuickConnectAdd", "QuickConnectAdd", typeof(ImagedConnectionTreeView));        
        #endregion

        #region Command Binding Methods
        private void tvDeleteEntryExecute(object sender, ExecutedRoutedEventArgs e)
        {
            //Trigger the Event
            if (this.DeleteClicked != null)
            {
                DeleteEventArgs eventArg = new DeleteEventArgs();
                eventArg.SelectedItem = (ImagedConnectionTreeViewItem) tvConnectionList.SelectedItem;
                this.DeleteClicked(sender, eventArg);
            }

            //Delete the Item from the Itemlist
            tvConnectionList.Items.Remove(sender);
            //loadConnectionList(); //old
        }
        private void tvDeleteEntryCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void tvAddFolderExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.AddFolderClicked != null)
            {
                AddFolderEventArgs eventArg = new AddFolderEventArgs();
                eventArg.SelectedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem;

                this.AddFolderClicked(sender, eventArg);
            }
        }
        private void tvAddFolderCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void tvAddSettingExecute(object sender, ExecutedRoutedEventArgs e)
        {
            //Trigger the Event
            if (this.AddConnectionClicked != null)
            {
                AddConnectionEventArgs eventArg = new AddConnectionEventArgs();
                eventArg.SelectedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem;
                this.AddConnectionClicked(sender, eventArg);
            }

            //addConnection(null);
        }
        private void tvAddSettingCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void tvEditSettingExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if ((ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue == null)
                return;

            //Trigger the Event
            if (this.EditClicked != null)
            {
                if (((ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem).Datatype == ImagedConnectionTreeViewDatatype.ConnectionHost) //Get the Defaultprotocol
                { 
                    
                }
                EditEventArgs eventArg = new EditEventArgs();
                eventArg.SelectedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem;
                this.EditClicked(sender, eventArg);
            }
        }
        private void tvEditSettingCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void tvSetDefaultFolderExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if ((ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue == null)
                return;

            //Trigger the Event
            if (this.SetDefaultFolderClicked != null)
            {
                SetDefaultFolderEventArgs eventArg = new SetDefaultFolderEventArgs();
                eventArg.SelectedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem;
                this.SetDefaultFolderClicked(sender, eventArg);
            }
                       
        }
        private void tvSetDefaultFolderCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void tvSetDefaultProtocolExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if ((TreeViewItem)tvConnectionList.SelectedValue == null)
                return;

            //Trigger the Event
            if (this.SetDefaultProtocolClicked != null)
            {
                SetDefaultProtocolEventArgs eventArg = new SetDefaultProtocolEventArgs();
                eventArg.SelectedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem;
                this.SetDefaultProtocolClicked(sender, eventArg);
            }
        }
        private void tvSetDefaultProtocolCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void tvSortUpExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if ((ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue == null)
                return;

            if (this.SortClicked != null)
            {
                SortEventArgs eventArg = new SortEventArgs();
                eventArg.SortedUp = true;
                eventArg.SelectedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue;
                
                this.SortClicked(sender, eventArg);
            }                     
        }
        private void tvSortUpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void tvSortDownExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if ((ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue == null)
                return;

            if (this.SortClicked != null)
            {
                SortEventArgs eventArg = new SortEventArgs();
                eventArg.SortedUp = false;
                eventArg.SelectedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue;
                this.SortClicked(sender, eventArg);
            }
        }
        private void tvSortDownCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void tvConnectExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if ((ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue == null)
                return;

            //Trigger the Event
            if (this.ConnectClicked != null)
            {
                ConnectTreeviewEventArgs eventArg = new ConnectTreeviewEventArgs();
                eventArg.SelectedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem;
                this.ConnectClicked(sender, eventArg);
            }
        }
        private void tvConnectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void tvQuickConnectAddExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if ((ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue == null)
                return;

            if (this.QuickConnectAddClicked != null)
            {
                QuickConnectAddEventArgs eventArg = new QuickConnectAddEventArgs();
                eventArg.SelectedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue;
                this.QuickConnectAddClicked(sender, eventArg);
            }
        }
        private void tvQuickConnectAddCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        

        #endregion

        #region constructor
        /// <summary>
        /// Create a FolderList
        /// </summary>
        public ImagedConnectionTreeView()
        {
            InitializeComponent();

            DataContext = this._Items;
        }

        /// <summary>
        /// Create a FolderList with the Possibility to show Connections and ConnectionSettings
        /// </summary>        
        public ImagedConnectionTreeView(List<ImagedConnectionTreeViewItem> Items)
        {
            InitializeComponent();
            DataContext = this._Items;

            _Items = Items;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The Items that are visible in the TreeView
        /// </summary>
        public List<ImagedConnectionTreeViewItem> Items
        {
            get { return (_Items); }
            set { setConnectionList(value); }
        }

        /// <summary>
        /// Show Contextmenus?
        /// </summary>
        public bool IsContextMenuEnabled
        {
            get { return tvConnectionList.ContextMenu.IsEnabled; }
            set { tvConnectionList.ContextMenu.IsEnabled = value; }
        }

        #endregion

        #region Methods
        public void setConnectionList(List<ImagedConnectionTreeViewItem> Items)
        {
            if (Items != _Items) //set the private variable
                _Items = Items;
            tvConnectionList.Items.Clear();

            foreach (ImagedConnectionTreeViewItem anItem in Items)
            {
                tvConnectionList.Items.Add(anItem);
            }
        }

        public ImagedConnectionTreeViewItem getSelectedValue()
        {
            return ((ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue);
        }

        public ImagedConnectionTreeViewItem SelectedValue
        {
            get { return ((ImagedConnectionTreeViewItem)tvConnectionList.SelectedValue); }
        }

        /// <summary>
        /// Gets the Collapsed-State of the Nodes
        /// </summary>
        /// <returns>saveState-String; load it by method loadState(string)</returns>
        public string saveState()
        {
            string savedState = "";

            foreach (ImagedConnectionTreeViewItem ictvi in tvConnectionList.Items)
            {
                savedState += saveStateHelper("", ictvi);
            }

            if (savedState.Length > 0)
                savedState = savedState.Substring(1); //Remove first sign (",")

            return (savedState);
        }

        private string saveStateHelper(string itemsSoFar, ImagedConnectionTreeViewItem currentItem)
        {
            if (currentItem.IsExpanded == true)
            {
                switch(currentItem.Datatype)
                {
                    case ImagedConnectionTreeViewDatatype.Folder:
                        itemsSoFar += ",f";
                        break;
                    case ImagedConnectionTreeViewDatatype.ConnectionHost:
                        itemsSoFar += ",h";
                        break;
                    case ImagedConnectionTreeViewDatatype.ConnectionProtocol:
                        itemsSoFar += ",p";
                        break;
                }

                itemsSoFar += currentItem.ID.ToString();
            }

            if (currentItem.HasItems == true)
            {
                foreach (ImagedConnectionTreeViewItem ictvi in currentItem.Items)
                {
                    itemsSoFar = saveStateHelper(itemsSoFar, ictvi);
                }
            }
            return (itemsSoFar);
        }

        /// <summary>
        /// Set the Collapsed-State of the Nodes by run with a stateString, created by getState()
        /// </summary>
        /// <param name="stateString"></param>
        public void loadState(string stateString)
        {
            //must have at last 2 signs
            if (stateString.Length < 2)
                return;

            //stateString i.e. = "f1,h3,p2"
            string[] expanded = stateString.Split(',');

            //Expand each ID
            foreach (string aState in expanded)
            {
                ImagedConnectionTreeViewItem toExpand = new ImagedConnectionTreeViewItem();
                toExpand.ID = Convert.ToInt32(aState.Substring(1));

                //Set Datatype
                switch (aState.Substring(0, 1))
                { 
                    case "f":
                        toExpand.Datatype = ImagedConnectionTreeViewDatatype.Folder;
                        break;
                    case "h":
                        toExpand.Datatype = ImagedConnectionTreeViewDatatype.ConnectionHost;
                        break;
                    case "p":
                        toExpand.Datatype = ImagedConnectionTreeViewDatatype.ConnectionProtocol;
                        break;
                }

                //Expand Node
                foreach (ImagedConnectionTreeViewItem itm in this.Items)
                {
                    expandNode(itm, toExpand);
                }
            }
        }

        /// <summary>
        /// Helper for LoadState
        /// </summary>
        /// <param name="item">Current Item of the Nodetree</param>
        /// <param name="toExpand">ItemID and Datatype of Item to Expand</param>
        /// <returns>Found Item/Or Not</returns>
        private bool expandNode(ImagedConnectionTreeViewItem item, ImagedConnectionTreeViewItem toExpand)
        {
            //Check current Item
            if (item.Datatype == toExpand.Datatype && item.ID == toExpand.ID)
            {
                item.IsExpanded = true;
                return (true);
            }
            else
            {
                //Check other Subitems
                foreach (ImagedConnectionTreeViewItem itm in item.Items)
                {
                    if (expandNode(itm, toExpand) == true)
                        return(true);
                }
            }
            return (false);
        }
        #endregion

        #region Triggered Events

        void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            //Included for future use
        }

        private void tvConnectionList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //Trigger the Event
            if (this.SelectedValueChanged != null)
            {             
                this.SelectedValueChanged(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Prevent the Opening of a Node on Doubleclick, if it was clicked on a ConnectioNHost
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (tvConnectionList.SelectedItem != null)
            { 
                //Cancel if it a connectionhost is selected
                if (((ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem).Datatype == ImagedConnectionTreeViewDatatype.ConnectionHost)                
                    ((ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem).IsExpanded = false;

                base.OnMouseDoubleClick(e);
            }
        }

        #endregion

        #region Custom Events
        //When "Add Folder" was clicked on the Contextmenu
        public delegate void AddFolderEventHandler(object sender, AddFolderEventArgs e);
        public event AddFolderEventHandler AddFolderClicked;

        //When "Add Connection" was clicked on the Contextmenu
        public delegate void AddConnectionEventHandler(object sender, AddConnectionEventArgs e);
        public event AddConnectionEventHandler AddConnectionClicked;

        //When "Edit" was clicked on the Contextmenu
        public delegate void EditItemEventHandler(object sender, EditEventArgs e);
        public event EditItemEventHandler EditClicked;

        //When "Delete" was clicked on the Contextmenu
        public delegate void DeleteItemEventHandler(object sender, DeleteEventArgs e);
        public event DeleteItemEventHandler DeleteClicked;

        //When "Set default folder" was clicked on the Contextmenu
        public delegate void SetDefaultFolderEventHandler(object sender, SetDefaultFolderEventArgs e);
        public event SetDefaultFolderEventHandler SetDefaultFolderClicked;

        //When "Set default protocol" was clicked on the Contextmenu
        public delegate void SetDefaultProtocolEventHandler(object sender, SetDefaultProtocolEventArgs e);
        public event SetDefaultProtocolEventHandler SetDefaultProtocolClicked;

        //When an Entry was DoubleClicked
        public delegate void DoubleClickEntryEventHandler(object sender, DoubleClickEntryEventArgs e);
        public event DoubleClickEntryEventHandler EntryDoubleClicked;

        //When an Entry was DoubleClicked
        public delegate void ConnectEventHandler(object sender, ConnectTreeviewEventArgs e);
        public event ConnectEventHandler ConnectClicked;

        //When an Item was moved by Drag-n-Drop
        public delegate void DragDropEventHandler(object sender, DragDropEventArgs e);
        public event DragDropEventHandler DragDropMoved;

        //When an Item was Sorted
        public delegate void SortEventHandler(object sender, SortEventArgs e);
        public event SortEventHandler SortClicked;
        
        //When a QuickConnect Added
        public delegate void QuickConnectAddEventHandler(object sender, QuickConnectAddEventArgs e);
        public event QuickConnectAddEventHandler QuickConnectAddClicked;

        

        //When the selected Value changed
        public event RoutedEventHandler SelectedValueChanged;

        #endregion

        private void Item_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {            
            if (e.ClickCount == 2)
            {
                if (this.EntryDoubleClicked != null)
                {
                    DoubleClickEntryEventArgs eventArg = new DoubleClickEntryEventArgs();

                    eventArg.SelectedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem;
                    this.EntryDoubleClicked(sender, eventArg);
                }
            }
        }

        #region Drag-n-Drop
        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(tvConnectionList);


                    if ((Math.Abs(currentPosition.X - _LastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _LastMouseDown.Y) > 10.0))
                    {
                        _DraggedItem = (ImagedConnectionTreeViewItem)tvConnectionList.SelectedItem;
                        if (_DraggedItem != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(tvConnectionList, tvConnectionList.SelectedValue,
                                DragDropEffects.Move);
                            //Checking target is not null and item is dragging(moving)
                            if ((finalDropEffect == DragDropEffects.Move) && (_Target != null))
                            {
                                // A Move drop was accepted
                                if (!_DraggedItem.Header.ToString().Equals(_Target.Header.ToString()))
                                {
                                    CopyItem(_DraggedItem, _Target);
                                    _Target = null;
                                    _DraggedItem = null;
                                }

                            }
                        }
                    }
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

                Point currentPosition = e.GetPosition(tvConnectionList);


                if ((Math.Abs(currentPosition.X - _LastMouseDown.X) > 10.0) ||
                    (Math.Abs(currentPosition.Y - _LastMouseDown.Y) > 10.0))
                {
                    // Verify that this is a valid drop and then store the drop target
                    ImagedConnectionTreeViewItem item = GetNearestContainer(e.OriginalSource as UIElement);
                    if (CheckDropTarget(_DraggedItem, item))
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
                ImagedConnectionTreeViewItem TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (TargetItem != null && _DraggedItem != null)
                {
                    _Target = TargetItem;
                    e.Effects = DragDropEffects.Move;

                }
            }
            catch (Exception)
            {
            }
        }

        private bool CheckDropTarget(ImagedConnectionTreeViewItem _sourceItem, ImagedConnectionTreeViewItem _targetItem)
        {
            //Check whether the target item is meeting your condition
            bool _isEqual = false;
            if (!_sourceItem.Header.ToString().Equals(_targetItem.Header.ToString()))
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
        private void CopyItem(ImagedConnectionTreeViewItem _sourceItem, ImagedConnectionTreeViewItem _targetItem)
        {
            try
            {
                //Only move, if the source is a Folder or Host and Target is only folder
                if (_sourceItem.Datatype == ImagedConnectionTreeViewDatatype.ConnectionProtocol ||
                    _sourceItem.Datatype == ImagedConnectionTreeViewDatatype.ConnectionProtocolOption ||
                    _targetItem.Datatype != ImagedConnectionTreeViewDatatype.Folder
                    )
                    return;

                    
                if (DragDropMoved != null)
                {
                    DragDropEventArgs ddm = new DragDropEventArgs();
                    ddm.SelectedItem = _sourceItem;
                    ddm.Target = _targetItem;

                    this.DragDropMoved(null, ddm);
                }
            }
            catch
            {

            }
        }
              
        private ImagedConnectionTreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            ImagedConnectionTreeViewItem container = element as ImagedConnectionTreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as ImagedConnectionTreeViewItem;
            }
            return container;
        }
        #endregion

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.OemPlus || e.Key == Key.OemMinus ||
                    e.Key == Key.Add || e.Key == Key.Subtract)
                {
                    if (e.Key == Key.OemPlus || e.Key == Key.Add)
                        tvSortUpExecute(sender, null);
                    else
                        tvSortDownExecute(sender, null);

                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (tvConnectionList.Items.Count > 0 && tvConnectionList.SelectedValue == null)
            {
                ((ImagedConnectionTreeViewItem)tvConnectionList.Items[0]).IsSelected = true;
            }
        }

        //Redirect Scrolling to Scrollviewer
        private void tvConnectionList_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) //up
            {
                for (int i = 0; i < e.Delta/10; i++)
                    svScroll.LineUp();
            }
            else //down
            {
                for (int i = e.Delta/10; i < 0; i++)
                    svScroll.LineDown();
            }
        }        
    }
}
