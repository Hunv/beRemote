using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdLoginImpl : ICommand
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            var pwBox = sender as PasswordBox;
            if (pwBox != null)
            {
                //If there is a Password
                var pWord = pwBox.SecurePassword;
                OnLoginClick(new RoutedEventArgs(null, pWord));
                pwBox.Clear();
            }
            else
            {
                //Of there is no Password
                OnLoginClick(null);
            }
        }

        public event EventHandler CanExecuteChanged;

        #region Click-Event

        public delegate void LoginClickEventHandler(object sender, RoutedEventArgs e);

        public event LoginClickEventHandler LoginClick;

        protected virtual void OnLoginClick(RoutedEventArgs e)
        {
            var Handler = LoginClick;
            if (Handler != null) 
                Handler(this, e);
        }
        #endregion
    }
}
