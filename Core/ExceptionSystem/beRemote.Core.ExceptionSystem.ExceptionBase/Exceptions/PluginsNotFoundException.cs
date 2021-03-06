﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    public class PluginsNotFoundException : BERemoteException
    {
       public PluginsNotFoundException(string errorMessage)
                             : base(errorMessage) {}

       public PluginsNotFoundException(string errorMessage, Exception innerEx)
                             : base(errorMessage, innerEx) {}
    }
}
