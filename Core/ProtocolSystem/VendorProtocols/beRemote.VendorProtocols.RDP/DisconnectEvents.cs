using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.VendorProtocols.RDP
{
    public static class DisconnectEvents
    {
        private static Dictionary<int, RdpError> _EventDescription = new Dictionary<int, RdpError>
        {    
            //For most of the Codes see: https://msdn.microsoft.com/en-us/library/windows/desktop/aa382170(v=vs.85).aspx
            {0, new RdpError(0 , "No information is available.", false)},
            {1, new RdpError(1 , "Local disconnection. This is not an error code.", false)},            
            {2, new RdpError(2 , "Remote disconnection by user. This is not an error code.", true)},            
            {3, new RdpError(3 ,"Remote disconnection by server. This is not an error code." , true)},
            {260, new RdpError(260 ,"DNS name lookup failure." , false)},
            {262, new RdpError(262 ,"Out of memory." , false)},
            {264, new RdpError(264 , "Connection timed out.", false)},
            {516, new RdpError(516 , "Windows Sockets connect failed.", false)},
            {518, new RdpError(518 ,"Out of memory." , false)},
            {520, new RdpError(520 ,"Host not found error." , false)},
            {772, new RdpError(772 ,"Windows Sockets send call failed." , false)},
            {774, new RdpError(774 ,"Out of memory." , false)},
            {776, new RdpError(776 ,"The IP address specified is not valid." , false)},
            {1028, new RdpError(1028 ,"Windows Sockets recv call failed." , false)},
            {1030, new RdpError(1030 ,"Security data is not valid." , false)},
            {1032, new RdpError(1032 , "Internal error." , false)},
            {1286, new RdpError(1286 , "The encryption method specified is not valid.", false)},
            {1288, new RdpError(1288 , "DNS lookup failed.", false)},
            {1540, new RdpError(1540 ,"Windows Sockets gethostbyname call failed." , false)},
            {1542, new RdpError(1542 ,"Server security data is not valid." , false)},
            {1544, new RdpError(1544 ,"Internal timer error." , false)},
            {1796, new RdpError(1796 , "Time-out occurred." , false)},
            {1798, new RdpError(1798 , "Failed to unpack server certificate.", false)},
            {2052, new RdpError(2052 ,"Bad IP address specified." , false)},
            {2055, new RdpError(2055 , "Login failed.", false)},
            {2056, new RdpError(2056 ,"License negotiation failed. Delete Registrykey HKLM\\Software\\Microsoft\\MSLicensing or HKLM\\Software\\Wow6432Node\\Microsoft\\MSLicensing" , false)},
            {2308, new RdpError(2308 ,"Socket closed." , false)},
            {2310, new RdpError(2310 ,"Internal security error." , false)},
            {2312, new RdpError(2312 ,"Licensing time-out." , false)},
            {2566, new RdpError(2566 ,"Internal security error." , false)},
            {2567, new RdpError(2567 , "The specified user has no account.", false)},
            {2822, new RdpError(2822 , "Encryption error.", false)},
            {2823, new RdpError(2823 ,"The account is disabled." , false)},
            {3078, new RdpError(3078 , "Decryption error.", false)},
            {3079, new RdpError(3079 , "The account is restricted.", false)},
            {3080, new RdpError(3080 ,"Decompression error." , false)},
            //3334: see http://www.coderewind.com/2014/10/solution-rdcman-error-3334-remote-desktop-manager/
            {3334, new RdpError(3334 , "Too less free Memory availabe. If you are running a 64 Bit-System: Try the 64 Bit-Version of beRemote, if you are running the 32 Bit-Version", false)},
            {3335, new RdpError(3335 ,"The account is locked out." , false)},
            {3591, new RdpError(3591 ,"The account is expired." , false)},
            {3847, new RdpError(3847 ,"The password is expired." , false)},
            {4615, new RdpError(4615 , "The user password must be changed before logging on for the first time.", false)},
            {5639, new RdpError(5639 ,"The policy does not support delegation of credentials to the target server." , false)},
            {5895, new RdpError(5895 ,"Delegation of credentials to the target server is not allowed unless mutual authentication has been achieved." , false)},
            {6151, new RdpError(6151 , "No authority could be contacted for authentication. The domain name of the authenticating party could be wrong, the domain could be unreachable, or there might have been a trust relationship failure.", false)},
            {6919, new RdpError(6919 , "The received certificate is expired.", false)},
            {7175, new RdpError(7175 ,"An incorrect PIN was presented to the smart card." , false)},
            {8455, new RdpError(8455 , "The server authentication policy does not allow connection requests using saved credentials. The user must enter new credentials.", false)},            
            {8711, new RdpError(8711 , "The smart card is blocked.", false)}
            
        };

        public static Dictionary<int, RdpError> EventDescription 
        {
            get { return _EventDescription; }
            set { _EventDescription = value; }
        }
    }    
}
