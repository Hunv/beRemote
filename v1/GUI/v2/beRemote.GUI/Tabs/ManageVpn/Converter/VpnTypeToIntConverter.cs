using beRemote.Core.Common.Vpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace beRemote.GUI.Tabs.ManageVpn.Converter
{
    [ValueConversion(typeof(VpnType), typeof(Int32))]
    public class VpnTypeToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)((VpnType)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((VpnType)(int)value);
        }
    }
}
