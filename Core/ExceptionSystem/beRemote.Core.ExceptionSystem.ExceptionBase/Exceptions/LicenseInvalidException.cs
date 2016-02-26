using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class LicenseInvalidException : BERemoteException
    {
        public LicenseInvalidException(string errorMessage)
                             : base(errorMessage) {}

        public LicenseInvalidException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
