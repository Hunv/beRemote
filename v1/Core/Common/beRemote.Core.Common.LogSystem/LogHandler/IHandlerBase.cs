using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Common.SimpleSettings;

namespace beRemote.Core.Common.LogSystem.LogHandler
{
    public interface IHandlerBase
    {
        /// <summary>
        /// Triggers a new logentry
        /// </summary>
        /// <param name="meldung">The new message.</param>
        void SendEntry(LogEntry message);

        /// <summary>
        /// Returns array of types that will be handled with this logger
        /// </summary>
        LogEntryType[] GetHandledTypes();

        /// <summary>
        /// Only used to initiate the Handler. (Done by the logging system) 
        /// You can do whatever you want in this code part!
        /// </summary>
        /// <param name="configuration">the prefix for the configuration </param>
        void InitiateLogHandler(IniFile configuration);

        Type GetClassLock();

        void PauseLogger();
        void UnpauseLogger();

        void Stop();
    }
}
