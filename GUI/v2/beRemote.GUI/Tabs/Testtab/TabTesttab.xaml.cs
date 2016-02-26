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
using beRemote.GUI.ViewModel;

namespace beRemote.GUI.Tabs.Testtab
{
    /// <summary>
    /// Interaction logic for ContentTabTesttab.xaml
    /// </summary>
    public partial class TabTesttab 
    {
        public TabTesttab()
        {
            InitializeComponent();
        }

        private void btnCloseTab_Click(object sender, RoutedEventArgs e)
        {
            CloseTab();
        }

        private void btnRefreshList_Click(object sender, RoutedEventArgs e)
        {
            RefreshConnectionList();
        }

        private void ViewModelTabGeneral_Loaded(object sender, RoutedEventArgs e)
        {
            RaisePropertyChanged("IsLoaded");
        }

        private void btnTopText_Click(object sender, RoutedEventArgs e)
        {
            TopText = "Hey, this is a Text";
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }

}
    