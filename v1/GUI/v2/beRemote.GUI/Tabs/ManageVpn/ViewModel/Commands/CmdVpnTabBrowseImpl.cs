using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.Tabs.ManageVpn.ViewModel.Commands
{
    public class CmdVpnTabBrowseImpl : BaseCommand
    {
        public override void Execute(object vpnType)
        {
            var OFd = new OpenFileDialog();
            OFd.Multiselect = false;

            if (OFd.ShowDialog() == false)
                return;

            var ev = new BrowseFileEventArgs();
            ev.BrowsePath = OFd.FileName;
            ev.BrowseSender = vpnType.ToString();
            OnBrowseFile(ev);
        }
    }
}
