using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using beRemote.Core;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.Definitions.EventArgs;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.Tabs.ManageFavorites
{
    /// <summary>
    /// Interaction logic for ContentTabAbout.xaml
    /// </summary>
    public partial class TabManageFavorites
    {
        public TabManageFavorites()
        {
            InitializeComponent();
        }

        private void dgQuickies_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void dgQuickies_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //Hide Image-Column and conId-Column
            //if (e.Column.Header.ToString() == "Image" || e.Column.Header.ToString() == "conSetId")
            //    e.Column.Visibility = System.Windows.Visibility.Collapsed;
        }

        private readonly DataTable _DtQuickies = new DataTable();

        public DataView DtQuickies { get { return _DtQuickies.DefaultView; } }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var uV = StorageCore.Core.GetUserVisuals();
            if (uV.Favorites.Length > 0)
            {
                var qCons = uV.Favorites.Split(';');

                var dC = new DataColumn("Image", typeof(ImageSource));
                _DtQuickies.Columns.Add(dC);
                var dC2 = new DataColumn("Host");
                _DtQuickies.Columns.Add(dC2);
                var dC3 = new DataColumn("conSetId", typeof(long));
                _DtQuickies.Columns.Add(dC3);

                foreach (var aCon in qCons)
                {
                    if (aCon == "") continue; //Prevent Errors (should never happen)

                    var cp = StorageCore.Core.GetConnectionSetting(Convert.ToInt64(aCon));

                    //The ConnectionSettings could not be queried
                    if (cp == null)
                        continue;

                    var ch = StorageCore.Core.GetConnection(cp.ConnectionId);

                    try
                    {
                        if (!Kernel.GetAvailableProtocols().ContainsKey(cp.Protocol))
                            continue;

                        var dR = _DtQuickies.NewRow();

                        dR["Image"] =
                            Kernel.GetAvailableProtocols()[cp.Protocol].GetProtocolIcon(
                                Core.ProtocolSystem.ProtocolBase.Declaration.IconType.SMALL); //Get Icon
                        dR["Host"] = ch.Name; //Get the Displayname
                        dR["conSetId"] = cp.Id; //Get the ID of the connectionSetting

                        _DtQuickies.Rows.Add(dR);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(LogEntryType.Warning, "Exception while loading Favorites managing.", ex);
                    }
                }

                RaisePropertyChanged("DtQuickies");
            }

            if (_DtQuickies.Rows.Count > 0)
                dgQuickies.SelectedIndex = 0;
        }

        private void dgQuickies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgQuickies.SelectedValue != null) //Enable Remove if something is selected
            {
                btnRemove.IsEnabled = true;

                //Disable Up if first item is selected
                btnUp.IsEnabled = dgQuickies.SelectedIndex != 0;

                //Disable down if last item is selected
                btnDown.IsEnabled = dgQuickies.SelectedIndex != dgQuickies.Items.Count - 1;
            }
            else
            {
                btnRemove.IsEnabled = false;
                btnUp.IsEnabled = false;
                btnDown.IsEnabled = false;
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var setId = Convert.ToInt64(dgQuickies.SelectedValue);

            //Go through the Datatable and search the removed ID
            for (var i = 0; i < _DtQuickies.Rows.Count; i++)
            {
                if ((long) _DtQuickies.Rows[i]["conSetId"] != setId) 
                    continue;

                _DtQuickies.Rows.Remove(_DtQuickies.Rows[i]);
                break;
            }

            RaisePropertyChanged("DtQuickies");
            updateFavString();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            var ind = dgQuickies.SelectedIndex;
            var dR = _DtQuickies.NewRow();
            dR.ItemArray = _DtQuickies.Rows[ind - 1].ItemArray;
            _DtQuickies.Rows.InsertAt(dR, ind + 1);
            _DtQuickies.Rows[ind-1].Delete();

            RaisePropertyChanged("DtQuickies");

            dgQuickies.SelectedIndex = ind - 1;

            dgQuickies_SelectionChanged(null, null);

            updateFavString();
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (_DtQuickies.Rows.Count < dgQuickies.SelectedIndex) 
                return;

            var ind = dgQuickies.SelectedIndex;
            var dR = _DtQuickies.NewRow();
            dR.ItemArray = _DtQuickies.Rows[ind + 1].ItemArray;
            _DtQuickies.Rows.InsertAt(dR, ind);
            _DtQuickies.Rows[ind + 2].Delete();

            RaisePropertyChanged("DtQuickies");
            dgQuickies.SelectedIndex = ind + 1;

            dgQuickies_SelectionChanged(null, null);

            updateFavString();
        }

        private void updateFavString()
        {
            //Generate new Favorites-String
            var favString = "";
            for (var i = 0; i < _DtQuickies.Rows.Count; i++)
            {
                favString += _DtQuickies.Rows[i]["conSetId"] + ";";
            }

            if (favString.Length > 0)
                favString = favString.Substring(0, favString.Length - 1);

            var newUv = new Dictionary<string, object>();
            newUv.Add("favorites", favString);
            StorageCore.Core.SetUserVisual(newUv);


            //Raise Update
            var evArgs = new FavoriteChangedEventArgs();
            evArgs.Favorites = favString;

            OnFavoritesChanged(evArgs);
        }

        public override void Dispose()
        {
            base.Dispose();
            _DtQuickies.Dispose();
        }
    }
}
