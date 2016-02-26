
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace beRemote.Core.Common.LogSystem
{
    internal static class LoggerFileLog
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
        private const string LOG_FORMAT = "{0} {1,-7} [{2}] {3}";
        private static TraceSwitch traceSwitch = new TraceSwitch(
            "beRemote Log", "Switch in beRemote Logger");

        #region Properties

        /// <summary>
        /// Gets or sets the trace level that determines the messages the 
        /// switch allows.
        /// </summary>
        public static TraceLevel LogLevel
        {
            get { return traceSwitch.Level; }
            set { traceSwitch.Level = value; }
        }

        /// <summary>
        /// Gets or sets whether Flush should be called on the Listeners after 
        /// every write.
        /// </summary>
        public static bool AutoFlush
        {
            get { return Trace.AutoFlush; }
            set { Trace.AutoFlush = value; }
        }

        private static ConsoleTraceListener consoleListener;
        private static bool logConsole = false;
        /// <summary>
        /// Gets or sets a value indicating whether logging to the console is
        /// enabled or not.
        /// </summary>
        public static bool LogConsole
        {
            get { return logConsole; }
            set
            {
                if (logConsole != value)
                {
                    logConsole = value;

                    // detach existing console listener 
                    if (null != consoleListener)
                    {
                        if (Trace.Listeners.Contains(consoleListener))
                        {
                            try
                            {
                                Trace.Listeners.Remove(consoleListener);
                            }
                            catch (Exception e)
                            {                                
                                Error("Error detaching ConsoleTraceListener from Trace: {0}",
                                    e.Message);
                            }
                        }

                        consoleListener = null;
                    }

                    try
                    {
                        consoleListener = new ConsoleTraceListener();
                        Trace.Listeners.Add(consoleListener);
                    }
                    catch (Exception e)
                    {
                        Error("Error adding ConsoleTraceListener to Trace: " +
                            "{0}. Console logging will be disabled.", e.Message);
                        logConsole = false;
                        return;
                    }
                    Verbose("ConsoleTraceListener attached to ");
                }
            }
        }

        private static TextWriterTraceListener logFileListener = null;
        private static string logTextFile = null;
        /// <summary>
        /// Gets or sets the path to a logfile. Setting the value to null will
        /// disable textfile logging.
        /// </summary>
        public static string LogTextFile
        {
            get { return logTextFile; }
            set
            {
                string cleanValue = string.IsNullOrEmpty(value) ? value :
                    Path.GetFullPath(Environment.ExpandEnvironmentVariables(value));

                if (logTextFile != cleanValue)
                {
                    logTextFile = cleanValue;

                    // detach existing text writer listener 
                    if (null != logFileListener)
                    {
                        if (Trace.Listeners.Contains(logFileListener))
                        {
                            try
                            {
                                Trace.Listeners.Remove(logFileListener);
                            }
                            catch (Exception e)
                            {
                                Error("Error detaching TextWriterTraceListener from Trace: {0}",
                                    e.Message);
                            }
                        }

                        logFileListener = null;
                    }

                    if (false == string.IsNullOrEmpty(logTextFile))
                    {
                        // ensure the path to the file exists
                        string logFile = Environment.ExpandEnvironmentVariables(logTextFile);
                        string directory = Path.GetDirectoryName(logFile);
                        if (false == Directory.Exists(directory))
                        {
                            try
                            {
                                Directory.CreateDirectory(directory);
                            }
                            catch (Exception e)
                            {
                                Error("Error creating parent directory for the " +
                                    "logfile at {0}: {1}. Textfile logging will be disabled.",
                                    logFile, e.Message);
                                logTextFile = null;
                                return;
                            }
                        }
                        //else
                        //{
                        //    // start a new file
                        //    try
                        //    {
                        //        if (File.Exists(logFile))
                        //            File.Delete(logFile);
                        //    }
                        //    catch (Exception e)
                        //    {
                        //        Error("Error cleaning up existing logfile at {0}:" +
                        //            "{1}. Textfile logging will be disabled.", logFile,
                        //            e.Message);
                        //        logTextFile = null;
                        //        return;
                        //    }
                        //}

                        try
                        {
                            //logFileListener = new TextWriterTraceListener(logFile);
                            logFileListener = new CyclicTextWriterTraceListener(logFile);
                            Trace.Listeners.Add(logFileListener);
                        }
                        catch (Exception e)
                        {
                            Error("Error adding TextWriterTraceListener to Trace: " +
                                "{0}. Textfile logging will be disabled.", e.Message);
                            logTextFile = null;
                            return;
                        }
                        Verbose("TextWriterTraceListener attached to ");
                    }
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes the logger using the supplied parameters.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="autoFlush"></param>
        /// <param name="logTextFile"></param>
        /// <param name="logConsole"></param>
        public static void Init(TraceLevel logLevel, bool autoFlush, string logTextFile, bool logConsole)
        {
            LogLevel = logLevel;
            AutoFlush = autoFlush;
            LogTextFile = logTextFile;
            LogConsole = logConsole;

            TraceLine(TraceLevel.Info, "Logger attached");
        }

        #region methods to enable backwards compatibility

        /// <summary>
        /// Logs the givven message to the specified type... OBSOLETE!! USE INSTEAD:
        /// Verbose(), Info(), Warning(), Error() !!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        [Obsolete("Log() is deprecated, please use Verbose(), Info(), Warning() or Error() instead.", false)]
        public static void Log(LogEntryType type, string message)
        {
            Log(type, message, "");
        }

        /// <summary>
        /// Logs the givven message to the specified type... OBSOLETE!! USE INSTEAD:
        /// Verbose(), Info(), Warning(), Error() !!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        [Obsolete("Log() is deprecated, please use Verbose(), Info(), Warning() or Error() instead.", false)]
        public static void Log(LogEntryType type, string message, Exception ex)
        {
            Log(type, message, ex, "");
        }

        /// <summary>
        /// Logs the givven message to the specified type... OBSOLETE!! USE INSTEAD:
        /// Verbose(), Info(), Warning(), Error() !!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        [Obsolete("Log() is deprecated, please use Verbose(), Info(), Warning() or Error() instead.", false)]
        public static void Log(LogEntryType type, string message, Exception ex, String module)
        {
            Log(type, message, "");
            Log(type, ex.ToString(), "");
        }

        /// <summary>
        /// Logs the givven message to the specified type... OBSOLETE!! USE INSTEAD:
        /// Verbose(), Info(), Warning(), Error() !!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        [Obsolete("Log() is deprecated, please use Verbose(), Info(), Warning() or Error() instead.", false)]
        public static void Log(LogEntryType type, string message, string module)
        {
            //Log(type, message, defaultContext);

            switch (type)
            {
                case LogEntryType.Debug:
                case LogEntryType.Verbose:
                    Verbose(message);
                    break;
                case LogEntryType.Warning:
                    Warning(message);
                    break;
                case LogEntryType.Info:
                    Info(message);
                    break;
                case LogEntryType.Exception:
                    Error(message);
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Writes a warning message to the log using the specified array of 
        /// objects and formatting information.
        /// </summary>
        /// <param name="format">A format string that contains zero or more 
        /// format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects 
        /// to format.</param> 
        [Conditional("TRACE")]
        public static void Warning(string format, params object[] args)
        {
            Warning(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        /// <summary>
        /// Writes a warning message to the log using the specified message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        [Conditional("TRACE")]
        public static void Warning(string message)
        {
            if (traceSwitch.TraceWarning)
                TraceLine(TraceLevel.Warning, message);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified array of 
        /// objects and formatting information.
        /// </summary>
        /// <param name="format">A format string that contains zero or more 
        /// format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects 
        /// to format.</param>
        [Conditional("TRACE")]
        public static void Verbose(string format, params object[] args)
        {
            Verbose(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        [Conditional("TRACE")]
        public static void Verbose(string message)
        {
            if (traceSwitch.TraceVerbose)
                TraceLine(TraceLevel.Verbose, message);
        }

        /// <summary>
        /// Writes an info message to the log using the specified array of 
        /// objects and formatting information.
        /// </summary>
        /// <param name="format">A format string that contains zero or more 
        /// format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects 
        /// to format.</param>
        [Conditional("TRACE")]
        public static void Info(string format, params object[] args)
        {
            Info(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        /// <summary>
        /// Writes an info message to the log using the specified message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        [Conditional("TRACE")]
        public static void Info(string message)
        {
            if (traceSwitch.TraceInfo)
                TraceLine(TraceLevel.Info, message);
        }

        /// <summary>
        /// Writes an error message to the log using the specified array of 
        /// objects and formatting information.
        /// </summary>
        /// <param name="format">A format string that contains zero or more 
        /// format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects 
        /// to format.</param>
        [Conditional("TRACE")]
        public static void Error(string format, params object[] args)
        {
            Error(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        /// <summary>
        /// Writes an error message to the log using the specified message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        [Conditional("TRACE")]
        public static void Error(string message)
        {
            if (traceSwitch.TraceError)
                TraceLine(TraceLevel.Error, message);
        }

        #endregion

        #region Private methods

        private static void TraceLine(TraceLevel level, string message)
        {
            try
            {
                Trace.WriteLine(string.Format(LOG_FORMAT,
                    DateTime.Now.ToString(DATE_FORMAT), level,
                    Thread.CurrentThread.ManagedThreadId, message));
            }
            catch (Exception)
            {
                // ignore
            }
        }

        #endregion

        private class CyclicTextWriterTraceListener : TextWriterTraceListener
        {
            public string BaseFileName { get; set; }

            public long ThresholdSize { get; set; }

            public int NumFiles { get; set; }

            public CyclicTextWriterTraceListener(string baseFileName)
                : base()
            {
                this.BaseFileName = Path.GetFullPath(baseFileName);
                ThresholdSize = 1000000;
                NumFiles = 3;
                CycleLogs();
            }

            private string GetFileName(int idx)
            {
                if (0 == idx)
                {
                    return BaseFileName;
                }
                else
                {
                    return string.Format("{0}\\{1}-{2}{3}",
                        Path.GetDirectoryName(BaseFileName), Path.GetFileNameWithoutExtension(BaseFileName),
                        idx, Path.GetExtension(BaseFileName));
                }
            }

            private void CycleLogs()
            {
                try
                {
                    // rollover logs
                    if (null != base.Writer)
                    {
                        base.Writer.Flush();
                        base.Writer.Close();
                    }

                    for (int i = NumFiles; i > 0; i--)
                    {
                        if (File.Exists(GetFileName(i)))
                        {
                            File.Delete(GetFileName(i));
                        }

                        if (File.Exists(GetFileName(i - 1)))
                        {
                            File.Move(GetFileName(i - 1), GetFileName(i));
                        }
                    }

                    base.Writer = TextWriter.Synchronized(new StreamWriter(BaseFileName));
                }
                catch (Exception e)
                {
                    Error("Error cycling logfiles: {0}.", e.Message);
                }
            }

            public override void WriteLine(string message)
            {
                this.Write(string.Format("{0}{1}", message, Environment.NewLine));
            }

            public override void Write(string message)
            {
                if (File.Exists(BaseFileName) && new FileInfo(BaseFileName).Length > this.ThresholdSize)
                {
                    CycleLogs();
                }

                base.Write(message);
            }
        }
  

    }
}
