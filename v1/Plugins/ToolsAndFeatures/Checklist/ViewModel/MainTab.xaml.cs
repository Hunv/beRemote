using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Windows.Threading;

namespace beRemote.VendorPlugins.Checklist.ViewModel
{
    /// <summary>
    /// Interaction logic for MainTab.xaml
    /// </summary>
    public partial class MainTab : INotifyPropertyChanged
    {
        #region Constructor
        public MainTab()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        public override void Dispose()
        {
            base.Dispose();

        }
        #endregion
    }
}
