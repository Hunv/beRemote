using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using beRemote.Core.Common.Helper;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.Tabs.ManageUser
{
    /// <summary>
    /// Interaction logic for ContentTabAbout.xaml
    /// </summary>
    public partial class TabManageUser
    {
        public TabManageUser()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Load Userlist
            Dictionary<int, string> userList = StorageCore.Core.GetUserList();
            lstUser.Items.Clear();
            foreach (KeyValuePair<int, string> kvp in userList)
            {
                var lstItem = new ListBoxItem();
                lstItem.Content = kvp.Value;
                lstItem.Tag = kvp.Key;
                lstUser.Items.Add(lstItem);
            }
        }

        private void lstUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User usr = StorageCore.Core.GetUserSettings(Convert.ToInt32(((ListBoxItem)lstUser.SelectedItem).Tag));
            txtUsername.Text = usr.getName();
            txtWinname.Text = usr.getWinname();
            chkSuperadmin.IsChecked = StorageCore.Core.GetUserSuperadmin(Convert.ToInt32(((ListBoxItem)lstUser.SelectedItem).Tag));
        }
        
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lstUser.SelectedItem == null)
            {
                MessageBox.Show("No user selected. Do you want to add a new user? Try the \"Add new\"-Button!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                StorageCore.Core.ModifyUser(Convert.ToInt32(((ListBoxItem)lstUser.SelectedItem).Tag), txtUsername.Text, txtWinname.Text);
                StorageCore.Core.ModifyUserSuperadmin(Convert.ToInt32(((ListBoxItem)lstUser.SelectedItem).Tag), chkSuperadmin.IsChecked != null && chkSuperadmin.IsChecked.Value);

                if (pbPassword.SecurePassword.Length > 0)
                {
                    var msgResult = MessageBox.Show(
                        "WARNING!" + Environment.NewLine +
                        "A reset of the userpassword will result in loss of all credential-passwords for the user!" + Environment.NewLine +
                        "Do you still want to reset the password?", "WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                    if (msgResult == MessageBoxResult.Yes)
                    {
                        var salt1 = Helper.GenerateSalt(512);
                        var salt2 = Helper.GenerateSalt(512);
                        var userId = Convert.ToInt32(((ListBoxItem) lstUser.SelectedItem).Tag);
                        StorageCore.Core.ModifyUserPassword(userId, Helper.GetPasswordHash(pbPassword.SecurePassword, salt1, salt2));
                        StorageCore.Core.SetUserSalt1(userId, salt1);
                        StorageCore.Core.SetUserSalt2(userId, salt2);
                        StorageCore.Core.SetUserSalt3(userId, Helper.GenerateSalt(512));
                    }
                    else
                    {
                        MessageBox.Show("Password was not changed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void btnAddAsNew_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text.Length <= 2) 
                return;
            
            if (StorageCore.Core.GetUserId(txtUsername.Text) == 0)
            {
                var salt1 = Helper.GenerateSalt(512);
                var salt2 = Helper.GenerateSalt(512);
                var newUser = StorageCore.Core.AddUser(txtUsername.Text, txtWinname.Text, Helper.GetPasswordHash(pbPassword.SecurePassword, salt1, salt2));
                StorageCore.Core.SetUserSalt1(newUser, salt1);
                StorageCore.Core.SetUserSalt2(newUser, salt2);
                StorageCore.Core.SetUserSalt3(newUser, Helper.GenerateSalt(512));
                StorageCore.Core.ModifyUserSuperadmin((int)newUser, chkSuperadmin.IsChecked != null && chkSuperadmin.IsChecked.Value);
                UserControl_Loaded(null, null);
            }
            else
                MessageBox.Show("This user already exists", "User exists", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
