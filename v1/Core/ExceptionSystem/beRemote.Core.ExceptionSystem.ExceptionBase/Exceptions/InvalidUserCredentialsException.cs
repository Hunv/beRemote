using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class InvalidUserCredentialsException : BERemoteException
    {
        public InvalidUserCredentialsException(string errorMessage)
                             : base(errorMessage) {}

        public InvalidUserCredentialsException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
