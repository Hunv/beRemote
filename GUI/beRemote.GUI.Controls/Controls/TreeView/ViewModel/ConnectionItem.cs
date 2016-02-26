using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using beRemote.Core.Common.LogSystem;
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.Controls.Items
{
    /// <summary>
    /// Represents an article category.
    /// </summary>
    public class ConnectionItem : DependencyObject, INotifyPropertyChanged
    {
        private ObservableCollection<ConnectionItem> _SubConnections = new ObservableCollection<ConnectionItem>(); //The nested connections
        private string _ConnectionName = String.Empty; //The Displayname of the connection
        private ConnectionTypeItems _ConnectionType = ConnectionTypeItems.folder; //The Type of connection        
        private ImageSource _ConnectionIcon; //The Main-Icon of the connection (usually the protocol-icon or a folder-icon)
        private ImageSource _DisplayIcon; //The Icon in front of the Node
        private Collection<ImageSource> _ConnectionIconOverlays = new Collection<ImageSource>(); //Overlays for the ConnectionIcon
        private ConnectionItem _ConnectionParent; //The Parent-Node
        private long _ConnectionID; //The ID of the Node
        private string _ConnectionHost = "undefined"; //The Host that will be connected to (for Tooltip)
        private string _ConnectionDescription = ""; //The Description for the Connection (for Tooltip)
        private string _ConnectionCredentialName = ""; //the name of the CredentialSet-Name (for Tooltip)

        private Visibility _IsFiltered = Visibility.Visible; //Defines, if the Item is hidden in the ConnectionTree due a Filter
        private ImageSource _ConnectionIconRaw; //The Icon without any overlay
        

        #region Properties

        /// <summary>
        /// The Nodes that are Children of this Node
        /// </summary>
        public ObservableCollection<ConnectionItem> SubConnections
        {
            get { return _SubConnections; }
            set
            {
                _SubConnections = value;
                RaisePropertyChanged("SubConnections");
            }
        }

        /// <summary>
        /// The name of the category. This sample also used
        /// the name as the category key.
        /// </summary>
        public string ConnectionName
        {
            get { return _ConnectionName; }
            set { _ConnectionName = value; }
        }

        /// <summary>
        /// The host/ip of the category.
        /// </summary>
        public string ConnectionHost
        {
            get { return _ConnectionHost; }
            set { _ConnectionHost = value; }
        }

        /// <summary>
        /// The description of the category.
        /// </summary>
        public string ConnectionDescription
        {
            get { return _ConnectionDescription; }
            set { _ConnectionDescription = value; }
        }

        /// <summary>
        /// Has the ConnectionItem a description?
        /// </summary>
        public bool HasDescription
        {
            get 
            {
                if (ConnectionDescription.Length == 0)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// The description of the category.
        /// </summary>
        public string ConnectionCredentialName
        {
            get { return _ConnectionCredentialName; }
            set { _ConnectionCredentialName = value; }
        }

        /// <summary>
        /// Has the ConnectionItem credentials?
        /// </summary>
        public bool HasCredentials
        {
            get
            {
                if (ConnectionCredentialName.Length == 0)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// The Type of the Category
        /// </summary>
        public ConnectionTypeItems ConnectionType { get { return _ConnectionType; } set { _ConnectionType = value; } }

        /// <summary>
        /// The parent category, if any. If this is
        /// a top-level category, this property returns null.
        /// </summary>
        public ConnectionItem ConnectionParent
        {
            get { return _ConnectionParent; }
            set { _ConnectionParent = value; }
        }

        /// <summary>
        /// The ID of the Node
        /// </summary>
        public long ConnectionID
        {
            get { return _ConnectionID; }
            set { _ConnectionID = value; }
        }
        
        /// <summary>
        /// Is this Item hidden due a Filter?
        /// </summary>
        public Visibility IsFiltered
        {
            get { return _IsFiltered; }
            set { _IsFiltered = value; }
        }

        /// <summary>
        /// The Base-Icon in Front of the node
        /// </summary>
        public ImageSource ConnectionIcon
        {
            get { return (_ConnectionIcon); }
            set
            {
                value.Freeze();
                _ConnectionIcon = value;
                GenerateDisplayIcon();
            }
        }

        /// <summary>
        /// The Icon in Front of the node
        /// </summary>
        public ImageSource DisplayIcon
        {
            get { return (_DisplayIcon); }
            private set
            {
                _DisplayIcon = value;
                RaisePropertyChanged("DisplayIcon");
            }
        }

        /// <summary>
        /// List of all Overlays for the TreeView Icon; ConnectionIcon has to be specified first
        /// </summary>
        public Collection<ImageSource> ConnectionIconOverlays
        {
            get { return _ConnectionIconOverlays; }
            set
            {
                foreach (var iS in value)
                {
                    if (iS.IsFrozen == false)
                        iS.Freeze();
                }

                _ConnectionIconOverlays = value; 
                GenerateDisplayIcon();
            }
        }

        #region RootLevel
        public static readonly DependencyProperty RootLevelProperty =
            DependencyProperty.Register(
                "RootLevel",
                typeof(byte),
                typeof(ConnectionItem)
                );

        /// <summary>
        /// Contains the Level of this Item (How many parents has it?)
        /// </summary>
        /// <returns></returns>
        public byte RootLevel
        {
            get
            {
                return ((byte)GetValue(RootLevelProperty));
            }
            set
            {
                SetValue(RootLevelProperty, value);
            }
        }
        #endregion

        #region Visibility
        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register(
                "Visibility",
                typeof(Visibility),
                typeof(ConnectionItem)
                );

        /// <summary>
        /// Is the Item Visibile?
        /// </summary>
        /// <returns></returns>
        public Visibility Visibility
        {
            get
            {
                return ((Visibility)GetValue(VisibilityProperty));
            }
            set
            {
                SetValue(VisibilityProperty, value);
            }
        }
        #endregion

        #region IsCollapsed
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register(
                "IsCollapsed",
                typeof(bool),
                typeof(ConnectionItem)
                );

        /// <summary>
        /// Is the item Collapsed
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

        #endregion
        
        #region Constructor

        /// <summary>
        /// Creates a category without a parent.
        /// </summary>
        /// <param name="connectionName">The category's name.</param>
        public ConnectionItem(string connectionName)
            : this(connectionName, ConnectionTypeItems.folder, null)
        {
        }

        /// <summary>
        /// Creates a category without a parent.
        /// </summary>
        /// <param name="categoryName">The category's name.</param>
        public ConnectionItem(string connectionName, ConnectionTypeItems connectionType)
            : this(connectionName, connectionType, null)
        {
        }


        /// <summary>
        /// Creates a category for a given parent. This sets the
        /// <see cref="ConnectionParent"/> reference, but does not
        /// automatically add the category to the parent's
        /// <see cref="SubConnections"/> collection.
        /// </summary>
        /// <param name="categoryName">The category's name.</param>
        /// <param name="connectionParent">The parent category, if any.</param>
        public ConnectionItem(string connectionName, ConnectionItem connectionParent)
            : this(connectionName, ConnectionTypeItems.folder, connectionParent)
        { }

        public ConnectionItem(string connectionName, ConnectionTypeItems connectionType, ConnectionItem connectionParent)
            : this(connectionName, connectionType, connectionParent, null, null, 0, "", "", "")
        {}

        public ConnectionItem(string connectionName, ConnectionTypeItems connectionType, ConnectionItem connectionParent, BitmapImage connectionIcon)
            : this(connectionName, connectionType, connectionParent, connectionIcon, null, 0, "", "", "")
        { }

        /// <summary>
        /// Creates a category for a given parent. This sets the
        /// <see cref="ConnectionParent"/> reference, but does not
        /// automatically add the category to the parent's
        /// <see cref="SubConnections"/> collection.
        /// </summary>
        public ConnectionItem(string connectionName, ConnectionTypeItems connectionType, ConnectionItem connectionParent, ImageSource connectionIcon, Collection<ImageSource> connectionIconOverlay, long connectionId, string connectionHost, string connectionDescription, string connectionCredentialName)
        {
            _ConnectionName = connectionName;
            _ConnectionType = connectionType;
            _ConnectionParent = connectionParent;
            _ConnectionID = connectionId;
            ConnectionHost = connectionHost;
            ConnectionDescription = connectionDescription;
            ConnectionCredentialName = connectionCredentialName;

            if (connectionIconOverlay != null) ConnectionIconOverlays = connectionIconOverlay;

            //Set the default ConnectionIcon for the given type of connection if it is a connection or a protocol
            if (connectionIcon == null && connectionType != ConnectionTypeItems.option) 
            {
                try
                {
                    var icon = new BitmapImage();

                    icon.BeginInit();

                    switch (connectionType)
                    {
                        case ConnectionTypeItems.connection:
                            icon.UriSource = new Uri("pack://application:,,,/beRemote.GUI.Controls;component/Images/screen16.png", UriKind.Absolute);
                            break;
                        case ConnectionTypeItems.protocol:
                            icon.UriSource = new Uri("pack://application:,,,/beRemote.GUI.Controls;component/Images/missing16.png", UriKind.Absolute);
                            break;
                        case ConnectionTypeItems.folder:
                            icon.UriSource = new Uri("pack://application:,,,/beRemote.GUI.Controls;component/Images/folder16.png", UriKind.Absolute);
                            break;
                            //Not required; Loaded by UI and saves Cache
                            //case ConnectionTypeItems.option:
                            //    icon.UriSource = new Uri("pack://application:,,,/beRemote.GUI.Controls;component/Images/missing16.png", UriKind.Absolute);
                            //    break;
                    }

                    icon.EndInit();
                    icon.Freeze();

                    ConnectionIcon = icon;
                }
                catch (Exception ea)
                {
                    Logger.Log(LogEntryType.Exception, "Error on loading ConnectionIcon", ea);
                }
            }
            else
            {
                ConnectionIcon = connectionIcon;
            }
        }

        #endregion

        #region Helper

        /// <summary>
        /// Returns a string with the <see cref="ConnectionName"/>.
        /// </summary>
        /// <returns>The category name.</returns>
        public override string ToString()
        {
            return ConnectionType +": " + _ConnectionName;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Merges the Protocol-Icon with the overlays and returns the merged Icon
        /// </summary>
        private void GenerateDisplayIcon()
        {
            if (ConnectionIcon == null)
                return;

            if (ConnectionIconOverlays.Count == 0)
            {
                DisplayIcon = ConnectionIcon;
                return;
            }


            DrawingImage finalIcon = null;



            //Dispatcher.BeginInvoke(new Action(() =>
            Application.Current.Dispatcher.Invoke(() =>
                                                  {
                                                      var dGr = new DrawingGroup();

                                                      var newGroupItem = new ImageDrawing(ConnectionIcon, new Rect(0, 0, 16, 16));
                                                      newGroupItem.Freeze();

                                                      dGr.Children.Add(newGroupItem);

                                                      foreach (var anOverlay in ConnectionIconOverlays)
                                                      {
                                                          dGr.Children.Add(new ImageDrawing(anOverlay, new Rect(0, 0, 16, 16)));
                                                      }
                                                      dGr.Freeze();


                                                      finalIcon = new DrawingImage(dGr);
                                                      finalIcon.Freeze();
                                                  });
            DisplayIcon = finalIcon;

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
    }

    public enum ConnectionTypeItems
    {
        folder,
        connection,
        protocol,
        option
    }
}