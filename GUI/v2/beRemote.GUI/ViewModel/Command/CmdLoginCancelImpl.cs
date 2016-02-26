using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdLoginCancelImpl : ICommand
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            OnLoginCancelClick(null);
        }

        public event EventHandler CanExecuteChanged;

        #region Click-Event

        public delegate void LoginCancelClickEventHandler(object sender, RoutedEventArgs e);

        public event CmdLoginCancelImpl.LoginCancelClickEventHandler LoginCancelClick;

        protected virtual void OnLoginCancelClick(RoutedEventArgs e)
        {
            var Handler = LoginCancelClick;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
