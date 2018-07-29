using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Common.Helper;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel
{
    public class Profile
    {
        #region Events
        #region ShowMessage

        public delegate void ShowMessageEventHandler(object sender, ShowMessageEventArgs e);

        public event ShowMessageEventHandler ShowMessage;

        protected virtual void OnShowMessage(ShowMessageEventArgs e)
        {
            var Handler = ShowMessage;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
        #endregion

        /// <summary>
        /// Changes the password
        /// </summary>
        /// <param name="oldPassword">The current password</param>
        /// <param name="newPassword">The new password</param>
        /// <param name="newPasswordRepeat">The new password again</param>
        /// <returns></returns>
        public bool ChangePassword(SecureString oldPassword, SecureString newPassword, SecureString newPasswordRepeat)
        {
            if (!newPassword.SecureStringsAreEqual(newPasswordRepeat))
            {
                //ToDo: Translate
                OnShowMessage(new ShowMessageEventArgs("The new passwords are not matching", "Wrong repeat", System.Windows.MessageBoxImage.Error));
                return (false);
            }

            if (StorageCore.Core.CheckUserPassword(Helper.GetPasswordHash(oldPassword, StorageCore.Core.GetUserSalt1(), StorageCore.Core.GetUserSalt2())) == false)
            {
                //Cancel; Old Password is wrong
                //ToDo: Translate
                OnShowMessage(new ShowMessageEventArgs("The old password you entered is wrong", "Wrong old password", System.Windows.MessageBoxImage.Error));
                return (false);
            }

            if (newPassword.Length < 3)
            {
                //Password to short
                //ToDo: Translate
                OnShowMessage(new ShowMessageEventArgs("The new password is to short. The minimum size are three characters.", "Password to small", System.Windows.MessageBoxImage.Error));
                return (false);
            }

            var userId = StorageCore.Core.GetUserId();
            var salt1 = Helper.GenerateSalt(512);
            var salt2 = Helper.GenerateSalt(512);
            var salt3 = Helper.GenerateSalt(512);

            //todo:
            //Reencrypt Credentials
            if (StorageCore.Core.GetUserSalt1(userId) != null)
            {
                var dbGuid = Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid());
                var creds = StorageCore.Core.GetUserCredentialsAll();
                foreach (var cred in creds)
                {
                    if (cred.Owner == userId) //Igonore Public credentials (for future use)
                    {
                        StorageCore.Core.ModifyUserCredential
                        (
                            cred.Id, 
                            cred.Username,
                            Helper.EncryptStringToBytes
                            (
                                Helper.DecryptStringFromBytes
                                (
                                    cred.Password, 
                                    Helper.GetHash1(StorageCore.Core.GetUserSalt1()),
                                    dbGuid, 
                                    StorageCore.Core.GetUserSalt3()
                                ),
                                Helper.GetHash1(salt1), 
                                dbGuid, 
                                salt3
                            ),
                            cred.Domain, 
                            cred.Owner, 
                            cred.Description
                        );
                    }
                }
            }

            var newHash = Helper.GetPasswordHash(newPassword, salt1, salt2);
            StorageCore.Core.ModifyUserPassword(userId, newHash);
            StorageCore.Core.SetUserSalt1(salt1);
            StorageCore.Core.SetUserSalt2(salt2);
            StorageCore.Core.SetUserSalt3(Helper.GenerateSalt(512));
            
            return (true);
        }
    }
}
