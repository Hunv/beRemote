using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace beRemote.GUI.Controls.FolderView
{
    [ValueConversion(typeof(byte), typeof(Thickness))]
    public class RootLevelSpaceConverter : IValueConverter
    {
        //The Space per Rootlevel of the Item on the Left-Side
        private const byte _Multiplicator = 8;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var baseValue = (byte)value;

            return (new Thickness(baseValue * _Multiplicator, 0, 0, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var baseThickness = (Thickness) value;
            var calcResult = baseThickness.Left/_Multiplicator;
            if (calcResult > 255)
                calcResult = 255;

            return ((byte)calcResult);
        }
    }
}
