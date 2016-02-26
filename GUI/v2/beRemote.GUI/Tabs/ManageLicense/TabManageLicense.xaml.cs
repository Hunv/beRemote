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
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.Tabs.ManageLicense
{
    /// <summary>
    /// Interaction logic for ContentTabAbout.xaml
    /// </summary>
    public partial class TabManageLicense
    {
        private List<License> _Licenses = new List<License>();

        public TabManageLicense()
        {
            InitializeComponent();
        }

        public List<License> Licenses
        {
            get { return _Licenses; }
            set { _Licenses = value; }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            License lic = new License(txtFirstname.Text, txtLastname.Text, txtEmail.Text, txtSecret.Text, StorageCore.Core.GetUserSettings().getId());
            StorageCore.Core.AddUserLicense(lic);

            UserControl_Loaded(null, null);

            btnClear_Click(sender, e);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtEmail.Clear();
            txtFirstname.Clear();
            txtLastname.Clear();
            txtSecret.Clear();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _Licenses = StorageCore.Core.GetLicenses();

            //Convert to Datatable (s*** TwoWay-Binding doesn't work *#!!"§"!)
            DataTable dT = new DataTable();
            DataColumn dC = new DataColumn();
            dC.Caption = dC.ColumnName = "Firstname";
            dT.Columns.Add(dC);
            dC = new DataColumn();
            dC.Caption = dC.ColumnName = "Lastname";
            dT.Columns.Add(dC);
            dC = new DataColumn();
            dC.Caption = dC.ColumnName = "Email";
            dT.Columns.Add(dC);
            dC = new DataColumn();
            dC.Caption = dC.ColumnName = "Secret";
            dT.Columns.Add(dC);

            foreach (License lic in _Licenses)
            {
                DataRow dR = dT.NewRow();
                dR["Firstname"] = lic.Firstname;
                dR["Lastname"] = lic.Lastname;
                dR["Email"] = lic.Email;
                dR["Secret"] = lic.Secret;

                dT.Rows.Add(dR);
            }

            dgLicenses.ItemsSource = dT.DefaultView;

            dgLicenses.CanUserAddRows = false;
            dgLicenses.CanUserDeleteRows = false;
            dgLicenses.AlternationCount = 2;
            dgLicenses.AreRowDetailsFrozen = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgLicenses.SelectedValue == null)
                return;

            DataRow x = ((DataRowView)dgLicenses.SelectedValue).Row;

            License selectedLicense = new License(x.ItemArray[0].ToString(), x.ItemArray[1].ToString(), x.ItemArray[2].ToString(), x.ItemArray[3].ToString(), StorageCore.Core.GetUserId());

            StorageCore.Core.DeleteUserLicense(selectedLicense);
            UserControl_Loaded(null, null);
        }

        private void dgLicenses_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        public override void Dispose()
        {
            base.Dispose();
            _Licenses.Clear();
        }
    }
}
