using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Common.LogSystem;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Handler
{
    public class LogHandler : IHandler
    {
        private String loggerContext = "ExceptionSystem";

        public void Handle(String message)
        {
            Logger.Log(LogEntryType.Exception, message, loggerContext);
        }


        public void Handle(string message, Exception exception)
        {
            Logger.Log(LogEntryType.Exception, message, loggerContext);
            Logger.Log(LogEntryType.Exception, exception.ToString() + "\r\n", loggerContext);
            //if (exception.StackTrace != null)
            //    Logger.Log(LogEntryType.Exception, exception.StackTrace);
        }



        public string GetHandlerName()
        {
            return "LogSystem Handler";
        }


    }
}
