using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class SessionInvalidException : BERemoteException
    {
        public SessionInvalidException(string errorMessage)
                             : base(errorMessage) {}

        public SessionInvalidException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
