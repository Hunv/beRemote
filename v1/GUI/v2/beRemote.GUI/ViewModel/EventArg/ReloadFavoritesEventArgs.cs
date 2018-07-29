using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class ReloadFavoritesEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Contains the complete new List of all Favorites
        /// </summary>
        public string NewFavorites { get; set; }

        /// <summary>
        /// Contains the complete old List of all Favorites
        /// </summary>
        public string OldFavorites { get; set; }

        /// <summary>
        /// Contains the new Favorite
        /// </summary>
        public long AddedFavorite { get; set; }

        /// <summary>
        /// Contains the removed Favorite
        /// </summary>
        public long RemovedFavorite { get; set; }

        /// <summary>
        /// Set to true, if all information should be ignore and the data will be reloaded from the Database
        /// </summary>
        public bool ReloadFromDatabase { get; set; }
    }
}
