using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace beRemote.VendorProtocols.SSH.ViewModel.Converter
{
    [ValueConversion(typeof(bool), typeof(TextWrapping))]
    public class BoolToTextWrappingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(TextWrapping) || value.GetType() != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            if ((bool)value)
                return (TextWrapping.NoWrap);

            return (TextWrapping.Wrap);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((TextWrapping)value == TextWrapping.NoWrap)
                return false;

            return true;
        }
    }
}
