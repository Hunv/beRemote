using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Common.PluginBase;

namespace beRemote.Services.ServiceLib.Classes.ServicePlugin
{
    [MetadataAttribute]
    public class ListenerMetadataAttribute : Attribute
    {
        /// <summary>
        /// An unique ID for this Service. This is mostly used for debugging purposes
        /// </summary>
        public String Id { get; set; }
        /// <summary>
        /// The human readable name of this service
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// The Listener on wich this service will respond. <br />
        /// If you define 'SampleService' as listener the service will be triggered by:<br/>
        /// http[s]://your-server.local/your/folder/Service.aspx?Service=SampleService&action=your-key
        /// </summary>
        public String Listener { get; set; }

    }
}
