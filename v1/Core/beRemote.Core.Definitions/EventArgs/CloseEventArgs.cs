using System.Windows;

namespace beRemote.Core.Definitions.EventArgs
{
    public class CloseEventArgs : RoutedEventArgs
    {
        public CloseEventArgs()
        { }

        public CloseEventArgs(string reason)
        {
            Reason = reason;
        }

        private string _Reason;

        public string Reason { get { return _Reason; } set { _Reason = value; } }
    }
}
