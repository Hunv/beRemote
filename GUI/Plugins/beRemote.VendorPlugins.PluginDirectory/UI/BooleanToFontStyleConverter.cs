using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace beRemote.VendorPlugins.PluginDirectory.UI
{
    [ValueConversion(typeof(bool), typeof(FontWeight))]
    public class BooleanToFontStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
          
            if(((Boolean)value) == false)
                return FontWeights.Normal;
            else
            {
                return FontWeights.Bold;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
