using System;
using System.Collections.Generic;
using System.Text;
using beRemote.Core.Common.EventSystem.Events;
using beRemote.Core.Common.EventSystem.Handler;

namespace beRemote.Core.Common.EventSystem
{
    public class EventManager
    {
        #region static
        private static EventManager _instance;

        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventManager();

                return _instance;
            }
        } 
        #endregion

        private event ConnectionStateChangedEventHandler _connectionStateChangedEvent;

        public void AddHandler(EventHandlerType type, beRemoteBaseEventListener handler)
        {
            switch (type)
            {
                case EventHandlerType.ConnectionStateChanged:
                    _connectionStateChangedEvent +=new ConnectionStateChangedEventHandler(handler.DoWork);
                    break;
            }
        }

        public void OnEvent(EventHandlerType type, beRemoteEventArgs e)
        {
            
            switch (type)
            {
                case EventHandlerType.ConnectionStateChanged:
                    if (_connectionStateChangedEvent != null)
                        _connectionStateChangedEvent(new object(), (ConnectionStateChangedEventArgs)e);
                    break;
            }
        }
    }
}
