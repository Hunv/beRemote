using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class SessionViewSizeInvalidException : BERemoteException
    {
        public SessionViewSizeInvalidException(string errorMessage)
                             : base(errorMessage) {}

        public SessionViewSizeInvalidException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}

    }
}
