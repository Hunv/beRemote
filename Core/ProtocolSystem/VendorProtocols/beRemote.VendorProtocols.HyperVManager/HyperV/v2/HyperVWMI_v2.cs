using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;
using System.Text;
using beRemote.Core.Common.LogSystem;

namespace beRemote.VendorProtocols.HyperVManager.HyperV.v2
{
    public static class HyperVWMI_v2
    {
        private static string _LoggerContext = "HyperVManager";

        public static ObservableCollection<HyperVMachine> GetVirtualMachineList(ManagementScope scope, HyperVHostOS operatingSystem)
        {
            ObservableCollection<HyperVMachine> virtualMachines = new ObservableCollection<HyperVMachine>();

            #region Msvm_ComputerSystem
            //Select all virtual systems. Physical system doesn't have value "OnTimeInMilliseconds" in this namespace
            SelectQuery query = new SelectQuery("SELECT * FROM Msvm_ComputerSystem WHERE NOT OnTimeInMilliseconds = NULL");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            foreach (ManagementObject queryObj in searcher.Get())
            {
                try
                {
                    HyperVMachine newMachine = new HyperVMachine();
                    newMachine.GUID = queryObj["Name"].ToString();
                    newMachine.Machinename = queryObj["ElementName"].ToString();
                    newMachine.Description = queryObj["Description"].ToString();
                    newMachine.EnabledState = HyperVConverter.ConvertToEnabledState(Convert.ToUInt16(queryObj["EnabledState"]));
                    newMachine.FailOverReplicationType = HyperVConverter.ConvertToFailedOverReplicationType(Convert.ToUInt16(queryObj["FailedOverReplicationType"]));
                    newMachine.HealthState = HyperVConverter.ConvertToHealthState(Convert.ToUInt16(queryObj["HealthState"]));
                    newMachine.InstallDate = HyperVConverter.ConvertToDateTime(queryObj["InstallDate"].ToString());
                    newMachine.LastApplicationConsistentReplicationTime = HyperVConverter.ConvertToDateTime(queryObj["LastApplicationConsistentReplicationTime"].ToString());
                    newMachine.LastReplicationTime = HyperVConverter.ConvertToDateTime(queryObj["LastReplicationTime"].ToString());
                    newMachine.LastReplicationType = HyperVConverter.ConvertToLastReplicationType(Convert.ToUInt16(queryObj["LastReplicationType"]));
                    newMachine.NumberOfNUMANodes = Convert.ToUInt16(queryObj["NumberOfNumaNodes"]);
                    newMachine.OnTimeInMilliseconds = Convert.ToUInt64(queryObj["OnTimeInMilliseconds"]);
                    newMachine.ReplicationHealth = HyperVConverter.ConvertToReplicationHealth(Convert.ToUInt16(queryObj["ReplicationHealth"]));
                    newMachine.ReplicationMode = HyperVConverter.ConvertToReplicationMode(Convert.ToUInt16(queryObj["ReplicationMode"]));
                    newMachine.ReplicationState = HyperVConverter.ConvertToReplicationState(Convert.ToUInt16(queryObj["ReplicationState"]));
                    newMachine.Status = queryObj["Status"].ToString();
                    newMachine.StatusDescriptions = (string[])queryObj["StatusDescriptions"];
                    newMachine.TimeOfLastConfigurationChange = HyperVConverter.ConvertToDateTime(queryObj["TimeOfLastConfigurationChange"].ToString());
                    newMachine.TimeOfLastStateChange = HyperVConverter.ConvertToDateTime(queryObj["TimeOfLastStateChange"].ToString());

                    //Operational Status Handling
                    UInt16[] OperationalStatus = (UInt16[])queryObj["OperationalStatus"];
                    if (OperationalStatus[0] == 2)
                    {
                        newMachine.OperationalStatus[0] = HyperVOperationalStatus.OK;
                    }
                    else if (OperationalStatus.Length > 1)
                    {
                        newMachine.OperationalStatus[0] = HyperVConverter.ConvertToOperationalStatus(OperationalStatus[0]);
                        newMachine.OperationalStatus[1] = HyperVConverter.ConvertToOperationalStatus(OperationalStatus[1]);
                    }
                    else
                    {
                        newMachine.OperationalStatus[0] = HyperVOperationalStatus.PredictiveFailure;
                        newMachine.OperationalStatus[1] = HyperVOperationalStatus.PredictiveFailure;
                    }

                    if (operatingSystem == HyperVHostOS.HyperV2012 || operatingSystem == HyperVHostOS.WindowsServer2012 || operatingSystem == HyperVHostOS.Windows8)
                        newMachine.Generation = 1;

                    virtualMachines.Add(newMachine);
                }
                catch (Exception ea)
                {
                    Logger.Log(LogEntryType.Warning, "Cannot get virtual machine list via WMI.", ea, _LoggerContext); //ID: 41004
                }
            }
            #endregion

            #region Msvm_VirtualSystemSettingData
            //Get Generation for HyperV2012R2 and WindowsServer2012R2 and Windows 8.1
            if (operatingSystem != HyperVHostOS.HyperV2012 && operatingSystem != HyperVHostOS.WindowsServer2012 && operatingSystem != HyperVHostOS.Windows8)
            {
                query = new SelectQuery("SELECT VirtualSystemSubType, ConfigurationID FROM Msvm_VirtualSystemSettingData");
                searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    try
                    {
                        string vsst = queryObj["VirtualSystemSubType"].ToString();
                        string currentGuid = queryObj["ConfigurationID"].ToString();

                        foreach (HyperVMachine hvMachine in virtualMachines)
                        {
                            if (hvMachine.GUID == currentGuid)
                            {
                                if (vsst == "Microsoft:Hyper-V:SubType:1")
                                    hvMachine.Generation = 1;
                                else if (vsst == "Microsoft:Hyper-V:SubType:2")
                                    hvMachine.Generation = 2;
                            }
                        }
                    }
                    catch (Exception ea)
                    {
                        Logger.Log(LogEntryType.Warning, "Cannot get virtual machine subtype via WMI.", ea, _LoggerContext); //ID: 41005
                    }
                }
            }
            #endregion


            return (virtualMachines);
        }
    }
}
