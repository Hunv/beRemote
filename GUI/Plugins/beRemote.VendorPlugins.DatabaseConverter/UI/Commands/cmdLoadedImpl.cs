using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.VendorPlugins.DatabaseConverter.UI.Commands
{
    public class CmdLoadedImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnLoaded(new RoutedEventArgs());
        }

        #region Loaded

        public delegate void LoadedEventHandler(object sender, RoutedEventArgs e);

        public event LoadedEventHandler Loaded;

        protected virtual void OnLoaded(RoutedEventArgs e)
        {
            var Handler = Loaded;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion
        
    }
}
