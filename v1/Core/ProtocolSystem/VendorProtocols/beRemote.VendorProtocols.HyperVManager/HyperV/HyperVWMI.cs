/****************************************
 * This class is the base-class for all *
 * communication with the hostsystem.   *
 * Every WMI-Query will be run by this  *
 * class.                               *
 * The classes _v1 and _v2 will be      *
 * queried by this class too.           *
 ****************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Management;
using beRemote.Core.Common.LogSystem;
using beRemote.VendorProtocols.HyperVManager.HyperV.v2;
using System.Collections.ObjectModel;

namespace beRemote.VendorProtocols.HyperVManager.HyperV
{
    class HyperVWMI
    {
        private ManagementScope _MgmScopeCIMV2; //The Scope to get Hostsystem-Information
        private ManagementScope _MgmScopeVirtualization; //The Scope to get Virtual System information
        private HyperVHost _Host = new HyperVHost(); //The Object, containing the Host-Information
        private string _LoggerContext = "HyperVManager"; //The Context in the Loggingsystem
        private bool _IsLocalhost = false; //If the connection was established to localhost, this will be true
        private string _Hostname = ""; //The hostname, if it is not a localhost-connection
        private ConnectionOptions _ConnectionOptions; //If it is not a localhost-connection, the Connection-Credentials will be stored here

        #region Constructor
        /// <summary>
        /// Query local System
        /// </summary>
        public HyperVWMI()
        {
            _MgmScopeCIMV2 = new ManagementScope("\\\\localhost\\root\\cimv2");
            _IsLocalhost = true;
            getHostInformation(); //gets the information about the HyperV-Host
        }

        /// <summary>
        /// Query a remotesystem
        /// </summary>
        /// <param name="host">Hostname or IP of the target system</param>
        /// <param name="user">Username (local or Domain)</param>
        /// <param name="domain">Domain (only if member of domain)</param>
        /// <param name="password">Password of the (domain-)user</param>
        public HyperVWMI(string host, string user, string domain, SecureString password)
        {
            _Host.CredUsername = user;
            _Host.CredDomain = domain;
            _Host.CredPassword = password;
            _Hostname = host;

            if (_Hostname == "localhost" || _Hostname == "::1")
                _Hostname = "127.0.0.1";

            ConnectionOptions conOptions = new ConnectionOptions();
            conOptions.Username = user;
            conOptions.SecurePassword = password;
            conOptions.Impersonation = ImpersonationLevel.Impersonate;
            conOptions.Authentication = AuthenticationLevel.PacketPrivacy;
            if (domain != "") 
                conOptions.Authority = "ntlmdomain:" + domain;

            _ConnectionOptions = conOptions;

            DateTime dtLog = DateTime.Now;
            Logger.Log(LogEntryType.Verbose, "Creating WMI-Scope CIMV2");
            _MgmScopeCIMV2 = new ManagementScope("\\\\" + host + "\\root\\cimv2", conOptions);
            Logger.Log(LogEntryType.Verbose, "WMI-Scope created in " + DateTime.Now.Subtract(dtLog).TotalMilliseconds.ToString() + "ms");

            dtLog = DateTime.Now;
            Logger.Log(LogEntryType.Verbose, "Getting Host Information");
            getHostInformation(); //gets the information about the HyperV-Host
            Logger.Log(LogEntryType.Verbose, "Host Information gathered in " + DateTime.Now.Subtract(dtLog).TotalMilliseconds.ToString() + "ms");
        }
        #endregion

        /// <summary>
        /// Starts the loading of data via WMI
        /// </summary>
        public ObservableCollection<HyperVMachine> GetData()
        {
            if (_Host.IsHypervisorPresent == true)
            {
                if (_Host.IsVirtualizationV2Namespace()) //New HyperV
                {
                    if (_IsLocalhost == false)
                        _MgmScopeVirtualization = new ManagementScope("\\\\" + _Hostname + "\\root\\virtualization\\v2", _ConnectionOptions);
                    else
                        _MgmScopeVirtualization = new ManagementScope("\\\\localhost\\root\\virtualization\\v2");

                    return(HyperVWMI_v2.GetVirtualMachineList(_MgmScopeVirtualization, _Host.OS));
                }
                else //Old HyperV
                {
                    if (_IsLocalhost == false)
                        _MgmScopeVirtualization = new ManagementScope("\\\\" + _Hostname + "\\root\\virtualization", _ConnectionOptions);
                    else
                        _MgmScopeVirtualization = new ManagementScope("\\\\localhost\\root\\virtualization");

                    //Remember: Generation is always 1
                    return (null);
                }
            }
            else
            {
                Logger.Log(LogEntryType.Info, "The remotesystem is not a HyperV-Hypervisor", _LoggerContext); //ID: 41001
            }
            return (null);
        }

        /// <summary>
        /// Gets the information about the Hypervisor
        /// </summary>
        private bool getHostInformation()
        {
            try
            {
                DateTime dtLog = DateTime.Now;
                #region Read Win32_ComputerSystem
                SelectQuery query = new SelectQuery("SELECT DNSHostName,Domain,HypervisorPresent,Manufacturer,Model,NumberOfLogicalProcessors,PartOfDomain,TotalPhysicalMemory,Workgroup FROM Win32_ComputerSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(_MgmScopeCIMV2, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    try
                    {
                        _Host.Hostname = queryObj["DNSHostName"].ToString();
                        _Host.Manufacturer = queryObj["Manufacturer"].ToString();
                        _Host.Model = queryObj["Model"].ToString();
                        _Host.ProcessorCount = Convert.ToByte(queryObj["NumberOfLogicalProcessors"]);
                        _Host.Memory = Convert.ToInt64(queryObj["TotalPhysicalMemory"]);
                        _Host.IsHypervisorPresent = Convert.ToBoolean(queryObj["HypervisorPresent"]);

                        if (Convert.ToBoolean(queryObj["PartOfDomain"]) == true)
                            _Host.Domain = queryObj["Domain"].ToString();
                        else
                            _Host.Domain = queryObj["Workgroup"].ToString();
                    }
                    catch (Exception ea)
                    {
                        Logger.Log(LogEntryType.Warning, "Cannot get Hypervisor (Win32_ComputerSystem) host information via WMI.", ea, _LoggerContext); //ID: 41000
                    }
                }
                Logger.Log(LogEntryType.Info, "ComputerSystem Information gathered after " + DateTime.Now.Subtract(dtLog).TotalMilliseconds.ToString() + "ms", _LoggerContext);
                #endregion

                #region Read Win32_OperatingSystem
                query = new SelectQuery("SELECT Caption, BuildNumber FROM Win32_OperatingSystem");
                searcher = new ManagementObjectSearcher(_MgmScopeCIMV2, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    try
                    {
                        string caption = queryObj["Caption"].ToString();

                        byte osType = 2; //0 = Windows Server, 1 = HyperV, 2 = Windows
                        if (caption.Contains("Microsoft Hyper-V")) //Check if it is a full Windows Server or a free Hyper-V-Server
                            osType = 1;
                        else if (caption.Contains("Server"))
                            osType = 0;

                        int build = Convert.ToInt32(queryObj["BuildNumber"]);

                        if (build >= 9600) //Win2012R2 / HV2012R2 / Win81
                        {
                            switch (osType)
                            {
                                case 0:
                                    _Host.OS = HyperVHostOS.WindowsServer2012R2;
                                    break;
                                case 1:
                                    _Host.OS = HyperVHostOS.HyperV2012R2;
                                    break;
                                case 2:
                                    _Host.OS = HyperVHostOS.Windows81;
                                    break;
                            }
                        }
                        else if (build >= 9200) //Win2012 / HV2012 /Win8
                        {
                            switch (osType)
                            {
                                case 0:
                                    _Host.OS = HyperVHostOS.WindowsServer2012R2;
                                    break;
                                case 1:
                                    _Host.OS = HyperVHostOS.HyperV2012R2;
                                    break;
                                case 2:
                                    _Host.OS = HyperVHostOS.Windows81;
                                    break;
                            }
                        }
                        else if (build >= 7600) //Win2008R2 / HV2008R2
                        {
                            _Host.OS = (osType == 1 ? HyperVHostOS.HyperV2012 : HyperVHostOS.WindowsServer2012);
                        }
                        else if (build >= 6000) //Win2008 / HV2098
                        {
                            _Host.OS = (osType == 1 ? HyperVHostOS.HyperV2012 : HyperVHostOS.WindowsServer2012);
                        }
                        else
                        {
                            _Host.OS = HyperVHostOS.Unknown;
                        }

                    }
                    catch (Exception ea)
                    {
                        Logger.Log(LogEntryType.Warning, "Cannot get Hypervisor (Win32_OperatingSystem) host information via WMI.", ea, _LoggerContext); //ID: 41002
                    }
                }
                Logger.Log(LogEntryType.Verbose, "OperationgSystem Information gathered after " + DateTime.Now.Subtract(dtLog).TotalMilliseconds.ToString() + "ms");
                #endregion

                #region Read Win32_NetworkAdapter
                query = new SelectQuery("SELECT Description FROM Win32_NetworkAdapter WHERE PhysicalAdapter=True");
                searcher = new ManagementObjectSearcher(_MgmScopeCIMV2, query);

                try
                {
                    _Host.NetworkAdapterCount = searcher.Get().Count;
                }
                catch (Exception ea)
                {
                    Logger.Log(LogEntryType.Warning, "Cannot get Hypervisor (Win32_NetworkAdapter) host information via WMI.", ea, _LoggerContext); //ID: 41003
                }
                Logger.Log(LogEntryType.Verbose, "NetworkAdapter Information gathered after " + DateTime.Now.Subtract(dtLog).TotalMilliseconds.ToString() + "ms");
                #endregion

                return (true);
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Could not gather HostInformation via WMI. Is the Server available, WMI activated and the Firewall configured?", ea, _LoggerContext);
                return (false);
            }
        }
    }
}
