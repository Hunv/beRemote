using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;
using beRemote.Services.VendorServices.PluginDirectoryService.Database;
using beRemote.Services.VendorServices.PluginDirectoryService.Listeners.Actions;

namespace beRemote.Services.VendorServices.PluginDirectoryService.Listeners
{
    [ListenerMetadataAttribute(
        Id = "94596C46-7AD5-4F38-84A9-D133D1BE57EE",
        Listener = "Plugin",
        Name="Listener for interacting with plugin related information")]
    public class PluginListener : AbstractListener
    {
        private GetPlugins _getPluginAction;
        private GetWholePluginDirectory _getWholePluginDirectory;
        private DownloadPlugin _downloadPlugin;

        public DatabaseConfig DatabaseConfig
        {
            get
            {
                var dc = (DatabaseConfig)this.Session["db-conf"];

                if(dc == null)
                    this.Session["db-conf"] = DatabaseConfig.Load(new FileInfo(BaseDirectory.FullName + "\\database.yml"));

                return (DatabaseConfig)this.Session["db-conf"];

            }
        }

        public PluginListener(AbstractServicePlugin plugin) : base(plugin)
        {
            _getPluginAction = new GetPlugins(this);
            _getWholePluginDirectory = new GetWholePluginDirectory(this);
            _downloadPlugin = new DownloadPlugin(this);
            ListenerActions.Add(_getPluginAction);
            ListenerActions.Add(_getWholePluginDirectory);
            ListenerActions.Add(_downloadPlugin);
        }


    }
}
