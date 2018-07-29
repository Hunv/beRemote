using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.ExceptionSystem.ExceptionBase.Handler;

namespace beRemote.Core.ExceptionSystem.ExceptionBase
{
    public class ExceptionHandler
    {
        private static ExceptionHandler _instance;
        private List<Handler.IHandler> _exceptionHandlers;
        private String loggerContext = "ExceptionSystem";
        public static ExceptionHandler GetInstance()
        {
            if (_instance == null)
                _instance = new ExceptionHandler();

            return _instance;
        }

        public ExceptionHandler()
        {
            _exceptionHandlers = new List<Handler.IHandler>();

            AddHandler(new Handler.LogHandler());

            Logger.Log(LogEntryType.Debug, String.Format("ExceptionSystem has {0} registred ExceptionHandler:", _exceptionHandlers.Count), loggerContext);
            foreach (IHandler handler in _exceptionHandlers)
            {
                Logger.Log(LogEntryType.Debug, String.Format("... {0} ", handler.GetHandlerName()), loggerContext);
               
            }
        }

        public void AddHandler(Handler.IHandler handler)
        {
           _exceptionHandlers.Add(handler);
        }

        public void Handle(String message)
        {
            foreach (Handler.IHandler handler in _exceptionHandlers)
            {
                handler.Handle(message);
            }
        }

        public void Handle(String message, Exception exception)
        {
            foreach (Handler.IHandler handler in _exceptionHandlers)
            {
                handler.Handle(message, exception);
            }
        }

    }
}
