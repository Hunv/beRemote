using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.Tabs.ManageVpn.ViewModel.Commands
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



        #endregion

        #region Events
        
        #region AddVpn

        public delegate void AddVpnEventHandler(object sender, AddTabEventArgs e);

        public event AddVpnEventHandler AddVpn;

        protected virtual void OnAddVpn(AddTabEventArgs e)
        {
            var Handler = AddVpn;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion

        #region TabLoaded

        public delegate void TabLoadedEventHandler(object sender, RoutedEventArgs e);

        public event TabLoadedEventHandler TabLoaded;

        protected virtual void OnTabLoaded(RoutedEventArgs e)
        {
            var Handler = TabLoaded;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion

        #region BrowseFile

        public delegate void BrowseFileEventHandler(object sender, BrowseFileEventArgs e);

        public event BrowseFileEventHandler BrowseFile;

        protected virtual void OnBrowseFile(BrowseFileEventArgs e)
        {
            var Handler = BrowseFile;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion
        
        #endregion
    }
}
