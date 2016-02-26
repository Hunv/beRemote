using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdRibBtnClickImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            var eArg = new AddTabEventArgs();

            switch (sender.ToString())
            {
                case "RibHelpBtnAbout":
                    eArg.Tab = new Tabs.About.TabAbout();
                    break;
                case "RibHelpBtnHelp":
                    eArg.Tab = new Tabs.Help.TabHelp();
                    break;
                case "RibHelpBtnCommunity":
                    System.Diagnostics.Process.Start("http://www.beremote.net/forum");
                    return;
                case "RibMenuBtnFiltersets":
                    eArg.Tab = new Tabs.ManageFilter.TabManageFilter();
                    break;
                case "RibMenuBtnEdit":
                    eArg.Tab = new Tabs.ManageProfile.TabManageProfile();
                    break;
                case "RibServerBtnWhoIsOnline":
                    eArg.Tab = new Tabs.UserOnline.TabUserOnline();
                    break;
                case "RibServerBtnHistory":
                    eArg.Tab = new Tabs.UserHistory.TabUserHistory();
                    break;
                case "RibServerBtnVpn":
                    eArg.Tab = new Tabs.ManageVpn.TabManageVpn();
                    break;
                case "RibServerBtnCredentials":
                    eArg.Tab = new Tabs.ManageCredential.TabManageCredential();
                    break;
                case "RibServerBtnAddFolder":
                    eArg.Tab = new Tabs.ManageFolder.TabManageFolder();
                    break;
                case "RibServerBtnAdd":
                    eArg.Tab = new Tabs.ManageConnection.TabManageConnection(null);
                    break;
                case "RibMenuBtnConnections":
                    eArg.Tab = new Tabs.Import.TabImport();
                    break;
                case "RibMenuBtnManageUser":
                    eArg.Tab = new Tabs.ManageUser.TabManageUser();
                    break;
                case "RibMenuBtnSuperadminTools":
                    eArg.Tab = new Tabs.SuperAdminTools.TabSuperAdminTools();
                    break;
                case "RibMenuBtnDatabaseManager":
                    eArg.Tab = new Tabs.ManageDatabase.TabManageDatabase();
                    break;
                case "RibMenuBtnTestTab":
                    eArg.Tab = new Tabs.Testtab.TabTesttab();
                    break;
                case "RibMenuBtnFavorites":
                    eArg.Tab = new Tabs.ManageFavorites.TabManageFavorites();
                    break;
            }

            OnAddTab(eArg);
        }
    }
}
