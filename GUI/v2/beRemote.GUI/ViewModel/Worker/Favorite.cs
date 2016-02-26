using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.ViewModel.Worker
{
    public class Favorite
    {
        public static List<FavoriteItem> GetFavorites(string favoriteItems)
        {
            //Final return-Variable
            var retList = new List<FavoriteItem>();

            //Set the Favorites-Buttons
            var qcButtons = favoriteItems.Split(';');
            foreach (var qcBut in qcButtons)
            {
                if (qcBut == "") continue; //Prevent Errors (should never happen)

                var cp = StorageCore.Core.GetConnectionSetting(Convert.ToInt64(qcBut));

                if (cp == null) //Maybe it was deleted
                    continue;
                
                if (!Kernel.GetAvailableProtocols().ContainsKey(cp.getProtocol())) //If Protocol not exists on this system, skip the entry
                    continue;

                var ch = StorageCore.Core.GetConnection(cp.getConnectionId());

                var fI = new FavoriteItem();
                fI.FavItemHost = ch;
                fI.FavItemProtocol = cp;
                fI.FavIconSmall = Kernel.GetAvailableProtocols()[cp.getProtocol()].ProtocolIconSmall;
                fI.FavIconLarge = Kernel.GetAvailableProtocols()[cp.getProtocol()].ProtocolIconMedium;
                retList.Add(fI);
            }

            return(retList);
        }
    }
}
