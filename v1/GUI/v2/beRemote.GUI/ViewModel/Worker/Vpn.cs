using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.GUI.ViewModel.Worker
{
    public class Vpn
    {
        /// <summary>
        /// Sends tha AddVpn Command to the VPN Management Tab
        /// </summary>
        /// <param name="tabs"></param>
        /// <param name="vpnType"></param>
        public static void AddVpn(ObservableCollection<ViewModelTabBase> tabs, string vpnType)
        {
            int iVpn = 0;

            //Try to parse the parameter. If it fails, no predefined VPN will be loaded (iVPN will be 0)
            Int32.TryParse(vpnType, out iVpn);

            GetTab(tabs).AddVpn(iVpn);
        }

        /// <summary>
        /// Sends the RemoveVpn Command to the VPN Management Tab
        /// </summary>
        /// <param name="tabs"></param>
        public static void RemoveVpn(ObservableCollection<ViewModelTabBase> tabs)
        {
            GetTab(tabs).RemoveVpn();
        }

        /// <summary>
        /// Sends the Save Command to the VPN Management Tab
        /// </summary>
        /// <param name="tabs"></param>
        public static void SaveVpn(ObservableCollection<ViewModelTabBase> tabs)
        {
            GetTab(tabs).Save();
        }

        /// <summary>
        /// Sends the Test Command to the VPN Management Tab
        /// </summary>
        /// <param name="tabs"></param>
        public static void TestVpn(ObservableCollection<ViewModelTabBase> tabs)
        {
            GetTab(tabs).Test();
        }

        #region Helper
        /// <summary>
        /// Gets the Tab of the Type that is used for this Worker
        /// </summary>
        /// <returns></returns>
        private static Tabs.ManageVpn.TabManageVpn GetTab(ObservableCollection<ViewModelTabBase> tabs)
        {
            foreach (var aTab in tabs)
            {
                if (aTab.Content.GetType() == typeof(Tabs.ManageVpn.TabManageVpn))
                {
                    return((Tabs.ManageVpn.TabManageVpn)aTab.Content);                    
                }
            }

            return (null);
        }
        #endregion
    }
}
