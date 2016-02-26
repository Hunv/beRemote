using System;
using System.ComponentModel.Composition;

namespace beRemote.Core.Common.PluginBase
{
    [MetadataAttribute]
    public class PluginMetadataAttribute : Attribute
    {
        public String PluginFullQualifiedName { get; set; }
        public String PluginConfigFolder { get; set; }
        public String PluginName { get; set; }
        public int PluginVersionCode { get; set; }
        public String PluginIniFile { get; set; }
        public String PluginDescription { get; set; }
        public String PluginAuthor { get; set; }
        public String PluginAuthorMail { get; set; }
        public String PluginWebsite { get; set; }
        public PluginType PluginType { get; set; }
        

    }
}
        //public String PluginName { get; set; }
        //public String PluginFullQuallifiedName { get; set; }
        //public int PluginVersion { get; set; }
        //public String PluginDescription { get; set; }
        //public String PluginlAuthor { get; set; }
        //public String PluginAuthorMail { get; set; }
        //public String PluginWebsite { get; set; }
        //public UIPluginType PluginType { get; set; }
        //public String PluginFolder { get; set; }