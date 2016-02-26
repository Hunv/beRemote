using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using beRemote.Core.Common.Vpn;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using Microsoft.Win32;

namespace beRemote.GUI.Tabs.ManageVpn
{
    public partial class TabManageVpn
    {
        public TabManageVpn()
        {
            InitializeComponent();
        }
        
        public override void Dispose()
        {
            base.Dispose();
            ((ViewModel.ViewModelVpn)DataContext).Dispose();

            ChangeContextRibbon(null, new List<string> { "ribTabVpn" });
        }

        #region ContextRibbon-Redirects

        public void AddVpn(int vpnType)
        {
            ((ViewModel.ViewModelVpn)DataContext).AddVpn(vpnType);
        }

        public void RemoveVpn()
        {
            ((ViewModel.ViewModelVpn)DataContext).RemoveVpn();
        }

        public void Save()
        {
            ((ViewModel.ViewModelVpn)DataContext).Save();
        }

        public void Test()
        {
            ((ViewModel.ViewModelVpn)DataContext).Test();
        }

        #endregion
    }
}
