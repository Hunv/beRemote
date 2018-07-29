using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Services.ServiceLib.Classes.ServicePlugin
{
    [MetadataAttribute]
    public class ListenerActionAttribute : Attribute
    {
        public String ActionName { get; set; }
        public String Description { get; set; }
    }
}
