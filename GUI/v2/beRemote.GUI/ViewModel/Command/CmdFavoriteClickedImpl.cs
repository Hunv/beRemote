using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core;
using beRemote.Core.Definitions.Classes;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdFavoriteClickedImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            //Connect
            var evArgs = new ConnectEventArgs();

            var cI = new ConnectionItem(((FavoriteItem) sender).FavItemHost.Name);
            cI.ConnectionType = ConnectionTypeItems.protocol;
            cI.ConnectionID = ((FavoriteItem) sender).FavItemProtocol.getId();
            cI.ConnectionIcon = Kernel.GetAvailableProtocols()[((FavoriteItem) sender).FavItemProtocol.getProtocol()].GetProtocolIcon(Core.ProtocolSystem.ProtocolBase.Declaration.IconType.SMALL);

            evArgs.ConnectionItem = cI;
            OnConnectEvent(evArgs);
        }
    }
}
