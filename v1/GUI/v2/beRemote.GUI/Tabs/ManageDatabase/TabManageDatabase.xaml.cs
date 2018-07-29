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
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.Tabs.ManageDatabase
{
    public partial class TabManageDatabase
    {
        public TabManageDatabase()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dataTables = StorageCore.Core.GetDatabaseTables();

            lbTables.DisplayMemberPath = "name";
            lbTables.SelectedValuePath = "name";
            lbTables.ItemsSource = dataTables.DefaultView;
        }

        DataTable _DataTableContent;

        private void lbTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbTables.SelectedValue == null)
                return;

            _DataTableContent = StorageCore.Core.GetDatabaseTableContent(lbTables.SelectedValue.ToString());

            dgContent.ItemsSource = _DataTableContent.DefaultView;
        }

        private void dgContent_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (_DataTableContent.Columns[e.Column.DisplayIndex].DataType == typeof(DateTime))
            {
                MessageBox.Show("Cannot edit Cells of type DateTime", "Not possible", MessageBoxButton.OK, MessageBoxImage.Hand);
                e.Cancel = true;

                return;
            }

            DataRowView myRow = (DataRowView)dgContent.SelectedItem;

            string id = "-1";
            for (int i = 0; i < dgContent.Columns.Count; i++)
            {
                if (dgContent.Columns[i].Header.ToString() == "id")
                {
                    id = myRow.Row.ItemArray[i].ToString();
                    break;
                }
            }

            if (id == "-1")
            {
                MessageBox.Show("Cannot edit Tables without an ID-Column, sorry", "Not possible", MessageBoxButton.OK, MessageBoxImage.Hand);
                e.Cancel = true;

                return;
            }

            Dictionary<string, string> filter = new Dictionary<string, string>(1);
            filter.Add("id", id);

            bool isString = true;
            string value = "";

            switch (e.Column.GetType().ToString().ToLower())
            {
                case "system.windows.controls.datagridtextcolumn":
                    TextBox t = e.EditingElement as TextBox;
                    value = t.Text;

                    if (_DataTableContent.Columns[e.Column.DisplayIndex].DataType == typeof(int) ||
                        _DataTableContent.Columns[e.Column.DisplayIndex].DataType == typeof(Int16) ||
                        _DataTableContent.Columns[e.Column.DisplayIndex].DataType == typeof(Int64) ||
                        _DataTableContent.Columns[e.Column.DisplayIndex].DataType == typeof(uint) ||
                        _DataTableContent.Columns[e.Column.DisplayIndex].DataType == typeof(UInt16) ||
                        _DataTableContent.Columns[e.Column.DisplayIndex].DataType == typeof(UInt32) ||
                        _DataTableContent.Columns[e.Column.DisplayIndex].DataType == typeof(byte))
                    {
                        isString = false;
                    }
                    else
                    {
                        isString = true;
                    }
                    break;
                case "system.windows.controls.datagridcheckboxcolumn":
                    CheckBox c = e.EditingElement as CheckBox;
                    value = (c.IsChecked.Value ? "1" : "0");
                    isString = false;
                    break;
            }



            StorageCore.Core.ModifyDatabaseTableContent(
                lbTables.SelectedValue.ToString(),
                e.Column.Header.ToString(),
                value,
                isString,
                filter);
        }

        public override void Dispose()
        {
            base.Dispose();
            _DataTableContent.Clear();
            _DataTableContent.Dispose();
        }
    }
}
