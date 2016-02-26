using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using beRemote.Core.Definitions.EventArgs;

namespace beRemote.Core.Definitions.Classes
{
    public class StatusBarBase : UserControl, INotifyPropertyChanged
    {
        #region Events

        #region PropertyChanged
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region ConnectionListChanged-Event

        public void RefreshConnectionList()
        {
            OnConnectionListChanged(new RoutedEventArgs());
        }

        public delegate void ConnectionListChangedEventHandler(object sender, RoutedEventArgs e);

        public event TabBase.ConnectionListChangedEventHandler ConnectionListChanged;

        protected virtual void OnConnectionListChanged(RoutedEventArgs e)
        {
            var Handler = ConnectionListChanged;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region FavoritesChanged-Event

        public void RefreshFavorites(string favorites)
        {
            var evArgs = new FavoriteChangedEventArgs();
            evArgs.Favorites = favorites;

            OnFavoritesChanged(evArgs);
        }

        public delegate void FavoritesChangedEventHandler(object sender, FavoriteChangedEventArgs e);

        public event TabBase.FavoritesChangedEventHandler FavoritesChanged;

        protected virtual void OnFavoritesChanged(FavoriteChangedEventArgs e)
        {
            var Handler = FavoritesChanged;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region OnContextRibbonVisibileChange

        public void ChangeContextRibbon(List<string> show, List<string> hide)
        {
            var evArgs = new ContextRibbonVisibileChangeEventArgs();
            evArgs.ShowContextRibbon = show;
            evArgs.HideContextRibbon = hide;

            OnContextRibbonVisibileChange(evArgs);
        }

        public delegate void ContextRibbonVisibileChangeEventHandler(object sender, ContextRibbonVisibileChangeEventArgs e);

        public event TabBase.ContextRibbonVisibileChangeEventHandler ContextRibbonVisibileChange;

        protected virtual void OnContextRibbonVisibileChange(ContextRibbonVisibileChangeEventArgs e)
        {
            var Handler = ContextRibbonVisibileChange;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
        #endregion
    }
}
