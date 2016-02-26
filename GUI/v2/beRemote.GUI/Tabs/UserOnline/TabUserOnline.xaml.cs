using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.Tabs.UserOnline
{
    /// <summary>
    /// Interaction logic for ContentTabAbout.xaml
    /// </summary>
    public partial class TabUserOnline
    {
        readonly DispatcherTimer tmrRefresh = new DispatcherTimer();

        public TabUserOnline()
        {
            InitializeComponent();
        }   
        
        void tmrRefresh_Tick(object sender, EventArgs e)
        {
            var online = StorageCore.Core.GetUsersOnline();
            lstUser.ItemsSource = online;
        }

        private void TabBase_Loaded(object sender, RoutedEventArgs e)
        {
            var online = StorageCore.Core.GetUsersOnline();
            lstUser.ItemsSource = online;

            tmrRefresh.Interval = new TimeSpan(0, 1, 0);
            tmrRefresh.Tick += tmrRefresh_Tick;
            tmrRefresh.Start();
        }

        public override void Dispose()
        {
            base.Dispose();
            tmrRefresh.Stop();
            lstUser.ItemsSource = null;

        }
    }
}
