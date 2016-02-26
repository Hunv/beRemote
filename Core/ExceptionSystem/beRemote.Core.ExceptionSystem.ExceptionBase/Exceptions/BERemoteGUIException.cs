using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    [Serializable]
    public class BERemoteGUIException : BERemoteException
    {
        
        public BERemoteGUIException(String message)
            : base(message)
        {
            
        }

        public BERemoteGUIException(String message, Exception innerEx)
            : base(message, innerEx)
        {
                      
        }

        public void ShowMessageWindow(Window owner)
        {
            GUI.ExceptionWindow excWindow = new GUI.ExceptionWindow(this, owner);

            

           
            excWindow.ShowDialog();
            
        }

    }
}

