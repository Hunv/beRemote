using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace beRemote.Core.Common.LogSystem
{
    internal static class LoggerEventLog
    {

        private static String source = "beRemote";
        private static String log_to = "beRemote Logs";
        private static LogEntryType level;


        public static void Init(LogEntryType logLevel)
        {
            level = logLevel;

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, log_to);
            }
            else
            {

            }


        }

        public static void Verbose(string message, int eventID)
        {
            WriteLine(LogEntryType.Verbose, EventLogEntryType.Information, message, eventID);
        }

        public static void Info(string message, int eventID)
        {
            WriteLine(LogEntryType.Info, EventLogEntryType.Information, message, eventID);
        }

        internal static void Info(string message)
        {
            WriteLine(LogEntryType.Info, EventLogEntryType.Information, message);
        }

        public static void Warning(string message)
        {
            WriteLine(LogEntryType.Warning, EventLogEntryType.Warning, message);
        }

        public static void Warning(string message, int eventID)
        {
            WriteLine(LogEntryType.Warning, EventLogEntryType.Warning, message, eventID);
        }

        public static void Error(string message, int eventID)
        {
            WriteLine(LogEntryType.Exception, EventLogEntryType.Error, message, eventID);
        }

        private static void WriteLine(LogEntryType p_level, EventLogEntryType logtype, String p_message, int eventID)
        {
            if (LineIsWriteable(p_level))
            {
                String callingMod = DetermineCallingModule();
                // TODO: event ids?
                if (!EventLog.SourceExists(callingMod))
                {
                    EventLog.CreateEventSource(callingMod, log_to);
                }

                EventLog.WriteEntry(callingMod, p_message, logtype, DetermineEventID(eventID, callingMod));
            }
        }

        private static void WriteLine(LogEntryType p_level, EventLogEntryType logtype, String p_message)
        {
            if (LineIsWriteable(p_level))
            {
                String callingMod = DetermineCallingModule();
                // TODO: event ids?
                if (!EventLog.SourceExists(callingMod))
                {
                    EventLog.CreateEventSource(callingMod, log_to);
                }

                EventLog.WriteEntry(callingMod, p_message, logtype, 0);
            }
        }


        private static String DetermineCallingModule()
        {
            String callingMod = "invalid";
            var t = new StackTrace();
            foreach (StackFrame frame in t.GetFrames())
            {
                //beRemote.Core.Exceptions.beRemoteException 
                if (false == frame.GetMethod().DeclaringType.ToString().ToLower().Contains("beRemote.Core.Common.LogSystem".ToLower()) &&
                    false == frame.GetMethod().DeclaringType.ToString().ToLower().Contains("beRemote.Core.Exceptions".ToLower()))
                {
                    // first occurence is our target
                    callingMod = frame.GetMethod().DeclaringType.ToString() + "." + frame.GetMethod().Name;
                    break;
                }
            }
            return callingMod;
        }

        private static int DetermineEventID(int eventID, String callingMod)
        {
            int returnEventId = GetPartialId(callingMod, new EventID[] { EventID.Kernel, EventID.ExceptionSystem, EventID.PluginSystem, EventID.ProtocolSystem, EventID.StorageSystem, EventID.GUI, EventID.Licensing, EventID.Common, EventID.PluginRDP, EventID.PluginTelnet, EventID.PluginVNC });

            //if (callingMod.ToLower().Contains(EventID.Kernel.ToString().ToLower()))
            //{
            //    returnEventId = (int)EventID.Kernel;
            //}

            return returnEventId + eventID;
        }

        private static int GetPartialId(string callingMod, EventID[] eventIDs)
        {
            int returnValue = (int)EventID.INVALID;
            foreach (EventID id in eventIDs)
            {
                if (callingMod.ToLower().Contains(id.ToString().ToLower()))
                {
                    returnValue = (int)id;
                }
            }

            return returnValue;
        }

        private static bool LineIsWriteable(LogEntryType p_level)
        {
            if (level == LogEntryType.Verbose || level == LogEntryType.Debug)
            {
                return true;
            }

            if (level == LogEntryType.Info)
            {
                if (p_level == LogEntryType.Info || p_level == LogEntryType.Warning || p_level == LogEntryType.Exception)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (level == LogEntryType.Warning)
            {
                if (p_level == LogEntryType.Warning || p_level == LogEntryType.Exception)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (level == LogEntryType.Exception)
            {
                if (p_level == LogEntryType.Exception)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }





        #region methods to enable backwards compatibility

        /// <summary>
        /// Logs the givven message to the specified type... OBSOLETE!! USE INSTEAD:
        /// Logger.Verbose(), Logger.Info(), Logger.Warning(), Logger.Error() !!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        [Obsolete("Logger.Log() is deprecated, please use Logger.Verbose(), Logger.Info(), Logger.Warning() or Logger.Error() instead.", false)]
        public static void Log(LogEntryType type, string message)
        {
            Log(type, message, "");
        }

        /// <summary>
        /// Logs the givven message to the specified type... OBSOLETE!! USE INSTEAD:
        /// Logger.Verbose(), Logger.Info(), Logger.Warning(), Logger.Error() !!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        [Obsolete("Logger.Log() is deprecated, please use Logger.Verbose(), Logger.Info(), Logger.Warning() or Logger.Error() instead.", false)]
        public static void Log(LogEntryType type, string message, Exception ex)
        {
            Log(type, message, ex, "");
        }

        /// <summary>
        /// Logs the givven message to the specified type... OBSOLETE!! USE INSTEAD:
        /// Logger.Verbose(), Logger.Info(), Logger.Warning(), Logger.Error() !!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        [Obsolete("Logger.Log() is deprecated, please use Logger.Verbose(), Logger.Info(), Logger.Warning() or Logger.Error() instead.", false)]
        public static void Log(LogEntryType type, string message, Exception ex, String module)
        {
            Log(type, message, "");
            Log(type, ex.ToString(), "");
        }

        /// <summary>
        /// Logs the givven message to the specified type... OBSOLETE!! USE INSTEAD:
        /// Logger.Verbose(), Logger.Info(), Logger.Warning(), Logger.Error() !!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        [Obsolete("Logger.Log() is deprecated, please use Logger.Verbose(), Logger.Info(), Logger.Warning() or Logger.Error() instead.", false)]
        public static void Log(LogEntryType type, string message, string module)
        {
            //Log(type, message, defaultContext);

            switch (type)
            {
                case LogEntryType.Debug:
                case LogEntryType.Verbose:
                    Verbose(message, 999);
                    break;
                case LogEntryType.Warning:
                    Warning(message, 999);
                    break;
                case LogEntryType.Info:
                    Info(message, 999);
                    break;
                case LogEntryType.Exception:
                    Error(message, 999);
                    break;
            }
        }

        #endregion


    }
}
