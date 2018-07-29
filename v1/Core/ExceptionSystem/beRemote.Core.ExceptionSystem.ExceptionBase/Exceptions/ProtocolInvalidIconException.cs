using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class ProtocolInvalidIconException : BERemoteException
    {
         public ProtocolInvalidIconException(string errorMessage)
                             : base(errorMessage) {}

         public ProtocolInvalidIconException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
