using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class PluginInvalidException : BERemoteException
    {
        public PluginInvalidException(string errorMessage)
                             : base(errorMessage) {}

        public PluginInvalidException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
