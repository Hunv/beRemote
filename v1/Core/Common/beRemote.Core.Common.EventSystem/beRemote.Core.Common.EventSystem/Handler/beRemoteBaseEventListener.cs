using System;
using System.Collections.Generic;
using System.Text;
using beRemote.Core.Common.EventSystem.Events;

namespace beRemote.Core.Common.EventSystem.Handler
{
    public abstract class beRemoteBaseEventListener
    {
        public abstract void DoWork(object o, beRemoteEventArgs e);
    }
}
