using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class ReloadConnectionListEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Contains the complete new List of all Favorites
        /// </summary>
        public List<ConnectionItem> NewConnections { get; set; }

        /// <summary>
        /// Contains the complete old List of all Favorites
        /// </summary>
        public List<ConnectionItem> OldConnections { get; set; }

        /// <summary>
        /// Contains the new Favorite
        /// </summary>
        public ConnectionItem AddedConnection { get; set; }

        /// <summary>
        /// Contains the removed Favorite
        /// </summary>
        public ConnectionItem RemovedConnection { get; set; }

        /// <summary>
        /// Set to true, if all information should be ignore and the data will be reloaded from the Database
        /// </summary>
        public bool ReloadFromDatabase { get; set; }
    }
}
