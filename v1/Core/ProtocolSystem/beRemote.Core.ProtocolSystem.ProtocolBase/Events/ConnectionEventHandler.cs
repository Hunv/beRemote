using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Common.LogSystem;

namespace beRemote.Core.ProtocolSystem.ProtocolBase.Events
{
    public class ConnectionEventHandler : beRemote.Core.Common.EventSystem.Handler.beRemoteBaseEventListener
    {
        public override void DoWork(object o, Common.EventSystem.Events.beRemoteEventArgs e)
        {
            
            Logger.Log(LogEntryType.Warning, String.Format("Protocol event listener has recieved an event! \r\n{0}", e));
            Logger.Log(LogEntryType.Warning, String.Format("Protocol event listener has recieved an event! \r\n{0}", e));
        }
    }
}
