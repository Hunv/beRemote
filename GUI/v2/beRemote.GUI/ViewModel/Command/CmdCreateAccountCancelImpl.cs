using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdCreateAccountCancelImpl : ICommand
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            OnCancelClick(new RoutedEventArgs());
        }

        public event EventHandler CanExecuteChanged;

        #region Click-Event

        public delegate void CancelClickEventHandler(object sender, RoutedEventArgs e);

        public event CancelClickEventHandler CancelClick;

        protected virtual void OnCancelClick(RoutedEventArgs e)
        {
            var Handler = CancelClick;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
