using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Services.ServiceLib.Classes;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;
using beRemote.Services.VendorServices.PluginDirectoryService.Database;

namespace beRemote.Services.VendorServices.PluginDirectoryService.Listeners.Actions
{
    [ListenerAction(ActionName = "GetJoinedList", Description = "Gets all plugins with all information joined to a list")]
    public class GetWholePluginDirectory : PluginDirectoryAction
    {
        public GetWholePluginDirectory(AbstractListener parentListener) : base(parentListener)
        {
        }

        public override void ExecuteAction(ServiceLib.Classes.ServicePlugin.ExecutionContext context)
        {
            using (var client = new Client(PluginListener.DatabaseConfig))
            {
                var pluginList = client.GetUseablePlugins(context.ClientVersion, context.Session[Key.ClientChannel].ToString());
                foreach (var plugin in pluginList)
                {
                    plugin.PluginType = client.GetPluginType(plugin.TypeId);
                    plugin.Author = client.GetAuthor(plugin.AuthorId);
                    plugin.PluginVersion = client.GetVersion(plugin.VersionId);
                    plugin.RequiredBeRemoteVersion =
                        client.GetVersion(plugin.RequiredBeRemoteVersionId);

                    //plugin.Groups = client.GetPluginGroups(plugin.Id);
                    plugin.SearchTerms = client.GetPluginSearchTerms(plugin.Id);
                }

                context.WriteJsonResponse(this.Serialize(pluginList));
            }
        }
    }
}
