using beRemote.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace beRemote.Core
{
    public class KernelFailedEventArgs : RoutedEventArgs
    {
        public beRemoteException Exception;

        public KernelFailedEventArgs(beRemoteException ex)
        {
            // TODO: Complete member initialization
            this.Exception = ex;
        }
    }
}
