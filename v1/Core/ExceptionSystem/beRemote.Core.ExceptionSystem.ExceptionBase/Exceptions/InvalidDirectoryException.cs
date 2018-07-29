using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class InvalidDirectoryException : BERemoteException
    {
        public InvalidDirectoryException(string errorMessage)
                             : base(errorMessage) {}

        public InvalidDirectoryException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
