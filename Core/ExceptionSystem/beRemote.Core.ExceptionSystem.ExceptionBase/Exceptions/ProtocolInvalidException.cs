using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class ProtocolInvalidException : BERemoteException
    {
         public ProtocolInvalidException(string errorMessage)
                             : base(errorMessage) {}

         public ProtocolInvalidException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
