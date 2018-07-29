using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace beRemote.VendorProtocols.HyperVManager.HyperV
{
    class HyperVHost
    {
        private string _Hostname;
        private string _Domain;
        private HyperVHostOS _OS = HyperVHostOS.Unknown;
        private long _Memory; //in Bytes
        private int _ProcessorCount;
        private int _NetworkAdapterCount;
        private bool _RemoteFX;
        private bool _IsHypervisorPresent;

        //Credentials for WMI-Usage
        private string _CredUsername;
        private string _CredDomain;
        private SecureString _CredPassword;

        //Hardware Information
        string _Manufacturer;
        string _Model;
        string _SerialNumber;



        public string Hostname { get { return (_Hostname); } set { _Hostname = value; } }
        public string Domain { get { return (_Domain); } set { _Domain = value; } }
        public HyperVHostOS OS { get { return (_OS); } set { _OS = value; } }
        public long Memory { get { return (_Memory); } set { _Memory = value; } }
        public int ProcessorCount { get { return (_ProcessorCount); } set { _ProcessorCount = value; } }
        public int NetworkAdapterCount { get { return (_NetworkAdapterCount); } set { _NetworkAdapterCount = value; } }
        public bool RemoteFX { get { return (_RemoteFX); } set { _RemoteFX = value; } }
        public bool IsHypervisorPresent { get { return (_IsHypervisorPresent); } set { _IsHypervisorPresent = value; } }

        public string CredUsername { get { return (_CredUsername); } set { _CredUsername = value; } }
        public string CredDomain { get { return (_CredDomain); } set { _CredDomain = value; } }
        public SecureString CredPassword { get { return (_CredPassword); } set { _CredPassword = value; } }

        public string Manufacturer { get { return (_Manufacturer); } set { _Manufacturer = value; } }
        public string Model { get { return (_Model); } set { _Model = value; } }
        public string SerialNumber { get { return (_SerialNumber); } set { _SerialNumber = value; } }

        #region Information-Methods depending on given Information
        /// <summary>
        /// Can this Hypervisor host Generation 2 Machines
        /// </summary>
        /// <returns></returns>
        public bool IsGeneration2() 
        {
            return (_OS == HyperVHostOS.HyperV2012R2 || _OS == HyperVHostOS.WindowsServer2012R2 ? true : false); 
        }

        /// <summary>
        /// Is this Host using Generation 2 Namespace?
        /// </summary>
        /// <returns></returns>
        public bool IsVirtualizationV2Namespace()
        {
            switch (_OS)
            { 
                default:
                case HyperVHostOS.WindowsServer2012R2:
                case HyperVHostOS.WindowsServer2012:
                case HyperVHostOS.HyperV2012R2:
                case HyperVHostOS.HyperV2012:
                    return(true);

                case HyperVHostOS.HyperV2008:
                case HyperVHostOS.HyperV2008R2:
                case HyperVHostOS.WindowsServer2008:
                case HyperVHostOS.WindowsServer2008R2:
                    return (false);
            }
        }
        #endregion
    }
}
