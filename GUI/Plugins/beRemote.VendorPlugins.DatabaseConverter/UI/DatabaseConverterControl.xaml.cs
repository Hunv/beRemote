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

namespace beRemote.VendorPlugins.DatabaseConverter.UI
{
    /// <summary>
    /// Interaction logic for DatabaseConverterControl.xaml
    /// </summary>
    public partial class DatabaseConverterControl
    {
        public DatabaseConverterControl()
        {
            InitializeComponent();

            ((ViewModel)DataContext).RefreshConnectionList += DatabaseConverterControl_RefreshConnectionList;
        }

        void DatabaseConverterControl_RefreshConnectionList(object sender, RoutedEventArgs e)
        {
            RefreshConnectionList();
        }

        public override void Dispose()
        {
            base.Dispose();

            ((ViewModel) DataContext).RefreshConnectionList -= DatabaseConverterControl_RefreshConnectionList;
            ((ViewModel) DataContext).Dispose();
        }
    }
}
