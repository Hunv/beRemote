using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.VendorProtocols.HyperVManager.HyperV
{
    public enum HyperVNetworkPortMirroring
    {
        None = 0,
        Target = 1,
        Source = 2
    }

    public enum HyperVStorageType
    { 
        Unknown = 0,
        HardDisk = 1,
        Floppy = 2,
        DVD = 3,
        Image = 4
    }


    public enum HyperVStartaction
    { 
        None,
        Automatic,
        Always
    }

    public enum HyperVStopaction
    { 
        None,
        Save,
        TurnOff,
        Shutdown
    }

    public enum HyperVRecoveraction
    { 
        None
    }

    public enum HyperVHostOS
    { 
        Unknown,
        WindowsServer2008,
        WindowsServer2008R2,
        WindowsServer2012,
        WindowsServer2012R2,
        HyperV2008,
        HyperV2008R2,
        HyperV2012,
        HyperV2012R2,
        Windows8,
        Windows81
    }

    //***************************************************************//
    //***** BELOW THIS LINE, IT IS TAKEN FROM THE DOCUMENTATION *****//
    //***************************************************************//

    #region Msvm_ComputerSystem
    public enum HyperVAvailableRequestedStates //only v2
    {                   //  v2
        Enabled,        //  2
        Disabled,       //  3
        ShutDown,       //  4
        Offline,        //  6
        Test,           //  7
        Defer,          //  8
        Quiesce,        //  9
        Reboot,         //  10
        Reset,          //  11
        DMFTReserved    //  everything else
    }

    public enum HyperVCommunicationStatus //only v2
    { //Don't has IDs
        OK,         
        Error,
        Degraded,
        Unknown,
        PredFail,
        Starting,
        Stopping,
        Service,
        Stressed,
        NonRecover,
        NoContact,
        LostComm
    }

    public enum HyperVEnabledState
    {                       //  v1      v2
        Unknown,            //  0       0
        Other,              //          1
        Enabled,            //  2       2
        Disabled,           //  3       3
        ShuttingDown,       //          4
        NotApplicable,      //          5
        EnabledButOffline,  //          6
        InTest,             //          7
        Deferred,           //          8
        Quiesce,            //          9
        Paused,             //  32768
        Suspended,          //  32769
        Starting,           //          10
        Snapshotting,       //  32771
        Saving,             //  32773
        Stopping,           //  32774
        Pausing,            //  32776
        Resuming            //  32777
    }

    public enum HyperVEnhancedSessionModeState //only v2
    {                           //  v2
        AllowedAndAvailable,    //  2
        NotAllowed,             //  3
        AllowedButNotAvailable  //  6
    }

    public enum HyperVFailedOverReplicationType //only v2
    {                           //  v2
        None,                   //  0
        Regular,                //  1
        ApplicationConsistent,  //  2
        Planned                 //  3
    }

    public enum HyperVHealthState
    {                           //  v1  v2
        Unknown,                //  
        OK,                     //  5   5
        MajorFailure,           //  20  20
        CriticalFailure         //  25  25
    }

    public enum HyperVLastReplicationType //only v2
    {                           //  v2
        None,                   //  0
        Regular,                //  1
        ApplicationConsistent,  //  2
        Planned                 //  3
    }

    public enum HyperVOperationalStatus
    {                           //  v1      v2
        OK,                     //  2       2
        Degraded,               //  3       3
        PredictiveFailure,      //  5       5
        Stopped,                //  10      10
        InService,              //  11      11
        Dormant,                //  15      15
        CreatingSnapshot,       //  32768   32768
        ApplyingSnapshot,       //  32769   32769
        DeletingSnapshot,       //  32770   32770
        WaitingToStart,         //  32771   32771
        MergingDisks,           //  32772   32772
        ExportingVirtualMachine,//  32773   32773
        MigratingVirtualMachine //  32774   32774
    }

    public enum HyperVReplicationHealth //only v2
    {                   //  v2
        NotApplicable,  //  0
        OK,             //  1
        Warning,        //  2
        Critical        //  3
    }

    public enum HyperVReplicationMode //only v2
    {                   //  v2
        None,           //  0
        Primary,        //  1
        Recovery,       //  2
        Replica,        //  3
        ExtendedReplica //  4
    }

    public enum HyperVReplicationState //only v2
    {                                           //  v2
        Disabled,                               //  0
        ReadyForReplication,                    //  1
        WaitingToCompleteInitialReplication,    //  2
        Replicating,                            //  3
        SyncedReplicationComplete,              //  4
        Recovered,                              //  5
        Commited,                               //  6
        Suspended,                              //  7
        Critical,                               //  8
        WaitingToStartResynchronization,        //  9
        Resynchronizing,                        //  10
        ResynchronizationSuspended,             //  11
        FailoverInProgress,                     //  12
        FailbackInProgress,                     //  13
        FailbackComplete                        //  14
    }
    #endregion
}
