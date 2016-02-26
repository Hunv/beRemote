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
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.Tabs.ManageFolder
{
    /// <summary>
    /// Interaction logic for TabManageFolder.xaml
    /// </summary>
    public partial class TabManageFolder
    {
        public TabManageFolder()
        {
            InitializeComponent();
        }

        public TabManageFolder(ConnectionItem selectedFolder)
        {
            InitializeComponent();
            ((ViewModel) DataContext).StartupFolder = selectedFolder;
        }

        public override void Dispose()
        {
            base.Dispose();
            ((ViewModel) DataContext).Dispose();
        }
    }
}
