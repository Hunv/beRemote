using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdCreateAccountImpl : ICommand
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            OnClick(new RoutedEventArgs());
        }

        public event EventHandler CanExecuteChanged;

        #region Click-Event

        public delegate void ClickEventHandler(object sender, RoutedEventArgs e);

        public event ClickEventHandler Click;

        protected virtual void OnClick(RoutedEventArgs e)
        {
            var Handler = Click;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
