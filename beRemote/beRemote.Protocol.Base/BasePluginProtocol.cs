using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Protocol.Base
{    
    public interface IBasePluginProtocol
    {
        string Name { get;}
        Version Version { get; }
        string Creator { get; }
    }
}
