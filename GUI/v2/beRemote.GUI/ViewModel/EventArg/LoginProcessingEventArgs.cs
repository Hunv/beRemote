using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class LoginProcessingEventArgs : RoutedEventArgs
    {
        public LoginProcessingEventArgs(bool isLoginProcessed)
        {
            IsLoginProcessed = isLoginProcessed;
        }

        public bool IsLoginProcessed { get; set; }
    }
}
