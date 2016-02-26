using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace beRemote.GUI.Tabs.Import
{
    /// <summary>
    /// Interaction logic for TabImport.xaml
    /// </summary>
    public partial class TabImport
    {
        public TabImport()
        {
            InitializeComponent();
            ((ViewModel)this.DataContext).RefreshConnectionList += TabImport_RefreshConnectionList;
        }

        void TabImport_RefreshConnectionList(object sender, RoutedEventArgs e)
        {
            RefreshConnectionList();
        }

        public override void Dispose()
        {
            base.Dispose();

            ((ViewModel)this.DataContext).RefreshConnectionList -= TabImport_RefreshConnectionList;
            ((ViewModel)this.DataContext).Dispose();
        }
    }
}
