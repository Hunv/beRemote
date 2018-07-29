using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.VendorPlugins.DatabaseConverter.UI.Commands
{
    public class CmdStartImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnStartImport(new RoutedEventArgs());
        }

        #region StartImport

        public delegate void StartImportEventHandler(object sender, RoutedEventArgs e);

        public event StartImportEventHandler StartImport;

        protected virtual void OnStartImport(RoutedEventArgs e)
        {
            var Handler = StartImport;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion
    }
}
