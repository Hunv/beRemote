using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace beRemote.GUI.Tabs.Import.Command
{
    public abstract class BaseCommand : ICommand
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public abstract void Execute(object sender);

        public event EventHandler CanExecuteChanged;

        #region Events

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

        #region StartBrowsing

        public delegate void StartBrowsingEventHandler(object sender, RoutedEventArgs e);

        public event StartBrowsingEventHandler StartBrowsing;

        protected virtual void OnStartBrowsing(RoutedEventArgs e)
        {
            var Handler = StartBrowsing;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion
        
        #endregion
    }
}
