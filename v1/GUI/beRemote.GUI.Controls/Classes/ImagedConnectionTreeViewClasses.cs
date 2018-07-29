using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using beRemote.Core.Definitions.Classes;

namespace beRemote.GUI.Controls.Classes
{
    public class ImagedConnectionTreeViewItem : TreeViewItem
    {
        private long _Id;
        private ImagedConnectionTreeViewDatatype _Datatype;
        private ImageSource _Icon = new BitmapImage(new Uri("pack://application:,,,/beRemote.GUI.Controls;component/Images/missing16.png"));
        private ImagedConnectionTreeViewRight _IsPrivate;
        private long _ParentId = 0;
        private long _SortOrder = 0;
        private string _Host = "";
        private string _Description = "";        

        #region Constructors
        public ImagedConnectionTreeViewItem()
        {
            this.ContextMenuOpening += new ContextMenuEventHandler(ImagedConnectionTreeViewItem_ContextMenuOpening);
        }

        public ImagedConnectionTreeViewItem(ImagedConnectionTreeViewDatatype datatype)
        {
            Datatype = datatype;
            this.ContextMenuOpening += new ContextMenuEventHandler(ImagedConnectionTreeViewItem_ContextMenuOpening);
        }

        public ImagedConnectionTreeViewItem(long id, ImagedConnectionTreeViewDatatype datatype)
        {
            ID = id;
            Datatype = datatype;
            this.ContextMenuOpening += new ContextMenuEventHandler(ImagedConnectionTreeViewItem_ContextMenuOpening);
        }

        public ImagedConnectionTreeViewItem(long id, ImagedConnectionTreeViewDatatype datatype, ImageSource icon, ImagedConnectionTreeViewRight isPrivate)
        {
            ID = id;
            Datatype = datatype;
            Icon = icon;
            IsPrivate = isPrivate;
            this.ContextMenuOpening += new ContextMenuEventHandler(ImagedConnectionTreeViewItem_ContextMenuOpening);
        }

        public ImagedConnectionTreeViewItem(long id, ImagedConnectionTreeViewDatatype datatype, ImageSource icon, ImagedConnectionTreeViewRight isPrivate, object header)
        {            
            ID = id;
            Datatype = datatype;
            Icon = icon;
            IsPrivate = isPrivate;
            Header = header;
            this.ContextMenuOpening += new ContextMenuEventHandler(ImagedConnectionTreeViewItem_ContextMenuOpening);
        }
        #endregion

        #region Properties
        public long ID { get { return (_Id); } set { _Id = value; } }
        public ImagedConnectionTreeViewDatatype Datatype
        {
            get { return (_Datatype); }
            set
            {
                _Datatype = value;

                //If it is a folder, the folder has the folder-Icon
                if (_Datatype == ImagedConnectionTreeViewDatatype.Folder)
                    Icon = new BitmapImage(new Uri("pack://application:,,,/beRemote.GUI.Controls;component/Images/folder16.png"));
            }
        }
        public ImageSource Icon{get { return (_Icon); }set { _Icon = value; }}
        public ImagedConnectionTreeViewRight IsPrivate { get { return (_IsPrivate); } set { _IsPrivate = value; } }
        public long ParentId
        {
            get { return _ParentId; }
            set
            {
                if (value < 0) _ParentId = 0;
                else _ParentId = value;
            }
        }
        public long SortOrder{get { return _SortOrder; }set { _SortOrder = value; }}
        public bool IsFolder { get { return (_Datatype == ImagedConnectionTreeViewDatatype.Folder ? true : false); } }
        public bool IsHost { get { return (_Datatype == ImagedConnectionTreeViewDatatype.ConnectionHost ? true : false); } }
        public bool IsProtocol { get { return (_Datatype == ImagedConnectionTreeViewDatatype.ConnectionProtocol ? true : false); } }
        public bool IsNoFolder { get { return (_Datatype == ImagedConnectionTreeViewDatatype.Folder ? false : true); } }
        public bool IsSortable 
        { 
            get 
            {
                if (_Datatype == ImagedConnectionTreeViewDatatype.ConnectionHost || _Datatype == ImagedConnectionTreeViewDatatype.Folder) 
                    return (true); 
                else 
                    return (false); 
            }
        }
        public string Description { get { return (_Description); } set { _Description = value; } }
        public string Host { get { return (_Host); } set { _Host = value; } }        

