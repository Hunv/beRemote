using System.Windows;

namespace beRemote.Core.Definitions.EventArgs
{
    public class UpdateEventArgs : RoutedEventArgs
    {
        public UpdateEventArgs()
        { }

        public UpdateEventArgs(object value)
        {

        }

        private object _Value;

        public object Value { get { return _Value; } set { _Value = value; } }

    }
}
