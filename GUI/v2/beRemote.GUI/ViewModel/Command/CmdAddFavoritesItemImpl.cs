using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.Command;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdAddFavoritesItemImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            var evArg = new ReloadFavoritesEventArgs();
            evArg.OldFavorites = StorageCore.Core.GetUserVisuals().Favorites;

            var newFavorites = evArg.OldFavorites + ";" + ((ConnectionItem)sender).ConnectionID;
            newFavorites = newFavorites.Trim(new[] { ';' });

            var newVisuals = new Dictionary<string, object>();
            newVisuals.Add("favorites", newFavorites);
            StorageCore.Core.SetUserVisual(newVisuals);

            evArg.NewFavorites = newFavorites;
            evArg.AddedFavorite = ((ConnectionItem) sender).ConnectionID;

            OnReloadFavorites(evArg);
        }
    }
}
