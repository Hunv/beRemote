using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace beRemote.GUI.Converter
{
    public class PasswordChangeMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3 || values[0] == null)
            {
                return (values);
            }

            var pwList = new SecureString[3];
            pwList[0] = (SecureString)values[0];
            pwList[1] = (SecureString)values[1];
            pwList[2] = (SecureString)values[2];

            return pwList;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Not available");
        }
    }
}
