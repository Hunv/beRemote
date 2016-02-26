using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;
using beRemote.Services.VendorServices.PluginDirectoryService.Database;

namespace beRemote.Services.VendorServices.PluginDirectoryService.Listeners.Actions
{
    [ListenerAction(ActionName = "LastChangeDate", Description = "Gets the last modification time")]
    public class GetLastChangeDate : PluginDirectoryAction
    {
        public GetLastChangeDate(AbstractListener parentListener) : base(parentListener)
        {
        }

        public override void ExecuteAction(ServiceLib.Classes.ServicePlugin.ExecutionContext context)
        {
            using (var client = new Client(PluginListener.DatabaseConfig))
            {
                context.WriteJsonResponse(this.Serialize(client.GetDbConfigValue("last_change_date")));
            }
        }

    }
}
