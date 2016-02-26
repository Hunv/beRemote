using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class ProtocolInvalidSettingsException : BERemoteException
    {
         public ProtocolInvalidSettingsException(string errorMessage)
                             : base(errorMessage) {}

         public ProtocolInvalidSettingsException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
