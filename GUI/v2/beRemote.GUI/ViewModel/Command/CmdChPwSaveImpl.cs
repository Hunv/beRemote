using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdChPwSaveImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            var _Profile = new Profile();

            var pwList = (SecureString[])sender;

            if (_Profile.ChangePassword(pwList[0], pwList[1], pwList[2]))
                OnPasswordChangeSuccess(new RoutedEventArgs());
            else
                OnPasswordChangeFailed(new RoutedEventArgs());
        }


        #region PasswordChangeSuccess

        public delegate void PasswordChangeSuccessEventHandler(object sender, RoutedEventArgs e);

        public event PasswordChangeSuccessEventHandler PasswordChangeSuccess;

        protected virtual void OnPasswordChangeSuccess(RoutedEventArgs e)
        {
            var Handler = PasswordChangeSuccess;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region PasswordChangeFailed

        public delegate void PasswordChangeFailedEventHandler(object sender, RoutedEventArgs e);

        public event PasswordChangeFailedEventHandler PasswordChangeFailed;

        protected virtual void OnPasswordChangeFailed(RoutedEventArgs e)
        {
            var Handler = PasswordChangeFailed;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
