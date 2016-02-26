using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;
using beRemote.Services.VendorServices.PluginDirectoryService.Database;

namespace beRemote.Services.VendorServices.PluginDirectoryService.Listeners.Actions
{
    [ListenerAction(
        ActionName = "DownloadPlugin", Description = "Lets an authenticated client download the given client")]
    public class DownloadPlugin : PluginDirectoryAction
    {
        public DownloadPlugin(AbstractListener parentListener) : base(parentListener)
        {
        }

        public override void ExecuteAction(ServiceLib.Classes.ServicePlugin.ExecutionContext context)
        {
            if (context.RequestParameters.ContainsKey("PluginId") && context.RequestParameters.ContainsKey("PluginBitness"))
            {
                using (Client client = new Client(PluginListener.DatabaseConfig))
                {
                    var plugin = client.GetPlugin(context.RequestParameters["PluginId"]);
                    if(false == plugin.IsProvisioned)
                        throw new Exception("The requested plugin is not available for your client.");

                    String bitness = (context.RequestParameters["PluginBitness"].Contains("64")) ? "x64" : "x86";

                    //provisioningpath always contains a placeholder for the bitness otherwise it will fail
                    String provPath = client.GetProvisioningPath(plugin.ProvisioningId);

                    if (File.Exists(String.Format(provPath, bitness)))
                        context.WriteFileResponse(new FileInfo(String.Format(provPath, bitness)));
                    else
                    {
                        throw new FileNotFoundException(plugin.Name + " update container not found on server");
                    }
                }

            }
            else
            {
                throw new Exception("Missing plugin-id parameter");
            }
        }
    }
}
