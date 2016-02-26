using System;

namespace beRemote.Core.Common.Helper.CLI
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field)]
    public class CommandLineOptionAttribute : Attribute
    {
        public String[] KeyCollection { get; private set; }
        public Boolean ValueNeeded { get; private set; }
        public String Description { get; private set; }
        
        public String Value { get; set; }

        /// <param name="keyCollection">Contains all useable parameters for this option</param>
        /// <param name="valueNeeded">Specifies if this attribute need a value (required for the parsing process)</param>
        public CommandLineOptionAttribute(String[] keyCollection, Boolean valueNeeded)
        {
            
            KeyCollection = keyCollection;
            ValueNeeded = valueNeeded;
            Description = "No description given";
        }

        /// <param name="keyCollection">Contains all useable parameters for this option</param>
        /// <param name="valueNeeded">Specifies if this attribute need a value (required for the parsing process)</param>
        /// <param name="description">The description for this option</param>
        public CommandLineOptionAttribute(String[] keyCollection, Boolean valueNeeded, String description)
        {
            KeyCollection = keyCollection;
            ValueNeeded = valueNeeded;
            Description = description;
        }
    }
}
