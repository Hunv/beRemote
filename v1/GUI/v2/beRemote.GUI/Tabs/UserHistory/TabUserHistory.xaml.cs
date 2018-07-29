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
using beRemote.Core;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Classes;

namespace beRemote.GUI.Tabs.UserHistory
{
    /// <summary>
    /// Interaction logic for ContentTabAbout.xaml
    /// </summary>
    public partial class TabUserHistory
    {
        private int _CurrentIndex = 0;

        public TabUserHistory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadHistory();
        }

        /// <summary>
        /// Loads the History
        /// </summary>
        private void loadHistory(DateTime date = new DateTime())
        {
            if (date == new DateTime())
                lblDisplay.Content = "Entry " + _CurrentIndex + " to " + (_CurrentIndex + 100);
            else
                lblDisplay.Content = "Entries of " + date.ToShortDateString();

            var dtHistory = new DataTable();
            var dC = new DataColumn("Image", typeof(ImageSource));
            dtHistory.Columns.Add(dC);
            var dC2 = new DataColumn("Name");
            dtHistory.Columns.Add(dC2);
            var dC3 = new DataColumn("conId", typeof(long));
            dtHistory.Columns.Add(dC3);
            var dC4 = new DataColumn("IP or Hostname");
            dtHistory.Columns.Add(dC4);
            var dC5 = new DataColumn("Connectiontime");//, typeof(DateTime));
            dtHistory.Columns.Add(dC5);

            try
            {
                List<UserHistoryEntry> history;
                if (date == new DateTime())
                    history = StorageCore.Core.GetUserHistory(StorageCore.Core.GetUserId(), 100, _CurrentIndex);
                else
                    history = StorageCore.Core.GetHistoryDate(StorageCore.Core.GetUserId(), date);
                                
                foreach (var uhe in history)
                {
                    var dR = dtHistory.NewRow();

                    if (Kernel.GetAvailableProtocols().ContainsKey(uhe.Protocol))
                        dR["Image"] =
                            Kernel.GetAvailableProtocols()[uhe.Protocol].GetProtocolIcon(
                                Core.ProtocolSystem.ProtocolBase.Declaration.IconType.SMALL); //Get Icon

                    dR["Name"] = uhe.Name; //Get the Displayname
                    dR["conId"] = uhe.ConnectionId; //Get the ID of the connection
                    dR["IP or Hostname"] = uhe.Host + ":" + uhe.Port.ToString(); //Get the IP and Port
                    dR["Connectiontime"] = uhe.PointOfTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss");

                    dtHistory.Rows.Add(dR);
                }
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Warning, "Error on loading History.", ea);
            }

            dgHistory.ItemsSource = dtHistory.DefaultView;
            dgHistory.Columns[1].Visibility = Visibility.Hidden; //Hide the path to the Image
            dgHistory.Columns[3].Visibility = Visibility.Hidden; //Hide the connectionID-Column
            dgHistory.SelectedValuePath = "conId";
            dgHistory.CanUserAddRows = false;
            dgHistory.CanUserDeleteRows = false;            
            dgHistory.CanUserResizeRows = false;            
            dgHistory.FocusVisualStyle = null;
        }

        private void dgHistory_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void dgHistory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Connect to System with selected Protocol and port
            //Trigger the Event            
            if (dgHistory.Items.Count > 0)
            {
                var cP = StorageCore.Core.GetConnectionSetting((long)dgHistory.SelectedValue);
                var cH = StorageCore.Core.GetConnection(cP.getConnectionId());
                     
                //todo
                //this.Connect(this, cH, cP);

                //Save to history
                StorageCore.Core.AddUserHistoryEntry(StorageCore.Core.GetConnectionSetting(cH.ID).getId());

                //Reload History-List
                loadHistory();
            }            
        }
        
        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (_CurrentIndex >= 100)
                _CurrentIndex -= 100;
            else            
                _CurrentIndex = 0;


            loadHistory();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _CurrentIndex += 100;
            btnLast.IsEnabled = true;

            loadHistory();
        }

        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDate.SelectedDate == new DateTime())            
                loadHistory(dpDate.SelectedDate.Value.AddDays(1));            
            else
                loadHistory(dpDate.SelectedDate.Value);
        }

        public int CurrentIndex
        {
            get { return _CurrentIndex; }
            set
            {
                if (value < 0)
                    _CurrentIndex = 0;
                else
                    _CurrentIndex = value;

                if (_CurrentIndex == 0)
                    btnLast.IsEnabled = false;

                loadHistory();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            dgHistory.ItemsSource = null;
        }
    }
}
