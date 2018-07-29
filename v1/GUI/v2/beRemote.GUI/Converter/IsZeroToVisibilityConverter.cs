using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace beRemote.GUI.Converter
{
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class IsZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return (Visibility.Visible);

            if (value is int)
            {
                if (System.Convert.ToInt32(value) > 0)
                    return (Visibility.Collapsed);
            }

            return (Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("Not possible");
        }
    }
}
