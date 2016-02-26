using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Common.LogSystem;

namespace beRemote.Core.ProtocolSystem.ProtocolBase
{
    public class DefinedProtocolSettingValue
    {
        private Object _value;
        private String _title;
        private String _description;



        public DefinedProtocolSettingValue(String title, Object value)
        {
            this._title = title;
            this._value = value;

            Logger.Log(LogEntryType.Verbose, String.Format("Initiating new DefinedProtocolSettingValue ({0})", title), "ProtocolSystem");
        }

        public DefinedProtocolSettingValue(String title, Object value, string description)
            : this(title, value)
        {
            this._description = description;
        }

        /// <summary>
        /// Returns the Value
        /// </summary>
        /// <returns>Returns the Value in the Datatype of the given DataType in ProtocolSetting</returns>
        public Object GetValue()
        {
            return (_value);
        }

        /// <summary>
        /// Get the Title of the Value
        /// </summary>
        /// <returns></returns>
        public String GetTitle()
        {
            return (_title);
        }

        /// <summary>
        /// Get the Description
        /// </summary>
        /// <returns></returns>
        public String GetDescription()
        {
            return (_description);
        }

        public Object Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
