using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Services.PluginDirectory.Library.Objects
{
    public class SearchTerm : BaseObject
    {
        public Guid Id { get; set; }
        public Guid PluginId { get; set; }
        public String Term { get; set; }
    }
}
