using System;

namespace beRemote.Core.Common.PluginBase
{
    public interface IPluginMetadata
    {


        String PluginFullQuallifiedName { get; }
        String PluginConfigFolder { get; }
        String PluginName { get; }
        int PluginVersionCode { get; }
        String PluginIniFile { get; }
        String PluginDescription { get; }
        String PluginAuthor { get; }
        String PluginAuthorMail { get; }
        String PluginWebsite { get;  } 
        PluginType PluginType { get; }

    }
}
        //String PluginName { get; }
        //String PluginFullQuallifiedName { get; }
        //int PluginVersion { get; }        
        //String PluginDescription { get; }
        //String PluginlAuthor { get; }
        //String PluginAuthorMail { get; }
        //String PluginWebsite { get; }
        //UIPluginType PluginType { get; }
        //String PluginFolder { get; }