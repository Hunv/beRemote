using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public abstract class BaseCommand : ICommand
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public abstract void Execute(object sender);

        public event EventHandler CanExecuteChanged;

        #region Methods

        /// <summary>
        /// Gets the given Icon as a BitmapFrame for an ImageSource
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private BitmapFrame GetIcon(string url)
        {
            //Load the public-overlay-Icon (small guy in the bottom right corner)
            var iconUri = new Uri(url, UriKind.RelativeOrAbsolute);
            var iconBitmap = BitmapFrame.Create(iconUri);
            iconBitmap.Freeze();
            return (iconBitmap);
        }

        #endregion

        #region Events

        #region ConnectionAdd

        public delegate void ConnectionAddEventHandler(object sender, AddTabEventArgs e);

        public event ConnectionAddEventHandler ConnectionAdd;

        protected virtual void OnConnectionAdd(AddTabEventArgs e)
        {
            var Handler = ConnectionAdd;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion

        #region ReloadFavorites

        public delegate void ReloadFavoritesEventHandler(object sender, ReloadFavoritesEventArgs e);

        public event ReloadFavoritesEventHandler ReloadFavorites;

        protected virtual void OnReloadFavorites(ReloadFavoritesEventArgs e)
        {
            var Handler = ReloadFavorites;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region AddTab

        public delegate void AddTabEventHandler(object sender, AddTabEventArgs e);

        public event AddTabEventHandler AddTab;

        protected virtual void OnAddTab(AddTabEventArgs e)
        {
            var Handler = AddTab;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
        
        #region Connect

        public delegate void ConnectEventHandler(object sender, ConnectEventArgs e);

        public event ConnectEventHandler Connect;

        protected virtual void OnConnectEvent(ConnectEventArgs e)
        {
            var Handler = Connect;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region ReloadConnectionList

        public delegate void ReloadConnectionListEventHandler(object sender, ReloadConnectionListEventArgs e);

        public event ReloadConnectionListEventHandler ReloadConnectionList;

        protected virtual void OnReloadConnectionList(ReloadConnectionListEventArgs e)
        {
            var Handler = ReloadConnectionList;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region ApplyFilter

        public delegate void ApplyFilterEventHandler(object sender, ConnectionFilterChangedEventArgs e);

        public event ApplyFilterEventHandler ApplyFilter;

        protected virtual void OnApplyFilter(ConnectionFilterChangedEventArgs e)
        {
            var Handler = ApplyFilter;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region CloseTab

        public delegate void CloseTabEventHandler(object sender, RoutedEventArgs e);

        public event CloseTabEventHandler CloseTab;

        protected virtual void OnCloseTab(RoutedEventArgs e)
        {
            OnCloseTab(this, e);
        }

        protected virtual void OnCloseTab(object sender, RoutedEventArgs e)
        {
            var Handler = CloseTab;
            if (Handler != null)
                Handler(sender, e);
        }
        #endregion

        #region FilterAdd

        public delegate void FilterAddEventHandler(object sender, FilterAddEventArgs e);

        public event FilterAddEventHandler FilterAdd;

        protected virtual void OnFilterAdd(FilterAddEventArgs e)
        {
            var Handler = FilterAdd;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion

        #region VpnAdd

        public delegate void VpnAddEventHandler(object sender, VpnAddEventArgs e);

        public event VpnAddEventHandler VpnAdd;

        protected virtual void OnVpnAdd(VpnAddEventArgs e)
        {
            var Handler = VpnAdd;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion

        #region VpnRemove

        public delegate void VpnRemoveEventHandler(object sender, RoutedEventArgs e);

        public event VpnRemoveEventHandler VpnRemove;

        protected virtual void OnVpnRemove(RoutedEventArgs e)
        {
            var Handler = VpnRemove;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion

        #region VpnSave

        public delegate void VpnSaveEventHandler(object sender, RoutedEventArgs e);

        public event VpnSaveEventHandler VpnSave;

        protected virtual void OnVpnSave(RoutedEventArgs e)
        {
            var Handler = VpnSave;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion

        #region VpnTest

        public delegate void VpnTestEventHandler(object sender, RoutedEventArgs e);

        public event VpnTestEventHandler VpnTest;

        protected virtual void OnVpnTest(RoutedEventArgs e)
        {
            var Handler = VpnTest;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion

        #endregion
    }
}
