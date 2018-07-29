using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Common.LogSystem;

namespace beRemote.Core.ProtocolSystem.ProtocolBase
{
    public class ProtocolSetting
    {
        private String _section;
        private String _key;
        private String _title;
        private String _description;
        private String _regEx;

        private Type _dataType;
        private DefinedProtocolSettingValue _value; // default value or selected value
        private List<DefinedProtocolSettingValue> _definedValues; // possible values used by this Setting

        public ProtocolSetting(String key, String title, String description, Type datatype)
        {
            Logger.Log(LogEntryType.Verbose, String.Format("Initiating new ProtocolSetting ({0})", key), "ProtocolSystem");

            this._key = key;
            this._title = title;
            this._dataType = datatype;
            this._definedValues = null;
            this._description = description;
        }

        public ProtocolSetting(String key, String title, String description, Type datatype, String section)
            : this(key, title, description, datatype)
        {
            this._section = section;
        }

        public ProtocolSetting(String key, String title, String description, Type datatype, List<DefinedProtocolSettingValue> definedValues)
            : this(key, title,description, datatype)
        {
            this._definedValues = definedValues;
        }

        public ProtocolSetting(String key, String title, String description, Type datatype, List<DefinedProtocolSettingValue> definedValues, String section)
            : this(key, title, description, datatype, definedValues)
        {
            this._section = section;
        }

        public ProtocolSetting(String key, String title, String description, Type datatype, List<DefinedProtocolSettingValue> definedValues, String section, String regEx)
            : this(key, title, description, datatype, definedValues)
        {
            this._regEx = regEx;
        }

        /// <summary>
        /// Adds a defined value to the list of defined values
        /// </summary>
        /// <param name="value">The value to add</param>
        public void AddDefinedProtocolSettingValue(DefinedProtocolSettingValue value)
        {
            if (_definedValues == null)
                _definedValues = new List<DefinedProtocolSettingValue>();
            _definedValues.Add(value);
        }

        /// <summary>
        /// Sets the default value for this ProtocolSetting
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetDefaultValue(DefinedProtocolSettingValue value)
        {
            _value = value;
        }


        /// <summary>
        /// Get the Section-Name
        /// </summary>
        /// <returns>Section-Name</returns>
        public string GetSection()
        {
            return (_section);
        }

        /// <summary>
        /// Get the Title 
        /// </summary>
        /// <returns>Title-Name</returns>
        public string GetTitle()
        {
            return (_title);
        }

        /// <summary>
        /// Get the internal Settingname
        /// </summary>
        /// <returns>internal Settingname</returns>
        public string GetKey()
        {
            return (_key);
        }

        /// <summary>
        /// Get the DataType of the Value
        /// </summary>
        /// <returns>Datatype</returns>
        public Type GetDataType()
        {
            return (_dataType);
        }

        /// <summary>
        /// The predefined Value
        /// </summary>
        /// <returns>NULL if none is set, else the predefined Value</returns>
        public DefinedProtocolSettingValue GetProtocolSettingValue()
        {
            return (_value);
        }

        /// <summary>
        /// The Description of the Setting
        /// </summary>
        /// <returns>Descriptiontext</returns>
        public string GetDescription()
        {
            return (_description);
        }

        /// <summary>
        /// A List of Values that are predefined for a Combobox
        /// </summary>
        /// <returns>NULL if no Combobox is set, else a List of all Comboboxitems</returns>
        public List<DefinedProtocolSettingValue> GetDefinedValues()
        {
            return (_definedValues);
        }

        /// <summary>
        /// The RegEx-Command for the values that can be entered in the Value-Field of the DataGrid
        /// </summary>
        /// <returns>RegEx-String</returns>
        public string GetRegEx()
        {
            return (_regEx);
        }

        /// <summary>
        /// Set the Value for this element
        /// </summary>
        public DefinedProtocolSettingValue Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
