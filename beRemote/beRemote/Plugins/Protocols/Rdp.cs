using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Plugins.Protocols
{
    public class Rdp : IProtocolBase
    {
        public string Name { get; private set; } = "Remote Desktop Protocol";
    }
}
