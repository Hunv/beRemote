using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.VendorProtocols.SSH
{
    public struct SshConnectionInfo
    {
        public string Host;
        public string User;
        public string Pass;
        public string IdentityFile;
    }
}
