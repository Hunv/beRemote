using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core
{
    public enum KernelEventId
    {
        DefaultEventId = 20000,
        FaultedThreadInStack = 20001,
        ThreadNotFound = 20002,
        LastUserNotFound=20003

    }
}
