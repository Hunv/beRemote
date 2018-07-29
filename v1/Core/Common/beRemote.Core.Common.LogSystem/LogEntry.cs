using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace beRemote.Core.Common.LogSystem
{
    /// <summary>
    /// Types of LogEntries.
    /// </summary>
    public enum LogEntryType
    {
        /// <summary>
        /// Verbose.
        /// </summary>
        Verbose,
        /// <summary>
        /// Debug.
        /// </summary>
        Debug,
        /// <summary>
        /// Info.
        /// </summary>
        Info,        
        /// <summary>
        /// Warning.
        /// </summary>
        Warning,
        /// <summary>
        /// Exception.
        /// </summary>
        Exception,

    }
    /// <summary>
    /// The Entry
    /// </summary>
    public struct LogEntry
    {
        /// <summary>
        /// Current type
        /// </summary>
        public LogEntryType Type;

        /// <summary>
        /// current message
        /// </summary>
        private string Text;

        /// <summary>
        /// The context in that this message will be presented
        /// </summary>
        private String Context;

        /// <summary>
        /// log entry time
        /// </summary>
        public DateTime Timestamp;

        public Exception ExceptionObject;

        /// <summary />
        /// <param name="type">Type of message</param>
        /// <param name="text">Message content</param>
        public LogEntry(LogEntryType type, string text, String context)
        {
            this.Type = type;
            this.Text = text;
            this.Timestamp = DateTime.Now;
            this.ExceptionObject = null;
            this.Context = context;
        }

        public LogEntry(LogEntryType type, string text, Exception ex, String context)
        {
            this.Type = type;
            this.Text = text;
            this.Timestamp = DateTime.Now;
            this.ExceptionObject = ex;
            this.Context = context;
        }

        public String GetContext()
        {
            return Context;
        }

        public LogEntryType GetEntryType()
        {
            return Type;
        }

        public DateTime GetTimestamp()
        {
            return Timestamp;
        }

        public String GetMessage()
        {
            return Text;
        }

        /// <summary>
        /// Generates type and message as String result
        /// </summary>
        public override string ToString()
        {
            int typeWhiteSpace = 30;            

            String whitespacedType = this.Type.ToString() + String.Format(" ({0})", Context); ;

            int typeChars = whitespacedType.Length;

            for (int i = 0; i < (typeWhiteSpace - typeChars); i++)
            {
                whitespacedType += " ";
            }

            String msg = whitespacedType + ": " + this.Text;

            if (ExceptionObject != null)
            {
                msg += "\r\n";
                msg += ExceptionObject.ToString();
                msg += "\r\n";
            }

            return msg;
        }

    }
}
