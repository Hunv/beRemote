using System;
using System.Collections.Generic;
using System.Text;

namespace beRemote.Core.Exceptions.Kernel
{
    public class FaultedThreadException : beRemoteException
    {
        public FaultedThreadException(beRemoteExInfoPackage info, String message)
            : base(info, message)
        {
        }

        public override int EventId
        {
            get { return 20001; }
        }
    }
}
