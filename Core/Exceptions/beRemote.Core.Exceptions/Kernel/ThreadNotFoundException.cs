using System;
using System.Collections.Generic;
using System.Text;

namespace beRemote.Core.Exceptions.Kernel
{
    public class ThreadNotFoundException : beRemoteException
    {
        public ThreadNotFoundException(String message) : base(beRemoteExInfoPackage.MajorInformationPackage, message) { }

        public override int EventId
        {
            get { return 20002; }
        }
    }
}
