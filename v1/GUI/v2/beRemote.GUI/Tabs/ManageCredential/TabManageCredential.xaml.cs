using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using beRemote.Core.Common.Helper;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.Tabs.ManageCredential
{
    /// <summary>
    /// Interaction logic for ContentTabAbout.xaml
    /// </summary>
    public partial class TabManageCredential
    {
        public TabManageCredential()
        {
            InitializeComponent();

            brdChangePassword.Visibility = Visibility.Hidden; //Hide Passwordchangegrid

            //Load Credentiallist
            List<UserCredential> credentialList = StorageCore.Core.GetUserCredentialsAll();
        }

        public DataRowView SelectedItem
        {
            get
            {
                Console.Beep();
                Console.Beep();
                return selectedItem;
            }
            set
            {
                Console.Beep();
                selectedItem = value;
            }
        }
        private DataRowView selectedItem;

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtDescription.Text.Length == 0)
            {
                MessageBox.Show("Your have to enter a name of this credential", "Missing data", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            //Adding User-Credentials to Database
            StorageCore.Core.AddUserCredentials(txtUsername.Text, Helper.EncryptStringToBytes(pbPassword.SecurePassword, Helper.GetHash1(StorageCore.Core.GetUserSalt1()), Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid().ToCharArray()), StorageCore.Core.GetUserSalt3()), txtDomain.Text, txtDescription.Text);

            //Empty Textboxes
            txtUsername.Clear();
            txtDomain.Clear();
            txtDescription.Clear();
            pbPassword.Clear();

            //Reload the DataGrid-Content
            LoadDataGrid();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Load the content of the DataGrid
            LoadDataGrid();
        }

        private List<UserCredentialGridInformation> getGridCredentials()
        {
            //Load Credentiallist
            List<UserCredential> credentialList = StorageCore.Core.GetUserCredentialsAll();

            //Information for the DataGrid
            List<UserCredentialGridInformation> ret = new List<UserCredentialGridInformation>();

            foreach (UserCredential uC in credentialList)
            {
                UserCredentialGridInformation ucgi = new UserCredentialGridInformation(uC);
                if (ucgi.PasswordStatus == "yes")
                    ucgi.KeyImage = "pack://application:,,,/Images/key16.png";

                ret.Add(ucgi);
            }

            return (ret);
        }

        private void LoadDataGrid()
        {
            Binding b = new Binding("") { Mode = BindingMode.OneTime, Source = getGridCredentials() };

            dgExisting.SetBinding(DataGrid.ItemsSourceProperty, b);

            if (dgExisting.Items.Count > 0)
                dgExisting.Columns[getColumnId("Secure")].DisplayIndex = 5;
        }

        /// <summary>
        /// Gets the Id of a Column identified by the columnheader of dgExisting
        /// </summary>
        /// <param name="columnHeader"></param>
        /// <returns></returns>
        private int getColumnId(string columnHeader)
        {
            for (int i = 0; i < dgExisting.Columns.Count; i++)
            {
                if (dgExisting.Columns[i].Header.ToString() == columnHeader)
                    return (i);
            }
            return (0);
        }

        private void btnDgRemove_Click(object sender, RoutedEventArgs e)
        {
            UserCredentialGridInformation myRow = (UserCredentialGridInformation)dgExisting.SelectedItem;

            StorageCore.Core.DeleteUserCredential(myRow.Id);

            LoadDataGrid();
        }

        private void btnDgChangePassword_Click(object sender, RoutedEventArgs e)
        {
            brdChangePassword.Margin = new Thickness(Mouse.GetPosition(this).X - 5, Mouse.GetPosition(this).Y - 5, 0, 0);
            brdChangePassword.Visibility = Visibility.Visible;
            pbChangePassword.Focus();
            brdChangePassword.Focus();

            //MessageBox.Show(dgExisting.SelectedIndex.ToString());
        }

        private void pbChangePassword_KeyUp(object sender, KeyEventArgs e)
        {
            //If Enter is pressed while the Grid was visible
            if (e.Key == Key.Enter && brdChangePassword.Visibility == Visibility.Visible)
            {
                //Hide the grid
                brdChangePassword.Visibility = Visibility.Hidden;

                //Change the Password
                changePassword();
            }
        }

        /// <summary>
        /// Changes the Password of a Credential, identified by ID
        /// </summary>
        private void changePassword()
        {
            var myRow = (UserCredentialGridInformation)dgExisting.SelectedItem;

            //Save the new Password
            StorageCore.Core.ModifyUserCredential(myRow.Id,
                myRow.Username,
                //Helper.GetPasswordHash(pbChangePassword.SecurePassword, StorageCore.Core.GetUserSalt1(), StorageCore.Core.GetUserSalt2()),
                Helper.EncryptStringToBytes(
                    pbChangePassword.SecurePassword,
                    Helper.GetHash1(StorageCore.Core.GetUserSalt1()),
                    Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid().ToCharArray()),
                    StorageCore.Core.GetUserSalt3()), 
                myRow.Domain,
                StorageCore.Core.GetUserId(),
                myRow.Description);

            //Clear Password-Box
            pbChangePassword.Clear();
        }

        /// <summary>
        /// Modifies a Credential, except Password, identified by ID
        /// </summary>
        private void modifyCredentials(string Columnname, string newValue)
        {
            UserCredentialGridInformation myRow = (UserCredentialGridInformation)dgExisting.SelectedItem;
            Columnname = Columnname.ToLower();

            //Save the new Usercredentials
            StorageCore.Core.ModifyUserCredential(myRow.Id,
                (Columnname == "username" ? newValue : myRow.Username),
                (Columnname == "domain" ? newValue : myRow.Domain),
                StorageCore.Core.GetUserId(),
                (Columnname == "description" ? newValue : myRow.Description));

            //Reload the DataGrid to get the Data consistent
            //LoadDataGrid();
        }

        private void brdChangePassword_MouseMove(object sender, MouseEventArgs e)
        {
            //Hide the ChangePassword-Dialog if the mouse moved out of the Dialog-Area
            if (Mouse.GetPosition(brdChangePassword).X > brdChangePassword.Width ||
                Mouse.GetPosition(brdChangePassword).X < 0 ||
                Mouse.GetPosition(brdChangePassword).Y > brdChangePassword.Height ||
                Mouse.GetPosition(brdChangePassword).Y < 0)
            {
                brdChangePassword.Visibility = Visibility.Hidden;
            }
        }

        private void btnChangePasswordClear(object sender, RoutedEventArgs e)
        {
            //Hide the grid
            brdChangePassword.Visibility = Visibility.Hidden;

            //Clear the PasswordField (because: Empty password)
            pbChangePassword.Clear();

            //change the Password
            changePassword();

            //Reload the grid to update the images
            LoadDataGrid();
        }

        private void dgExisting_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox t = e.EditingElement as TextBox;
            if (t != null)
            {
                string value = t.Text;

                modifyCredentials(e.Column.Header.ToString(), value);
            }
        }

        private void dgExisting_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //Hide some columns
            if (e.Column.Header.ToString() == "Id" ||
                e.Column.Header.ToString() == "Password" ||
                e.Column.Header.ToString() == "Owner" ||
                e.Column.Header.ToString() == "PasswordStatus" ||
                e.Column.Header.ToString() == "KeyImage")
                e.Column.Visibility = Visibility.Hidden;
        }

        public override void Dispose()
        {
            base.Dispose();
            SelectedItem = null;
        }
    }
}
