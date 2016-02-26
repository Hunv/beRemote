using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.VendorPlugins.DatabaseConverter.UI.Commands
{
    public class CmdImportImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnImport(new RoutedEventArgs());
        }

        #region Import

        public delegate void ImportEventHandler(object sender, RoutedEventArgs e);

        public event ImportEventHandler Import;

        protected virtual void OnImport(RoutedEventArgs e)
        {
            var Handler = Import;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion
    }
}
