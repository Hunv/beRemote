using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdCreateAccountSaveImpl : ICommand
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            OnSaveClick(new RoutedEventArgs());
        }

        public event EventHandler CanExecuteChanged;

        #region Click-Event

        public delegate void SaveClickEventHandler(object sender, RoutedEventArgs e);

        public event SaveClickEventHandler SaveClick;

        protected virtual void OnSaveClick(RoutedEventArgs e)
        {
            var Handler = SaveClick;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
