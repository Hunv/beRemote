using System;
using System.Collections.Generic;
using System.Security;
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

namespace beRemote.VendorProtocols.Sample
{
    /// <summary>
    /// Interaction logic for SampleSessionWindow.xaml
    /// </summary>
    public partial class SampleSessionWindow
    {
        private string _user;
        private string _pass;

        public SampleSessionWindow()
        {
            InitializeComponent();


            //Title = session.GetSessionServer().GetServerName();
            //IconSource = session.GetSessionProtocol().ProtocolIconSmall;
            //TabToolTip = session.GetSessionServer().GetServerHostName();
        }


        internal void OpenNewConnection(string username, SecureString password)
        {
            wbBrowser.Navigate("http://beremote.net/samples/SampleProtocol.php?name=" + username);
        }

        public override void Dispose()
        {
            base.Dispose();

            wbBrowser.Dispose();
        }
    }
}
