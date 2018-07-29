using System;
using System.Collections.Generic;
using System.Text;

namespace beRemote.Core.Exceptions
{
    public class UnhandledUIException : beRemoteException
    {
        private beRemoteExInfoPackage beRemoteExInfoPackage;


        public UnhandledUIException(beRemoteExInfoPackage infoPackage, String message)
            : base(infoPackage, message)
        {

        }

        public UnhandledUIException(beRemoteExInfoPackage infoPackage, String message, Exception innerEx)
            : base(infoPackage, message, innerEx)
        {

        }



        public UIExceptionWindow.brDialogResult ShowDialog(bool isTerminating)
        {
            var wnd = new UIExceptionWindow(this, isTerminating);
            wnd.ShowDialog();

           return wnd.DialogResult;
        }

        public override int EventId
        {
            get { return 102; }
        }
    }
}
