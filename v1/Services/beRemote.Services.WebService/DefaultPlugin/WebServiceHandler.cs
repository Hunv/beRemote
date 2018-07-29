using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using beRemote.Core.Common.PluginBase;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;
using beRemote.Services.WebService.DefaultPlugin.Services;

namespace beRemote.Services.WebService.DefaultPlugin
{
    [PluginMetadata(
        PluginName = "beRemote web service",
        PluginDescription = "beRemote web service that provides basic communication with the beRemote developers",
        PluginFullQualifiedName = "beRemote.Services.WebService.DefaultPlugin.WebServiceHandler",
        PluginConfigFolder = "."
        )]
    [Export(typeof(AbstractServicePlugin))]
    public class WebServiceHandler : AbstractServicePlugin
    {
        public WebServiceHandler()
        {
            this.RegisterListener(new SessionService(this));

        }
    }
}