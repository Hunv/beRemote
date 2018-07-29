using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.Controls.FolderView
{
    [ValueConversion(typeof(ObservableCollection<ConnectionItem>), typeof(bool))]
    public class SubConnectionsToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var baseValue = (ObservableCollection<ConnectionItem>)value;

            return baseValue.Count > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("How should I do this, man?");
        }
    }
}
