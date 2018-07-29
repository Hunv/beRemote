using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace beRemote.VendorProtocols.HyperVManager.HyperV
{
    public class HyperVMachine
    {        
        private string _MachineName; //"Element Name" Displayname of the machine in the HyperV Manager
        private string _GUID; //"Name" The internal name of the machine
        private string _Description; //"description" - usally "Microsoft Virtual Machine" or "Microsoft Hosting Computer System"
        private string _Notes;
        private byte _Generation; //Generation 1 or Generation 2 (available until 2012/8 and higher)
        private string _PathForCheckpointFiles = "";
        private string _PathForSmartPaging = "";
        private UInt16 _NumberOfNUMANodes = 0;

        //Start/Stop-Actions
        private HyperVStartaction _StartupAction = HyperVStartaction.None;
        private int _StartupDelay = 0; //Delay of the Startup-Action if startupAction is not "none"
        private HyperVStopaction _StopAction = HyperVStopaction.Save;
        private HyperVRecoveraction _RecoverAction = HyperVRecoveraction.None;

        //Machine-Status
        private HyperVEnabledState _EnabledState = HyperVEnabledState.Unknown;
        private HyperVHealthState _HealthState = HyperVHealthState.Unknown;
        private string _Status;
        private string[] _StatusDescriptions;
        private HyperVOperationalStatus[] _OperationalStatus = new HyperVOperationalStatus[2]{HyperVOperationalStatus.OK, HyperVOperationalStatus.OK};
        private UInt64 _OnTimeInMilliseconds = 0;        

        //Replication and Failover Thinks
        private HyperVReplicationHealth _ReplicationHealth = HyperVReplicationHealth.OK;
        private HyperVReplicationMode _ReplicationMode = HyperVReplicationMode.None;
        private HyperVReplicationState _ReplicationState = HyperVReplicationState.Disabled;
        private HyperVFailedOverReplicationType _FailOverReplicationType = HyperVFailedOverReplicationType.None;        
        private HyperVLastReplicationType _LastReplicationType = 0;

        //Timestamps
        private DateTime _InstallDate;
        private DateTime _LastApplicationConsistentReplicationTime;
        private DateTime _LastReplicationTime;
        private DateTime _TimeOfLastConfigurationChange;
        private DateTime _TimeOfLastStateChange;
        
        //Hardware components
        private HyperVMemory _Memory = new HyperVMemory();
        private List<HyperVProcessor> _Processor = new List<HyperVProcessor>();
        private HyperVIntegration _Integration = new HyperVIntegration();
        private List<HyperVNetworkAdapter> _Network = new List<HyperVNetworkAdapter>();
        private List<HyperVIdeController> _IDEController = new List<HyperVIdeController>();
        private List<HyperVScsiController> _SCSIController = new List<HyperVScsiController>();
        private List<HyperVStorageDevice> _StorageDevice = new List<HyperVStorageDevice>();
        private List<HyperVSerialController> _SerialController = new List<HyperVSerialController>();
        private HyperVBios _BIOS = new HyperVBios();


        public string Machinename { get { return _MachineName; } set { _MachineName = value; } }
        public string GUID { get { return _GUID; } set { _GUID = value; } }
        public string Notes { get { return _Notes; } set { _Notes = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }
        public byte Generation { get { return _Generation; } set { _Generation = value; } }
        public string PathForCheckpointFiles { get { return _PathForCheckpointFiles; } set { _PathForCheckpointFiles = value; } }
        public string PathForSmartPaging { get { return _PathForSmartPaging; } set { _PathForSmartPaging = value; } }
        public UInt16 NumberOfNUMANodes { get { return _NumberOfNUMANodes; } set { _NumberOfNUMANodes = value; } }

        public HyperVStartaction StartupAction { get { return _StartupAction; } set { _StartupAction = value; } }
        public int StartupDelay { get { return _StartupDelay; } set { _StartupDelay = value; } }
        public HyperVStopaction StopAction { get { return _StopAction; } set { _StopAction = value; } }
        public HyperVRecoveraction RecoverAction { get { return _RecoverAction; } set { _RecoverAction = value; } }

        public HyperVEnabledState EnabledState { get { return _EnabledState; } set { _EnabledState = value; } }
        public HyperVHealthState HealthState { get { return _HealthState; } set { _HealthState = value; } }
        public string Status { get { return _Status; } set { _Status = value; } }
        public string[] StatusDescriptions { get { return _StatusDescriptions; } set { _StatusDescriptions = value; } }
        public HyperVOperationalStatus[] OperationalStatus { get { return _OperationalStatus; } set { _OperationalStatus = value; } }
        public UInt64 OnTimeInMilliseconds { get { return _OnTimeInMilliseconds; } set { _OnTimeInMilliseconds = value; } }
        
        public HyperVReplicationHealth ReplicationHealth { get { return _ReplicationHealth; } set { _ReplicationHealth = value; } }
        public HyperVReplicationMode ReplicationMode { get { return _ReplicationMode; } set { _ReplicationMode = value; } }
        public HyperVReplicationState ReplicationState { get { return _ReplicationState; } set { _ReplicationState = value; } }
        public HyperVFailedOverReplicationType FailOverReplicationType { get { return _FailOverReplicationType; } set { _FailOverReplicationType = value; } }
        public HyperVLastReplicationType LastReplicationType { get { return _LastReplicationType; } set { _LastReplicationType = value; } }

        public DateTime TimeOfLastConfigurationChange { get { return _TimeOfLastConfigurationChange; } set { _TimeOfLastConfigurationChange = value; } }
        public DateTime TimeOfLastStateChange { get { return _TimeOfLastStateChange; } set { _TimeOfLastStateChange = value; } }
        public DateTime InstallDate { get { return _InstallDate; } set { _InstallDate = value; } }
        public DateTime LastApplicationConsistentReplicationTime { get { return _LastApplicationConsistentReplicationTime; } set { _LastApplicationConsistentReplicationTime = value; } }
        public DateTime LastReplicationTime { get { return _LastReplicationTime; } set { _LastReplicationTime = value; } }

        public HyperVMemory Memory { get { return _Memory; } set { _Memory = value; } }
        public List<HyperVProcessor> Processor { get { return _Processor; } set { _Processor = value; } }
        public HyperVIntegration Integration { get { return _Integration; } set { _Integration = value; } }
        public List<HyperVNetworkAdapter> Network { get { return _Network; } set { _Network = value; } }
        public List<HyperVIdeController> IDEController { get { return _IDEController; } set { _IDEController = value; } }
        public List<HyperVScsiController> SCSIController { get { return _SCSIController; } set { _SCSIController = value; } }
        public List<HyperVStorageDevice> StorageDevice { get { return _StorageDevice; } set { _StorageDevice = value; } }
        public List<HyperVSerialController> SerialController { get { return _SerialController; } set { _SerialController = value; } }
        public HyperVBios BIOS { get { return _BIOS; } set { _BIOS = value; } }

        #region Property and Methods, based on existing Variables
        public string OnTimeInDays 
        {
            get 
            {
                UInt64 onTimeValue = _OnTimeInMilliseconds;
                UInt64 days = onTimeValue / 86400000;
                onTimeValue = onTimeValue - 86400000 * days;
                UInt64 hours = onTimeValue / 3600000;
                onTimeValue = onTimeValue - 3600000 * hours;
                UInt64 minutes = onTimeValue / 60000;
                onTimeValue = onTimeValue - 60000 * minutes;
                UInt64 seconds = onTimeValue / 1000;

                return (days + "." + hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2"));
            }             
        }
        #endregion
    }

    public class HyperVMemory
    {
        //General
        long size = 1024; //"Memory on Startup"
        bool dynamicMemory = false; //"activate dynamic Memory"
        int dynamicMemoryMin = 512; //"Minimum Memory" - only used, if dynamicMemory is true
        int dynamicMemoryMax = 1024; //"Maximum Memory" - only used, if dynamicMemory is true
        byte dynamicMemoryBuffer = 20; //"Memorybuffer" - only used, if dynamicMemory is true
        byte memoryPriority = 4; //Priority of this memory compared to other virtual machines. Valid values: 0-8;

        //Statistics
        long used;
    }

    public class HyperVProcessor
    {
        //General
        byte resourceControl_Reserve = 0; //"Reserve for virtual machine (in %)"
        byte resourceControl_Limit = 100; //"Limit for virtual machine (in %)"
        byte resourceControl_RelativeWeight = 100; //"Relative Weigth"

        //Compability
        bool migrateToOtherCPU = false; //"Migrate to another physical system with another processorversion"

        //NUMA
        byte numa_MaxProcessors = 1; //"Maximum Processors"
        int muma_MaxMemory = 1024; //"Maximum Memory (in MB)"
        byte numa_MaxNodesInSocket = 1; //"Maximum NUMA-Nodes in one Socket"

        //Statistics
        List<byte> load;
        List<List<byte>> loadHistory;
    }

    public class HyperVIntegration
    {
        bool shutdown = false;
        bool timesync = false;
        bool heartbeat = false;
        bool vssprovider = false;
        bool kvpexchange = false;
        bool guestServices = false;
    }

    public class HyperVNetworkAdapter
    {
        //General settings
        string virtualSwitch = ""; //The virtual switch this NetworkAdapter is connected to
        byte type; //Legacy or "new"?
        int vlan = 1; //1 = no VLAN
        bool bandwidthcontrol = false; //Bandwidthcontrol enabled?
        int bandwidthMin = 0;
        int bandwdithMax = 0;

        //Section "Hardwareacceleratement"
        bool queueForVirtualMachine = true; //Queue for virtual Computer
        bool ipsecTaskOffload = true; //IPsec Taskoffload
        int ipsecTaskOffloadMax = 512; //IPsec Taskoffload maximum number
        bool eaVirtualisationWithSingleRoot = false; //"SR-IOV"

        //Section "Advanced Features"
        bool macAddressDynamic = true; //Dynamic MAC-Address
        byte[] macAddressStatic = new byte[6] { 0, 0, 0, 0, 0, 0 }; //The static MAC-Adress if macAddressDynamic is set to false
        bool macAddressSpoofing = false; //Allow spoofing of mac Addresses
        bool dhcpGuard = false; //DHCP-Guard enabled
        bool routerGuard = false; //Routeranouncementguard enabled
        bool guardedNetwork = true; //Guarded Network enabled
        HyperVNetworkPortMirroring portMirroring = HyperVNetworkPortMirroring.None; //Portmirroring-settings
        bool nicTeamingInVirtualMachine = false; //Allow NIC-Teaming in virtual machine
    }

    public class HyperVIdeController
    {
        string guid;
    }

    public class HyperVScsiController
    {
        string guid;
    }

    public class HyperVStorageDevice
    {
        HyperVStorageType type = HyperVStorageType.Unknown; //The type of a storage device
        string controller = ""; //The IDE or SCSI-Controller this device is connected to
        int connector = 0; //The port, the Device is connected to (valid values: IDE: 0-1, SCSI: 0-63)        
        bool? isVirtual = true; //Is the physical Storagemedium a virtual device (i.e. VHD(X)-Files) - for DVD- and Floppy-Drives: null = not inserted
        string virtualPath = ""; //The path to the storagefile (i.e. VHD(X)-Files)
        string physicalPath = ""; //the path toe the phyical Device, if the Source is not a virtual file.

        //For SCSI-HDDs Only
        bool scsiServiceQualityManagement = false; //"activate Management of the Servicequality"
        int scsiServiceQualityManagementMin = 0; //"Minumum IOPS" - only applied, if scsiServiceQualityManagement is set to true; 0 is only valid, if max is not 0
        int scsiServiceQualityManagementMax = 0; //"Maximum IOPS" - only applied, if scsiServiceQualityManagement is set to true; 0 is only vlaid, if min is not 0
        bool scsiVirtualDiskShare = false; //"activate the sharing of virtual disks"
    }

    public class HyperVSerialController
    {
        string guid;
        bool isNamedPipe = false; //is NamedPipe activated?
        string pipename = ""; //Name of the pipe - only applied, if isNamedPipe = true
        bool isRemoteSystem = false; //Is Remotesystem activated? - only applied, if isNamedPipe = true
        string remoteSystem = ""; //The path to the remotesystem - only applied, if isNamedPipe = true
    }

    public class HyperVBios
    {
        List<string> bootOrder = new List<string>(4) { "CD", "IDE", "Legacy Networkadapter", "Diskette" };
    }
}