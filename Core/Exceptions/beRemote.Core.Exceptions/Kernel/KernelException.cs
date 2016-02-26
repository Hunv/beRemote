using System;
using System.Collections.Generic;
using System.Text;

namespace beRemote.Core.Exceptions.Kernel
{
    public class KernelException : beRemoteException
    {
        private int evtId = 20000;
        public KernelException(beRemoteExInfoPackage info, String message, int eventId) : this(info, message, eventId, null)
        {
            
        }

        public KernelException(beRemoteExInfoPackage info, String message, int eventId, Exception ex)
            : base(info, message, ex)
        {
            evtId = EventId;
        }

        public override int EventId
        {
            get
            {
                return evtId;
            }
        }
    }
}
