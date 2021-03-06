﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class WebServiceBackendMisconfigured : BERemoteException
    {
        public WebServiceBackendMisconfigured(string errorMessage)
                             : base(errorMessage) {}

        public WebServiceBackendMisconfigured(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
