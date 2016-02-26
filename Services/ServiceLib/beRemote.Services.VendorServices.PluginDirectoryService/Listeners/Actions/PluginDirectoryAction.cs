using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;

namespace beRemote.Services.VendorServices.PluginDirectoryService.Listeners.Actions
{
    public abstract class PluginDirectoryAction : AbstractListenerAction
    {
        public PluginListener PluginListener
        {
            get { return (PluginListener)Listener; }
        }

        public PluginDirectoryAction(AbstractListener parentListener) : base(parentListener)
        {
        }

        public override abstract void ExecuteAction(ExecutionContext context);
    }
}
