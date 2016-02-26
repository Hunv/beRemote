using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace beRemote.GUI.Tabs.ManageFolder
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolToBoolInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool) || value.GetType() != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return (!(bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool) || value.GetType() != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return (!(bool)value);
        }
    }
}
