using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class MissingRequirementException : BERemoteGUIException
    {
        public MissingRequirementException(string errorMessage)
                             : base(errorMessage) {}

        public MissingRequirementException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
