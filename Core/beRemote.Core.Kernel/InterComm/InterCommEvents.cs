using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.InterComm
{
    public class InterCommEvents
    {
        //public delegate void KernelReadyEventHandler(object sender, KernelReadyEventArgs e);
        //public static event KernelReadyEventHandler OnKernelReady;
        public delegate void UiFocusMainWindowEventHandler();

        public event UiFocusMainWindowEventHandler UiFocusMainWindow;


        public void SendSetFocusToMainWindow()
        {
            if (UiFocusMainWindow != null)
                UiFocusMainWindow();
        }
    }
}
