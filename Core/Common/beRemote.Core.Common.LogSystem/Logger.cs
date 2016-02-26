using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace beRemote.Core.Common.LogSystem
{
    public static class Logger
    {



        private static bool LogEventLog = false;
        private static bool LogFileLog = true;

        public static void Init(LogEntryType logEntryType, bool autoFlush, string logTextFile, bool logConsole, bool logEventLog, bool logFileLog)
        {
            LogEventLog = logEventLog;
            LogFileLog = logFileLog;

            if(LogEventLog)
                LoggerEventLog.Init(logEntryType);

            if (LogFileLog)
            {
                TraceLevel tl = TraceLevel.Info;

                switch (logEntryType)
                {
                    case LogEntryType.Verbose:                        
                    case LogEntryType.Debug:
                        tl = TraceLevel.Verbose;
                        break;
                    case LogEntryType.Info:
                        tl = TraceLevel.Info;
                        break;
                    case LogEntryType.Warning:
                        tl = TraceLevel.Warning;
                        break;
                    case LogEntryType.Exception:
                        tl = TraceLevel.Error;
                        break;
                }

                LoggerFileLog.Init(tl, autoFlush, logTextFile, logConsole);
            }

        }

        public static void Verbose(string message, int EventId)
        {
            if (LogEventLog)
                LoggerEventLog.Verbose(message, EventId);

            if (LogFileLog)
                LoggerFileLog.Verbose(message, EventId);
        }

        public static void Error(string message, int EventId)
        {
            if (LogEventLog)
                LoggerEventLog.Error(message, EventId);

            if (LogFileLog)
                LoggerFileLog.Error(message, EventId);
        }

        public static void Warning(string message, int EventId)
        {
            if (LogEventLog)
                LoggerEventLog.Warning(message, EventId);

            if (LogFileLog)
                LoggerFileLog.Warning(message, EventId);
        }

        public static void Info(string message)
        {
            if (LogEventLog)
                LoggerEventLog.Info(message);

            if (LogFileLog)
                LoggerFileLog.Info(message);
        }
        public static void Info(string message, int EventId)
        {
            if (LogEventLog)
                LoggerEventLog.Info(message, EventId);

            if (LogFileLog)
                LoggerFileLog.Info(message, EventId);
        }

        public static void Warning(string message)
        {
            if (LogEventLog)
                LoggerEventLog.Warning(message);

            if (LogFileLog)
                LoggerFileLog.Warning(message);
        }

        public static void Log(LogEntryType logEntryType, string message, string module)
        {
            if (LogEventLog)
                LoggerEventLog.Log(logEntryType, message, module);

            if (LogFileLog)
                LoggerFileLog.Log(logEntryType, message, module);
        }

        public static void Log(LogEntryType logEntryType, string message)
        {
            if (LogEventLog)
                LoggerEventLog.Log(logEntryType, message);

            if (LogFileLog)
                LoggerFileLog.Log(logEntryType, message);
        }


        public static void Log(LogEntryType logEntryType, string message, Exception ea)
        {
            if (LogEventLog)
                LoggerEventLog.Log(logEntryType, message, ea);

            if (LogFileLog)
                LoggerFileLog.Log(logEntryType, message, ea);
        }

        public static void Log(LogEntryType logEntryType, string message, Exception ea, string _loggerContext)
        {
            if (LogEventLog)
                LoggerEventLog.Log(logEntryType, message, ea, _loggerContext);

            if (LogFileLog)
                LoggerFileLog.Log(logEntryType, message, ea, _loggerContext);
        }



    }
}
