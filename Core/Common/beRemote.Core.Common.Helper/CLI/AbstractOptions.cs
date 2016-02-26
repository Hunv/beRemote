using System;
using System.Collections.Generic;
using System.Reflection;

namespace beRemote.Core.Common.Helper.CLI
{
    public class AbstractOptions
    {
        private readonly Dictionary<String, MemberInfo> _memberMapping = new Dictionary<string, MemberInfo>();


        private readonly Dictionary<String, CommandLineOptionAttribute> _attributeMapping = new Dictionary<string, CommandLineOptionAttribute>();

        private readonly List<CommandLineOptionAttribute> _availableAttributes = new List<CommandLineOptionAttribute>();

        public static List<AbstractOptions> UsedInstances = new List<AbstractOptions>();

        public String[] GetHelpInfo()
        {
            var res = new List<String>();
            foreach (var map in _availableAttributes)
            {
                res.Add(String.Join(", ", map.KeyCollection) + ":   " + map.Description);
            }

            return res.ToArray();
        }

        public AbstractOptions()
        {
            ParseMemberInfos(this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static));
            ParseMemberInfos(this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static));
            ParseMemberInfos(this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static));

            UsedInstances.Add(this);
          
        }

        private void ParseMemberInfos(MemberInfo[] members)
        {
            foreach (var member in members)
            {
                foreach (var attribute in member.GetCustomAttributes(typeof(CommandLineOptionAttribute), true))
                {
                    _availableAttributes.Add((CommandLineOptionAttribute)attribute);

                    foreach (String key in ((CommandLineOptionAttribute)attribute).KeyCollection)
                    {
                        if(false == _memberMapping.ContainsKey(key))
                            _memberMapping.Add(key, member);
                        if (false == _attributeMapping.ContainsKey(key))
                            _attributeMapping.Add(key, (CommandLineOptionAttribute)attribute);

                    }

                }
            }
        }

        public void ParseArguments(String[] args)
        {
            CommandLineOptionAttribute currentAttribute = null;
            MemberInfo currentMember = null;
            String currentKey = "";
            foreach (var arg in args)
            {
                if (arg.StartsWith("-") || arg.StartsWith("/"))
                {
                    if (arg.StartsWith("-?") || arg.StartsWith("-help") || arg.StartsWith("/?") ||
                        arg.StartsWith("/help"))
                    {
                        var cloa = new CommandLineOptionAttribute(new[] {"?", "help"}, false,
                            "Displays a message box with help information for CLI");


                        _availableAttributes.Add(cloa);
                        _attributeMapping.Add("?", cloa);
                        _attributeMapping.Add("help", cloa);

                    }
                    else
                    {



                        #region Work on key

                        // we are working on a key
                        currentKey = arg.Remove(0, 1);
                        if (_attributeMapping.ContainsKey(currentKey))
                        {
                            currentAttribute = _attributeMapping[currentKey];
                        }
                        else
                        {
                            // TODO: do something that invokes a fail
                        }

                        if (_memberMapping.ContainsKey(currentKey))
                        {
                            currentMember = _memberMapping[currentKey];
                        }
                        else
                        {
                            // TODO: do something that invokes a fail
                        }

                        #endregion
                    }
                }
                else
                {
                    // this is a value
                    if (currentAttribute != null && currentMember != null)
                    {
                        if (currentAttribute.ValueNeeded)
                        {
                            // this is a value
                            switch (currentMember.MemberType)
                            {
                                case MemberTypes.Field:
                                    ((FieldInfo)currentMember).SetValue(this, arg);
                                    currentAttribute.Value = arg;
                                    break;
                                case MemberTypes.Property:
                                    ((PropertyInfo)currentMember).SetValue(this, arg);
                                    currentAttribute.Value = arg;
                                    break;
                            }
                        }
                        else
                        {
                            // we need to trigger a method
                            throw new NotImplementedException("Method invocation from CLI is not yet implemented");
                        }
                    }
                    else
                    {
                        // TODO: do something to fail
                    }
                }


            }
        }

        /// <summary>
        /// Checks if the given instance continas the specified parameter. This will only return true if the fiven parameter was issued by cli
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public bool ContainsParameter(string parameterName)
        {
            return GetParameter(parameterName) != null;
        }

        public CommandLineOptionAttribute GetParameter(string parameterName)
        {
            foreach (var option in _availableAttributes)
            {
                foreach (var param in option.KeyCollection)
                {
                    if (param.ToLower().Equals(parameterName.ToLower()))
                    {
                        //if(option.Value != null)
                            return option;
                    }
                }
            }

            return null;
        }

        public List<CommandLineOptionAttribute> GetParameters()
        {
            return _availableAttributes;
        }

        public String GetParameterValue(string parameterName)
        {
            return "";
        }
    }
}
