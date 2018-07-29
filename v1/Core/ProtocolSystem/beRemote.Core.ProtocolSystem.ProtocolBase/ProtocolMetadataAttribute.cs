using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Common.PluginBase;

namespace beRemote.Core.ProtocolSystem.ProtocolBase
{
    [MetadataAttribute]
    public class ProtocolMetadataAttribute : PluginMetadataAttribute
    {
        public bool ProtocolAuthIsHandled { get; set; }
    }
}
