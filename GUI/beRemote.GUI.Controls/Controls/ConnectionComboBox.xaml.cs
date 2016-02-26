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
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Classes;
using beRemote.Core;

namespace beRemote.GUI.Controls
{
    /// <summary>
    /// Interaction logic for ConnectionComboBox.xaml
    /// </summary>
    public partial class ConnectionComboBox : UserControl
    {
        #region Public EventHandler
        public event RoutedEventHandler SelectedValueChanged; //When the Value chagned
        public event RoutedEventHandler MenuExpanded; //When the menu is shown
        public event RoutedEventHandler MenuContract; //When the menu is hidden
        #endregion

        private bool _ShowHosts = false; //Should Hosts be shown?
        private bool _ShowProtocols = false; //Should Protocols be shown?
        private bool _MenuIsVisible = false; //Is the menu currently visible?
        private ImagedConnectionTreeViewItem _SelectedValue; //Contains the currently selected value
        private bool _DontHide = false; //Don't Hide the Dropdown, when the Mouse is over it
        private bool _AllowFolderSelection = true; //Is a folder a valid selection?
        private bool _AllowHostSelection = true; //Is a Host a valid selection?
        private bool _AllowProtocolSelection = true; //Is a Protocol a valid selection?
        private bool _HasValidSelection = false; //is the selection valid?
        private Brush _InvalidBackgroundColor = Brushes.Red; //The Backgroundcolor for btnCmb if selected value is invalid
        private Brush btnCmdDefaultBackground; //The Backgroundcolor for btnCmb if selected value is valid; is setted on loading
        private bool _HidePublicFolder = false; //Hide public folders?
        private int _MaxHeight = 200; //The maximum height of the popup

        public ConnectionComboBox()
        {
            InitializeComponent();
            btnCmdDefaultBackground = btnCmb.Background;
        }

        private bool selectValue(ImagedConnectionTreeViewDatatype datatype, long id, ImagedConnectionTreeViewItem parent)
        {
            if (parent.Datatype == datatype && parent.ID == id)
            {
                parent.IsSelected = true;
                ValidateValue(parent);
                return (true);
            }

            bool valueChanged = false;
            foreach (ImagedConnectionTreeViewItem anItem in parent.Items)
            {
                valueChanged = selectValue(datatype, id, anItem);

                if (valueChanged == true)                
                    return (true);
            }   

            return (valueChanged);
        }

        #region Properties

