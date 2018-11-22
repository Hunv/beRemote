using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using beRemote.Protocol.Base;

namespace beRemote.Protocol.Rdp
{
    [Export(typeof(IBasePluginProtocol))]
    public class Rdp : IBasePluginProtocol
    {
        public string Name { get; private set; } = "RDP";
        public Version Version { get; private set; } = new Version(0, 0, 1, 1);
        public string Creator { get; private set; } = "Kristian Reukauff";
    }
}
