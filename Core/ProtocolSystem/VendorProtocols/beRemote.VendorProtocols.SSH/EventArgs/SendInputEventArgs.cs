using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.VendorProtocols.SSH.EventArgs
{
    public class SendInputEventArgs : RoutedEventArgs
    {
        public string SendCommand { get; set; }
    }
}
