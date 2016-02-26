using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using beRemote.Core.Common.PluginBase;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.ProtocolSystem.ProtocolBase.Declaration;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using System.Windows.Media;
using System.IO;
using beRemote.Core.Common.SimpleSettings;
using beRemote.Core.Exceptions.Plugin.Protocol;
using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Plugin;
using System.ComponentModel.Composition;

namespace beRemote.Core.ProtocolSystem.ProtocolBase
{
    /// <summary>
    /// Base class for a new Protocol
    /// </summary>
    public abstract class Protocol : Plugin
    {
        private Boolean _enabled;
        private String _protocolIdentifier = "invalid";

        private IniFile _propertyMappings;

        /// <summary>
        /// Static image cache. Key consits of: <protocol-fqn>-<icon-type>
        /// </summary>
        private static Dictionary<String, ImageSource> iconCache;

        private string loggerContext = "ProtocolSystem";


        private SortedList<String, ProtocolSetting> _protocolSettings;

        /// <summary>
        /// Returns all compatible ServerTypes defined in Common.Types.ServerType
        /// </summary>
        public abstract Types.ServerType[] GetPrtocolCompatibleServers();

        public abstract Session NewSession(IServer server, long connSettingsID);

        #region Basics for protocol

        public Protocol()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => { iconCache = new Dictionary<String, ImageSource>(); });
        }

        public void InitiateProtocol(String baseDirectory)
        {
            Logger.Log(LogEntryType.Info, String.Format("Initiating protocol {0} in version {1}", this.GetProtocolName(), this.GetProtocolVersion()), loggerContext);
            
            // adding settings
            InitiateProtocolSettings();
        }

        /// <summary>
        /// Initiate Protocol settings with default values
        /// </summary>
        private void InitiateProtocolSettings()
        {
            _protocolSettings = new SortedList<string, ProtocolSetting>();
            FileInfo _propertyMappingsIniFile = new FileInfo(this.Config.GetDirectory() + "\\mapping.ini");

            if (!_propertyMappingsIniFile.Exists)
                throw new ProtocolConfigurationException(beRemoteExInfoPackage.SignificantInformationPackage, "Could not find mappings file for protocol setting mappings. (FileNotFound: " + _propertyMappingsIniFile.FullName + ")");

            _propertyMappings = new IniFile(_propertyMappingsIniFile);

            foreach(String key in Config.GetSection(IniSection.SETTINGS_CUSTOM).Keys)
            {
                String dataTypeString = _propertyMappings.GetValue(IniSection.SETTINGS_MAPPING_DATATYPES, key);
                bool multiple = false;
                if (dataTypeString.ToLower().Contains("multiple"))
                {
                    dataTypeString = dataTypeString.Split(new String[] { ";" }, StringSplitOptions.None)[1];
                    multiple = true;
                }
                Type dataType;

                if (Type.GetType(dataTypeString) != null)
                    dataType = Type.GetType(dataTypeString, false, true);
                else if (Type.GetType("System." + dataTypeString, false, true) != null)
                    dataType = Type.GetType("System." + dataTypeString, false, true);
                else if (dataTypeString.Contains("credential"))
                    dataType = typeof(UserCredential);
                else
                    throw new TypeAccessException("This System.Type is not known (" + dataTypeString + ")");

                ProtocolSetting ps  = new ProtocolSetting(key, _propertyMappings.GetValue(IniSection.SETTINGS_MAPPING_DISPLAYTEXT, key), "description", dataType);
                ps.SetDefaultValue(new DefinedProtocolSettingValue(Config.GetValue(IniSection.SETTINGS_CUSTOM, key) + GetDefinedValueTitle(key) , Config.GetValue(IniSection.SETTINGS_CUSTOM, key)));
                if (multiple)
                {
                    String[] definedValues = _propertyMappings.GetValue(IniSection.SETTINGS_MAPPING_DEFINEDVALUES, key).Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (String value in definedValues)
                    {
                        DefinedProtocolSettingValue dv = new DefinedProtocolSettingValue(value + GetDefinedValueTitle(key), value);
                        ps.AddDefinedProtocolSettingValue(dv);
                    }
                }

                _protocolSettings.Add(key, ps);
            }
        }

        private string GetDefinedValueTitle(string key)
        {
            String s = _propertyMappings.GetValue(IniSection.SETTINGS_MAPPING_DEFINEDVALUES_TITLE, key);

            if (s != null || s != "")
                return " " + s;
            else
                return "";
        }

        /// <summary>
        /// Returns a Dictionary that contains all Settings of this Protocol
        /// <b>Note: </b> Theses Settings are default settings. Do not use this for existing sessions
        /// </summary>
        /// <returns></returns>
        public SortedList<String, ProtocolSetting> GetProtocolSettings()
        {
            return _protocolSettings;
        }

        public void TestProtocol()
        {
            Logger.Log(LogEntryType.Debug, "Testing protocol...");
            // Just execute some methods to check that they are implemented and OK
            this.GetProtocolDescription();
            this.GetProtocolIcon(IconType.HIGHDEF);
            this.GetProtocolName();
            this.GetProtocolVersion();
            this.GetPrtocolCompatibleServers();
            this.GetDefaultProtocolPort();

            foreach (String key in Config.GetSection(IniSection.SETTINGS_CUSTOM).Keys)
            {
                GetPropertyDataTypeString(key);
                GetPropertyDisplayText(key);
            }
            Logger.Log(LogEntryType.Debug, "... test finished!");
        }       

        /// <summary>
        /// Returns the DisplayText of the given property
        /// </summary>
        /// <param name="key">The property key</param>
        /// <exception cref="ProtocolInvalidSettingsException"></exception>
        public string GetPropertyDisplayText(string key)
        {
            String ret = null;

            ret = Config.GetValue(IniSection.SETTINGS_MAPPING_DISPLAYTEXT, key);

            if (ret == null)
                throw new ProtocolConfigurationException(beRemoteExInfoPackage.MajorInformationPackage, "Some displaytext mappings return NULL. NULL is invalid as displaytext! (key: " + key + ")");

            return ret;
        }

        /// <summary>
        /// Gets the DataType for the given key (only string representation, not the System.Type!)
        /// </summary>
        /// <param name="key">The property key</param>
        /// <exception cref="ProtocolInvalidSettingsException"></exception>   
        public string GetPropertyDataTypeString(String key)
        {
            String ret = null;

            ret = Config.GetValue(IniSection.SETTINGS_MAPPING_DATATYPES, key);

            if (ret == null)
                throw new ProtocolConfigurationException(beRemoteExInfoPackage.MajorInformationPackage, "Some datatype mappings return NULL. NULL is invalid as DataType! (key: " + key + ")");

            return ret;
        }



        #endregion

        #region UI

        private String GetProtocolIconCacheKey(IconType type)
        {
            return GetType() + "-" + type;
        }


        /// <summary>
        /// Gets the protocols default icon.
        /// </summary>
        /// <param name="type">The desired icon quality. If the sspecified size is not available we will try it with a smaller image</param>
        /// <exception>
        ///     <cref>ProtocolInvalidIconException</cref>
        /// </exception>
        public ImageSource GetProtocolIcon(IconType type)
        {
            if (iconCache.ContainsKey(GetProtocolIconCacheKey(type)))
            {
                return iconCache[GetProtocolIconCacheKey(type)];
            }

            var filename = "{0}{1}." + GetSetting(IniSection.INFORMATION, IniKey.PROTOCOL_ICON_EXTENSION);
            FileInfo IconFile = null;
            switch (type)
            {
                case IconType.SMALL:
                    filename = String.Format(filename, GetSetting(IniSection.INFORMATION, IniKey.PROTOCOL_ICON_PREFIX), "16");
                    break;
                case IconType.MEDIUM:
                    filename = String.Format(filename, GetSetting(IniSection.INFORMATION, IniKey.PROTOCOL_ICON_PREFIX), "32");
                    break;
                case IconType.LARGE:
                    filename = String.Format(filename, GetSetting(IniSection.INFORMATION, IniKey.PROTOCOL_ICON_PREFIX), "64");
                    break;
                case IconType.HIGHDEF:
                    filename = String.Format(filename, GetSetting(IniSection.INFORMATION, IniKey.PROTOCOL_ICON_PREFIX), "128");
                    break;
            }
            IconFile = new FileInfo(Config.GetDirectory() + "\\res\\" + filename);

            Logger.Log(LogEntryType.Verbose, String.Format("Trying to load {0} from path: {1}", filename, IconFile.FullName), loggerContext);

            if (IconFile.Exists)
            {
                var imsc = new ImageSourceConverter();
                try
                {
                    ImageSource ims = null;
                    Application.Current.Dispatcher.Invoke(() =>
                                                          {
                                                              ims = (ImageSource)imsc.ConvertFromString(IconFile.FullName);
                                                              Logger.Log(LogEntryType.Verbose, String.Format("Protocol icon loaded and converted, returning it now..."), loggerContext);

                                                              if (ims == null) return;

                                                              //Required for Multithreaded processing where this icon is used
                                                              ims.Freeze();

                                                              //Check again, if Icon really not exists
                                                              if (!iconCache.ContainsKey(GetProtocolIconCacheKey(type)))
                                                                  iconCache.Add(GetProtocolIconCacheKey(type), ims);
                                                          });
                    return ims;
                }
                catch (Exception ex)
                {
                    throw new ProtocolConfigurationException(beRemoteExInfoPackage.SignificantInformationPackage, "Error while loading image: " + IconFile.Name, ex);
                }
            }
            else
            {
                Logger.Log(LogEntryType.Verbose, "The given icon type(" + type + ") could not be loaded. Trying it with lower resolution", loggerContext);

                var returnIcon = GetProtocolIcon_LowerType(type);

                //Required for Multithreaded processing where this icon is used
                returnIcon.Freeze();
                return returnIcon;
            }
        }

        /// <summary>
        /// Gets an icon that is one step lower in resolution as the type of the given paremter
        /// </summary>
        private ImageSource GetProtocolIcon_LowerType(IconType currentType)
        {
            switch (currentType)
            {
                case IconType.SMALL:
                    throw new ProtocolConfigurationException(beRemoteExInfoPackage.SignificantInformationPackage, "The given icon size is not available: " + currentType.ToString());
                case IconType.MEDIUM:
                    return GetProtocolIcon(IconType.SMALL);
                case IconType.LARGE:
                    return GetProtocolIcon(IconType.MEDIUM);
                case IconType.HIGHDEF:
                    return GetProtocolIcon(IconType.LARGE);
                default:
                    throw new ProtocolConfigurationException(beRemoteExInfoPackage.SignificantInformationPackage, "Problem determinig lower type: " + currentType.ToString());
            }
        }

        #endregion

        #region Get and set settings and informations
        /// <summary>
        /// Returns the default port for this protocol that is defined in the configuration 
        /// <b>Note:</b> This port number could be overwritten by the session that uses this protocol. 
        /// </summary>
        /// <exception cref="ProtocolInvalidPortException">Is thrown if the protocol port is not defined or invalid</exception>
        public int GetDefaultProtocolPort()
        {

            int t = System.Convert.ToInt32(this.Config.GetValue(Declaration.IniSection.SETTINGS_BASE
                , Declaration.IniKey.PROTOCOL_PORT));
            
            if (t == 0)
                throw new ProtocolException(beRemoteExInfoPackage.SignificantInformationPackage, String.Format("The given protocol port is not valid!"));
            else
                return t;
        }

        /// <summary>
        /// Identifies if this protocol is enabled or not
        /// </summary>        
        public bool IsProtocolEnabled()
        {
            return Boolean.Parse(GetBaseSetting(IniKey.PROTOCOL_ENABLED));
        }

        /// <summary>
        /// Identifies if this protocol is set as favorite
        /// </summary>                
        public bool IsProtocolFavorite()
        {
            return Boolean.Parse(GetBaseSetting(IniKey.PROTOCOL_IS_FAVORITE));
        }


        public String GetSetting(String section, String key)
        {
            String setting = Config.GetValue(section, key);

            if (setting == "")
                throw new PluginConfigurationException(beRemoteExInfoPackage.MinorInformationPackage, "Setting " + key + " is missing");

            return setting;
        }

        private String GetBaseSetting(String Key)
        {
            String setting = this.GetSetting(IniSection.SETTINGS_BASE, Key);

            if (setting == "")
                throw new PluginConfigurationException(beRemoteExInfoPackage.MajorInformationPackage, "Setting " + Key + " is missing");

            return setting;
        }
        private String GetCustomSetting(String Key)
        {
            String setting = this.GetSetting(IniSection.SETTINGS_CUSTOM, Key);

            if (setting == "")
                throw new PluginConfigurationException(beRemoteExInfoPackage.MajorInformationPackage, "Setting " + Key + " is missing");

            return setting;
        }
      
        /// <summary>
        /// Returns the protocolname
        /// </summary>        
        public string GetProtocolName()
        {
            return MetaData.PluginName;
        }

        /// <summary>
        /// Return the complete protocol description
        /// </summary>        
        public string GetProtocolDescription()
        {
            return MetaData.PluginDescription;
        }

        /// <summary>
        /// Returns the version of this protocol
        /// </summary>        
        public string GetProtocolVersion()
        {
            return MetaData.PluginVersionCode.ToString();
        }

        /// <summary>
        /// Sets the default protocol port. 
        /// <b>Note:</b> This setting is persistent. 
        /// <b>Note:</b> This port number could be overwritten by the session that uses this protocol. 
        /// </summary>
        /// <param name="port"></param>
        public void SetDefaultProtocolPort(int port)
        {
            this.Config.SetValue(Declaration.IniSection.SETTINGS_BASE, Declaration.IniKey.PROTOCOL_PORT, port.ToString(), true);
        }

        public void SetCustomSetting(String Key, String value)
        {
            Config.SetValue(IniSection.SETTINGS_CUSTOM, Key, value, true);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the Displayname of the Protocol
        /// </summary>
        public string ProtocolName
        {
            get { return MetaData.PluginName; }
        }

        /// <summary>
        /// Gets the Small Icon of the Protocol (16x16)
        /// </summary>
        public ImageSource ProtocolIconSmall
        {
            get { return GetProtocolIcon(IconType.SMALL); }
        }

        /// <summary>
        /// Gets the Medium Icon of the Protocol (32x32)
        /// </summary>
        public ImageSource ProtocolIconMedium
        {
            get { return GetProtocolIcon(IconType.MEDIUM); }
        }

        /// <summary>
        /// Gets the large Icon of the Protocol (64x64)
        /// </summary>
        public ImageSource ProtocolIconLarge
        {
            get { return GetProtocolIcon(IconType.LARGE); }
        }

        /// <summary>
        /// Gets the high Definition Icon of the Protocol (128x128)
        /// </summary>
        public ImageSource ProtocolIconHighDef
        {
            get { return GetProtocolIcon(IconType.HIGHDEF); }
        }

        /// <summary>
        /// Gets the internal Identifier of this Protocol
        /// </summary>
        public string ProtocolIdentifier
        {
            get { return GetProtocolIdentifer(); }
        }


        #endregion

        public override string ToString()
        {
            String retVal = base.ToString() + "\r\n";

            retVal += String.Format("{0} - {1} \r\n{2}", this.GetProtocolName(), this.GetProtocolVersion(), this.GetProtocolDescription());
            
            return retVal;
        }

        /// <summary>
        /// Returns a sorted list with the basic settings for this protocol.
        /// <b>Note: </b>This settings will be overwritten by the session if there are any.
        /// </summary>
        public SortedList<String, String> GetBaseSettings()
        {
            return Config.GetSection(IniSection.SETTINGS_CUSTOM);
        }

        public void SetProtocolIdentifier(String identifier)
        {
            _protocolIdentifier = identifier;
        }

        public String GetProtocolIdentifer()
        {
            if (_protocolIdentifier == "invalid" || _protocolIdentifier == null)
            {
                throw new PluginException(beRemoteExInfoPackage.SignificantInformationPackage,"Protocolidentifer is null or invalid!");
            }

            return _protocolIdentifier;
        }

        public Boolean IsInDebugMode()
        {
            bool debug_enabled = System.Convert.ToBoolean(GetBaseSetting(IniKey.PROTOCOL_DEBUG));

            if (debug_enabled)
                Logger.Log(LogEntryType.Warning, "Protocol is in debug mode. This may result in massive log output");

            return debug_enabled;
        }


        public override string GetPluginIdentifier()
        {
            return this.GetProtocolIdentifer();
        }
    }


}
