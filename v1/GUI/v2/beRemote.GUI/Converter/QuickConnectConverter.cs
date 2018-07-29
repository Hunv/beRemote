using System;
using System.Windows.Data;
using System.Windows.Input;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.Converter
{
    [ValueConversion(typeof(object[]), typeof(QuickConnectEventArgs))]
    public class QuickConnectConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.Length != 3)
                return(null);

            if (string.IsNullOrEmpty(value[1].ToString()) || value[2] == null)
                return(null);

            var evArgs = new QuickConnectEventArgs();
            evArgs.SelectedProtocol = (Protocol) value[0];
            evArgs.Text = value[1].ToString();

            if (value[2] is KeyEventArgs)
                evArgs.Key = ((KeyEventArgs) value[2]).Key;
            
            return (evArgs);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("Not possible, maybe");
        }
    }
}
