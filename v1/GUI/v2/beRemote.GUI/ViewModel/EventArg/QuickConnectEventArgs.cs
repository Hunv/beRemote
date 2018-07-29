using System.Windows;
using System.Windows.Input;
using beRemote.Core.ProtocolSystem.ProtocolBase;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class QuickConnectEventArgs : RoutedEventArgs
    {
        public Protocol SelectedProtocol { get; set; }
        public Key? Key { get; set; }
        public string Text { get; set; }
    }
}
