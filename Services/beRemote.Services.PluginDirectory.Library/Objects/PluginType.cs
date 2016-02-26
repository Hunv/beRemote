using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Services.PluginDirectory.Library.Objects
{
    public class PluginType : BaseObject
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
    }
}
