using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.Common.Vpn.WVPN
{
    public enum WindowsVpnErrorCode
    {
        ConnectionEstablished = 0,
        NotExists = 623,
        UserOrPasswordIncorrect = 691,
        ServerNotAvailabe = 789
    }
}
