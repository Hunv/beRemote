using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class ProtocolInvalidPortException : BERemoteException
    {
         public ProtocolInvalidPortException(string errorMessage)
                             : base(errorMessage) {}

         public ProtocolInvalidPortException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
