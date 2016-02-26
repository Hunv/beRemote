using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class ShowMessageEventArgs : RoutedEventArgs
    {
        public ShowMessageEventArgs(string message, string title, MessageBoxImage image)
        {
            Message = message;
            Title = title;
            Image = image;
        }

        public string Message { get; set; }
        public string Title { get; set; }
        public MessageBoxImage Image { get; set; }
    }
}
