using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class PluginInvalidSettingsException : BERemoteException
    {
         public PluginInvalidSettingsException(string errorMessage)
                             : base(errorMessage) {}

         public PluginInvalidSettingsException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
