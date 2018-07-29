using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class SessionIdInvalidException : BERemoteException
    {
         public SessionIdInvalidException(string errorMessage, Guid invalidId)
                             : base(errorMessage) {}

         public SessionIdInvalidException(string errorMessage, Guid invalidId, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
