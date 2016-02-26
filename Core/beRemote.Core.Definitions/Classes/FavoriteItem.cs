using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using beRemote.Core.Definitions.Enums;

namespace beRemote.Core.Definitions.Classes
{
    public class FavoriteItem
    {
        public ImageSource FavIconLarge { get; set; }
        public ImageSource FavIconSmall { get; set; }
        public ConnectionHost FavItemHost { get; set; }
        public ConnectionProtocol FavItemProtocol { get; set; }
        public FavoriteType FavType { get;set;}
    }
}
