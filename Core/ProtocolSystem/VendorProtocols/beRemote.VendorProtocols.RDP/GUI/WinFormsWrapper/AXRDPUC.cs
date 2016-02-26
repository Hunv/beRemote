using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MSTSCLib;

namespace beRemote.VendorProtocols.RDP.GUI.WinFormsWrapper
{
    public partial class AXRDPUC : UserControl
    {
        public AXRDPUC()
        {
            InitializeComponent();
        }

        public IMsRdpClientTransportSettings2 TransportSettings
        {
            get
            {
                return (IMsRdpClientTransportSettings2)((MSTSCLib.IMsRdpClient6) rdpControl.GetOcx()).TransportSettings;
            }
        }

        public IMsRdpClientAdvancedSettings6 AdvancedSettings
        {
            get
            {
                return (IMsRdpClientAdvancedSettings6)rdpControl.AdvancedSettings;
            }
        }
    }
}
