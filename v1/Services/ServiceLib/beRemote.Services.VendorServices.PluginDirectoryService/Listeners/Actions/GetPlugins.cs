using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Services.PluginDirectory.Library.Objects;
using beRemote.Services.ServiceLib.Classes;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;
using beRemote.Services.VendorServices.PluginDirectoryService.Database;

namespace beRemote.Services.VendorServices.PluginDirectoryService.Listeners.Actions
{
    [ListenerAction(
        ActionName = "GetPlugins", Description = "Returns a list of plugins available")]
    public class GetPlugins : PluginDirectoryAction
    {
        

        public GetPlugins(AbstractListener parentListener) : base(parentListener)
        {
        }

        public override void ExecuteAction(ExecutionContext context)
        {
            // create returner
            using (Client client = new Client(PluginListener.DatabaseConfig))
            {
                var plugins = client.GetUseablePlugins(context.ClientVersion, context.Session[Key.ClientChannel].ToString());

                

                context.WriteJsonResponse(this.Serialize(plugins));
            }
        }
    }
}