        #endregion

        #region Events
        void ImagedConnectionTreeViewItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (e.Source == sender)
                this.IsSelected = true;  
        }
        #endregion
        
        #region Customization for TreeView
        protected override System.Windows.DependencyObject GetContainerForItemOverride()
        {
            return new ImagedConnectionTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ImagedConnectionTreeViewItem;
        }
        #endregion
    }

    /// <summary>
    /// Types of ImagedConnectionTreeViewDatatype.
    /// </summary>
    public enum ImagedConnectionTreeViewDatatype
    {
        /// <summary>
        /// Folder.
        /// </summary>
        Folder,
        /// <summary>
        /// ConnectionHost.
        /// </summary>
        ConnectionHost,
        /// <summary>
        /// ConnectionProtocol.
        /// </summary>
        ConnectionProtocol,
        /// <summary>
        /// ConnectionProtocolOption.
        /// </summary>
        ConnectionProtocolOption,
    }

    /// <summary>
    /// Types of ImagedConnectionTreeViewDatatype.
    /// </summary>
    public enum ImagedConnectionTreeViewRight
    {
        /// <summary>
        /// A private Item.
        /// </summary>
        Private,
        /// <summary>
        /// A public Item.
        /// </summary>
        Public,
        /// <summary>
        /// A public Item and the current user is the owner.
        /// </summary>
        PublicAndOwner,
    }

    public class ImagedConnectionTreeViewEventArgs : RoutedEventArgs
    {
        private ImagedConnectionTreeViewItem _SelectedItem;

        public ImagedConnectionTreeViewItem SelectedItem { get { return _SelectedItem; } set { _SelectedItem = value; } }
    }

    public class AddFolderEventArgs : ImagedConnectionTreeViewEventArgs
    {
    }

    public class AddConnectionEventArgs : ImagedConnectionTreeViewEventArgs
    {
    }

    public class EditEventArgs : ImagedConnectionTreeViewEventArgs
    {
    }

    public class DeleteEventArgs : ImagedConnectionTreeViewEventArgs
    {
    }

    public class SetDefaultFolderEventArgs : ImagedConnectionTreeViewEventArgs
    {
    }

    public class SetDefaultProtocolEventArgs : ImagedConnectionTreeViewEventArgs
    {
    }

    public class ConnectTreeviewEventArgs : ImagedConnectionTreeViewEventArgs
    {
    }

    public class DoubleClickEntryEventArgs : ImagedConnectionTreeViewEventArgs
    {
        private ConnectionHost _Host;
        private ConnectionProtocol _Protocol;
        private ConnectionProtocolOption _Options;

        public ConnectionHost Host { get { return _Host; } set { _Host = value; } }
        public ConnectionProtocol Protocol { get { return _Protocol; } set { _Protocol = value; } }
        public ConnectionProtocolOption Options { get { return _Options; } set { _Options = value; } }
    }

    public class DragDropEventArgs : ImagedConnectionTreeViewEventArgs
    {
        private ImagedConnectionTreeViewItem _Target; //The Item the Connection was dropped on

        public ImagedConnectionTreeViewItem Target { get { return _Target; } set { _Target = value; } }
    }

    public class SortEventArgs : ImagedConnectionTreeViewEventArgs
    {
        private bool _SortedUp = true;

        public bool SortedUp { get { return (_SortedUp); } set { _SortedUp = value; } }
    }

    public class QuickConnectAddEventArgs : ImagedConnectionTreeViewEventArgs
    { }
}
