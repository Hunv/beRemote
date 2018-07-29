using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;
using beRemote.Services.VendorServices.PluginDirectoryService.Database;

namespace beRemote.Services.VendorServices.PluginDirectoryService.Listeners.Actions
{
    [ListenerAction(ActionName = "GetAuthors", Description = "Returns a List of authors")]
    public class GetAuthors : PluginDirectoryAction
    {
        public GetAuthors(AbstractListener parentListener) : base(parentListener)
        {
        }

        public override void ExecuteAction(ExecutionContext context)
        {
            using (Client client = new Client(PluginListener.DatabaseConfig))
            {
                context.WriteJsonResponse(this.Serialize(client.GetAllAuthors()));
            }
        }
    }
}
