using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.Core.Definitions.EventArgs
{
    public class FavoriteChangedEventArgs : RoutedEventArgs
    {
        public string Favorites { get; set; }
    }
}
