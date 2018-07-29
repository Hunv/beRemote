using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Common.SimpleSettings;

namespace beRemote.Core.Common.LogSystem.LogHandler
{
    public class ConsoleLogger : IHandlerBase
    {
        #region ILogger Member
        /// <summary>
        /// Triggers a new messageline
        /// </summary>
        /// <param name="message">Logentry</param>
        public void SendEntry(LogEntry message)
        {
            Console.WriteLine(message.Timestamp.ToString("HH:mm:ss.ffffff") + " : " +  message.ToString());            
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <returns>-</returns>
        public List<LogEntry> GetEntries()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
        
        public LogEntryType[] GetHandledTypes()
        {
            return new LogEntryType[] { LogEntryType.Verbose, LogEntryType.Debug, LogEntryType.Info, LogEntryType.Warning, LogEntryType.Exception };
        }


        public void InitiateLogHandler(IniFile configuration)
        {
           // emptry block
        }


        public Type GetClassLock()
        {
            return typeof(ConsoleLogger);
        }

        #region IHandlerBase Member


        public void PauseLogger()
        {
            // there is no reason to stop this logger as this writes to std-out
            SendEntry(new LogEntry(LogEntryType.Info, "Pausing logging system (Only if PauseLogger method is implemented", "LogSystem"));
        }

        #endregion

        #region IHandlerBase Member


        public void UnpauseLogger()
        {
            
        }

        #endregion


        public void Stop()
        {
            

        }
    }
}