        /// <summary>
        /// Get/Set the currently selected value
        /// </summary>
        public ImagedConnectionTreeViewItem SelectedValue
        {
            get { return (_SelectedValue); }
            set
            {
                _SelectedValue = value;

                if (value == null)
                {
                    //btnCmb.Content = "";
                    lblText.Content = "";
                    return;
                }
                
                string buttonContent = "";
                switch (value.Datatype)
                {
                    case ImagedConnectionTreeViewDatatype.Folder:
                        Folder curFolder = StorageCore.Core.GetFolder(value.ID);
                        if (curFolder != null)
                            buttonContent = curFolder.getName();
                        break;
                    case ImagedConnectionTreeViewDatatype.ConnectionHost:
                        ConnectionHost conHost = StorageCore.Core.GetConnection(value.ID);
                        if (conHost != null)
                            buttonContent = conHost.Name;
                        break;
                    default:
                        buttonContent = "Unable to get Name";
                        break;
                }
                
                lblText.Content = buttonContent;
                imgIcon.Source = value.Icon;

                //Check if the selected Value is a valid Datatype of selection 
                if (tvTreeView.Items != null && tvTreeView.Items.Count > 0)
                {
                    var theValue = value;

                    //Set the new Value for Query
                    bool valueChanged = false;
                    foreach (ImagedConnectionTreeViewItem anItem in tvTreeView.Items)
                    {
                        valueChanged = selectValue(theValue.Datatype, theValue.ID, anItem);

                        if (valueChanged)
                            break;
                    }
                }

                //Trigger the Event
                if (SelectedValueChanged != null)
                    SelectedValueChanged(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Should Hosts be shown?
        /// </summary>
        public bool ShowHosts
        {
            get { return (_ShowHosts); }
            set { _ShowHosts = value; }
        }

        /// <summary>
        /// Should Protocols be shown?
        /// </summary>
        public bool ShowProtocols
        {
            get { return (_ShowProtocols); }
            set { _ShowProtocols = value; }
        }

        /// <summary>
        /// Is a Folder a valid selection?
        /// </summary>
        public bool AllowFolderSelection
        {
            get { return (_AllowFolderSelection); }
            set { _AllowFolderSelection = value; }
        }

        /// <summary>
        /// Is a host a valid selection?
        /// </summary>
        public bool AllowHostSelection
        {
            get { return (_AllowHostSelection); }
            set { _AllowHostSelection = value; }
        }

        /// <summary>
        /// Is a selected Protocol a valid selection? Maybe you should set ShowProtocols to false, if not?!
        /// </summary>
        public bool AllowProtocolSelection
        {
            get { return (_AllowProtocolSelection); }
            set { _AllowProtocolSelection = value; }
        }

        /// <summary>
        /// Get if the selected Value is Allowed
        /// </summary>
        public bool HasValidSelection
        {
            get { return (_HasValidSelection); }
        }

        /// <summary>
        /// Defines the Backgroundcolor, if the selected Value is not allowed
        /// </summary>
        public Brush InvalidBackgroundColor
        {
            get { return (_InvalidBackgroundColor); }
            set { _InvalidBackgroundColor = value; }
        }

        /// <summary>
        /// Should public folders be hidden?
        /// </summary>
        public bool HidePublicFolder
        {
            get { return (_HidePublicFolder); }
            set { _HidePublicFolder = value; }
        }

        /// <summary>
        /// The maximum height of the Popup; default = 200;
        /// </summary>
        public int MaximumHeight
        {
            get { return (_MaxHeight); }
            set { _MaxHeight = value; }
        }
        #endregion

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Configure Popup and load TreeView
            popUp.Placement = System.Windows.Controls.Primitives.PlacementMode.RelativePoint;
            popUp.VerticalOffset = btnCmb.Height;
            popUp.IsOpen = true;
            popUp.Width = btnCmb.Width;
            popUp.Height = MaximumHeight;
            tvTreeView.Items = loadFolderList();            
        }

        private List<ImagedConnectionTreeViewItem> loadFolderList()
        { 
            //Variable to feed the TreeView with Data
            List<ImagedConnectionTreeViewItem> tvList = new List<ImagedConnectionTreeViewItem>();

            //Folders in Root
            List<Folder> rootFolders = StorageCore.Core.GetSubfolders(0);

            //Fill the TreeView-Data-Variable
            foreach (Folder folder in rootFolders)
            {
                tvList.Add(getTreeViewFolderItems(folder.getId()));
            }

            return(tvList);
        }

        /// <summary>
        /// Get the content of a Folder
        /// </summary>
        /// <param name="folderId">id of the folder</param>
        /// <returns></returns>
        private ImagedConnectionTreeViewItem getTreeViewFolderItems(long folderId)
        {
            ImagedConnectionTreeViewItem ret = new ImagedConnectionTreeViewItem(ImagedConnectionTreeViewDatatype.Folder);
            ret.Datatype = ImagedConnectionTreeViewDatatype.Folder; //This is a Folder
            ret.ID = folderId; //The folderID of this folder is the folderId-Parameter

            //Get this Folderproperties
            Folder thisFolder = StorageCore.Core.GetFolder(folderId);

            ret.Header = thisFolder.getName(); //This Displayname of this folder

            #region Determinate the Rights of this Folder
            if (thisFolder.getIsPublic() == true) //If the Folder is a Public folder
            {
                if (thisFolder.getOwner() == StorageCore.Core.GetUserId()) //Is the User also the owner?
                {
                    ret.IsPrivate = ImagedConnectionTreeViewRight.PublicAndOwner;
                }
                else //User is not the owner
                {
                    ret.IsPrivate = ImagedConnectionTreeViewRight.Public;
                }
            }
            else
            {
                ret.IsPrivate = ImagedConnectionTreeViewRight.Private;
            }
            #endregion

            //Get Subfolders and its childs and add them to the subitems
            List<Folder> subFolders = StorageCore.Core.GetSubfolders(folderId);
            foreach (Folder folder in subFolders)
            {
                ret.Items.Add(getTreeViewFolderItems(folder.getId()));
            }

            if (_ShowHosts == true)
            {
                //Get Connections and its childs and add them to the subitems
                List<ConnectionHost> subHosts = StorageCore.Core.GetConnectionsInFolder(folderId);
                foreach (ConnectionHost conn in subHosts)
                {
                    ret.Items.Add(getTreeViewConnectionItems(conn.ID));
                }
            }

            return (ret);
        }

        /// <summary>
        /// Get the Content of a Connection
        /// </summary>
        /// <param name="connectionId">ID of the connection</param>
        /// <returns></returns>
        private ImagedConnectionTreeViewItem getTreeViewConnectionItems(long connectionId)
        {
            ImagedConnectionTreeViewItem ret = new ImagedConnectionTreeViewItem(ImagedConnectionTreeViewDatatype.ConnectionHost);

            ret.Datatype = ImagedConnectionTreeViewDatatype.ConnectionHost; //This is a ConnectionHost
            ret.ID = connectionId; //The connectionId of this folder is the connectionId-Parameter            

            //Get this Connection
            ConnectionHost thisConn = StorageCore.Core.GetConnection(connectionId);

            ret.Header = thisConn.Name; //This Displayname of this folder

            #region Determinate the Rights of this Connection
            if (thisConn.IsPublic == true && HidePublicFolder == false) //If the Connections is public and public connections are shown
            {
                if (thisConn.Owner == StorageCore.Core.GetUserId()) //Is the User also the owner?
                {
                    ret.IsPrivate = ImagedConnectionTreeViewRight.PublicAndOwner;
                }
                else //User is not the owner
                {
                    ret.IsPrivate = ImagedConnectionTreeViewRight.Public;
                }
            }
            else
            {
                ret.IsPrivate = ImagedConnectionTreeViewRight.Private;
            }
            #endregion

            #region Get containing Protocols
            List<ConnectionProtocol> protocols = StorageCore.Core.GetConnectionSettings(connectionId);
            if (_ShowProtocols == true)
            {                
                foreach (ConnectionProtocol prot in protocols)
                {
                    if (Kernel.GetAvailableProtocols().ContainsKey(prot.Protocol))
                    {
                        ImagedConnectionTreeViewItem newProt = new ImagedConnectionTreeViewItem(ImagedConnectionTreeViewDatatype.ConnectionProtocol);
                        //Get the Icon From Protocol
                        newProt.Icon = Kernel.GetAvailableProtocols()[prot.Protocol].GetProtocolIcon(Core.ProtocolSystem.ProtocolBase.Declaration.IconType.SMALL);
                        newProt.Datatype = ImagedConnectionTreeViewDatatype.ConnectionProtocol;
                        newProt.ID = prot.Id;
                        //Get the Displayname from Protocol
                        newProt.Header = Kernel.GetAvailableProtocols()[prot.Protocol].GetProtocolName();

                        ret.Items.Add(newProt);
                    }
                    else
                    {
                       
                    }
                }
            }
            #endregion

            #region Set Icon of the protocol used by this connection
            User userSettings = StorageCore.Core.GetUserSettings();
            bool foundDefaultProtocol = false;

            //Determinate the default Connection that is used at a doubleclick and set the icon
            foreach (ConnectionProtocol prot in protocols)
            {
                if (prot.Protocol == userSettings.DefaultProtocol)
                {
                    ret.Icon = Kernel.GetAvailableProtocols()[prot.Protocol].GetProtocolIcon(Core.ProtocolSystem.ProtocolBase.Declaration.IconType.SMALL);
                    foundDefaultProtocol = true;
                    break;
                }
            }

            if (foundDefaultProtocol == false)
            {
                if (protocols.Count > 0 && Kernel.GetAvailableProtocols().ContainsKey(protocols[0].Protocol))
                    ret.Icon = Kernel.GetAvailableProtocols()[protocols[0].Protocol].GetProtocolIcon(Core.ProtocolSystem.ProtocolBase.Declaration.IconType.SMALL);
                else
                    ret.Icon = new BitmapImage(new Uri("pack://application:,,,/beRemote.GUI;component/Images/screen16.png"));
            }

            #endregion

            return (ret);
        }

        #region Public Events
        /// <summary>
        /// Receive SelectedValueChanged Event from ImagedConnectionTreeView
        /// Redirect it to the own SelectedValueChanged-Event to forward the Event to the hosting control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ictv_SelectedValueChanged (object sender, RoutedEventArgs e)
        {
            ValidateValue(((ImagedConnectionTreeView)sender).SelectedValue);
            this.SelectedValue = ((ImagedConnectionTreeView)sender).SelectedValue;
        }

        /// <summary>
        /// Checks, if a value is valid and Sets the IsValidSelection-Value
        /// </summary>
        /// <param name="valiValue"></param>
        public void ValidateValue(ImagedConnectionTreeViewItem valiValue)
        {
            //Check, if the selected Value is an Allowed selection
            switch (valiValue.Datatype)
            {
                case ImagedConnectionTreeViewDatatype.Folder:
                    if (_AllowFolderSelection == true)
                        _HasValidSelection = true;
                    else
                        _HasValidSelection = false;
                    break;
                case ImagedConnectionTreeViewDatatype.ConnectionHost:
                    if (_AllowHostSelection == true)
                        _HasValidSelection = true;
                    else
                        _HasValidSelection = false;
                    break;
                case ImagedConnectionTreeViewDatatype.ConnectionProtocol:
                    if (_AllowProtocolSelection == true)
                        _HasValidSelection = true;
                    else
                        _HasValidSelection = false;
                    break;
            }

            //If it is not allowed, Turn the Backcolor to alternative Color
            if (_HasValidSelection == false)
            {
                btnCmb.Background = _InvalidBackgroundColor;
            }
            else
            {
                if (btnCmdDefaultBackground != null) //Should never be null, but just to be sure
                    btnCmb.Background = btnCmdDefaultBackground;
            }
        }

        #endregion

        private void UserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            imgArrow.Visibility = (this.IsEnabled == true ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.SelectedValue = _SelectedValue;
            
            if (_SelectedValue != null)
                ValidateValue(_SelectedValue);
        }
    }
}
