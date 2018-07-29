using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using beRemote.Core.Common.PluginBase;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;
using beRemote.Services.VendorServices.PluginDirectoryService.Listeners;

namespace beRemote.Services.VendorServices.PluginDirectoryService
{
    [PluginMetadata(
        PluginFullQualifiedName = "beRemote.Services.VendorServices.PluginDirectoryService.PluginDirectoryService",
        PluginConfigFolder = "beRemote.Services.VendorServices.PluginDirectoryService",
        PluginName = "Plugin Directory",
        PluginDescription = "Service for browsing all available and approved plugins for the beRemote client",
        PluginVersionCode = 1,
        PluginAuthor = "beRemote Team <Benedikt Kröning>",
        PluginAuthorMail = "benediktkroening@beremote.net",
        PluginWebsite = "www.beremote.net")]
    [Export(typeof(AbstractServicePlugin))]
    public class PluginDirectoryService : AbstractServicePlugin
    {
        public PluginDirectoryService()
        {
            this.RegisterListener(new PluginListener(this));
        }
    }
}
