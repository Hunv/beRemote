using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.VendorProtocols.HyperVManager.HyperV
{
    public static class HyperVConverter
    {
        public static HyperVEnabledState ConvertToEnabledState(UInt16 value)
        {
            switch (value)
            { 
                default:
                case 0:
                    return (HyperVEnabledState.Unknown);
                case 1:
                    return (HyperVEnabledState.Other);
                case 2:
                    return (HyperVEnabledState.Enabled);
                case 3:
                    return (HyperVEnabledState.Disabled);
                case 4:
                    return (HyperVEnabledState.ShuttingDown);
                case 5:
                    return (HyperVEnabledState.NotApplicable);
                case 6:
                    return (HyperVEnabledState.EnabledButOffline);
                case 7:
                    return (HyperVEnabledState.InTest);
                case 8:
                    return (HyperVEnabledState.Deferred);
                case 9:
                    return (HyperVEnabledState.Quiesce);
                case 10:
                    return (HyperVEnabledState.Starting);
                case 32768:
                    return (HyperVEnabledState.Paused);
                case 32769:
                    return (HyperVEnabledState.Suspended);
                case 32771:
                    return (HyperVEnabledState.Snapshotting);
                case 32773:
                    return (HyperVEnabledState.Saving);
                case 32774:
                    return(HyperVEnabledState.Stopping);
                case 32776:
                    return (HyperVEnabledState.Paused);
                case 32777:
                    return(HyperVEnabledState.Resuming);
            }
        }
        public static HyperVFailedOverReplicationType ConvertToFailedOverReplicationType(UInt16 value)
        {
            switch (value)
            { 
                default:
                case 0:
                    return (HyperVFailedOverReplicationType.None);
                case 1:
                    return (HyperVFailedOverReplicationType.Regular);
                case 2:
                    return (HyperVFailedOverReplicationType.ApplicationConsistent);
                case 3:
                    return (HyperVFailedOverReplicationType.Planned);
            }
        }
        public static HyperVHealthState ConvertToHealthState(UInt16 value)
        {
            switch (value)
            { 
                default:
                    return (HyperVHealthState.Unknown);
                case 5:
                    return (HyperVHealthState.OK);
                case 20:
                    return (HyperVHealthState.MajorFailure);
                case 25:
                    return (HyperVHealthState.CriticalFailure);
            }
        }
        public static HyperVLastReplicationType ConvertToLastReplicationType(UInt16 value)
        {
            switch (value)
            {
                default:
                case 0:
                    return (HyperVLastReplicationType.None);
                case 1:
                    return (HyperVLastReplicationType.Regular);
                case 2:
                    return (HyperVLastReplicationType.ApplicationConsistent);
                case 3:
                    return (HyperVLastReplicationType.Planned);
            }
        }
        public static HyperVOperationalStatus ConvertToOperationalStatus(UInt16 value)
        {
            switch (value)
            { 
                default:
                case 2:
                    return (HyperVOperationalStatus.OK);
                case 3:
                    return (HyperVOperationalStatus.Degraded);
                case 5:
                    return (HyperVOperationalStatus.PredictiveFailure);
                case 10:
                    return (HyperVOperationalStatus.Stopped);
                case 11:
                    return (HyperVOperationalStatus.InService);
                case 15:
                    return (HyperVOperationalStatus.Dormant);
                case 32768:
                    return (HyperVOperationalStatus.CreatingSnapshot);
                case 32769:
                    return (HyperVOperationalStatus.ApplyingSnapshot);
                case 32770:
                    return (HyperVOperationalStatus.DeletingSnapshot);
                case 32771:
                    return (HyperVOperationalStatus.WaitingToStart);
                case 32772:
                    return (HyperVOperationalStatus.MergingDisks);
                case 32773:
                    return (HyperVOperationalStatus.ExportingVirtualMachine);
                case 32774:
                    return (HyperVOperationalStatus.MigratingVirtualMachine);
            }
        }
        public static HyperVReplicationHealth ConvertToReplicationHealth(UInt16 value)
        {
            switch (value)
            { 
                default:
                case 0:
                    return (HyperVReplicationHealth.NotApplicable);
                case 1:
                    return (HyperVReplicationHealth.OK);
                case 2:
                    return (HyperVReplicationHealth.Warning);
                case 3:
                    return (HyperVReplicationHealth.Critical);
            }
        }
        public static HyperVReplicationMode ConvertToReplicationMode(UInt16 value)
        {
            switch (value)
            { 
                default:
                case 0:
                    return (HyperVReplicationMode.None);
                case 1:
                    return (HyperVReplicationMode.Primary);
                case 2:
                    return (HyperVReplicationMode.Recovery);
                case 3:
                    return (HyperVReplicationMode.Replica);
                case 4:
                    return (HyperVReplicationMode.ExtendedReplica);
            }
        }
        public static HyperVReplicationState ConvertToReplicationState(UInt16 value)
        {
            switch (value)
            { 
                default:
                case 0:
                    return (HyperVReplicationState.Disabled);
                case 1:
                    return (HyperVReplicationState.ReadyForReplication);
                case 2:
                    return (HyperVReplicationState.WaitingToCompleteInitialReplication);
                case 3:
                    return (HyperVReplicationState.Replicating);
                case 4:
                    return (HyperVReplicationState.SyncedReplicationComplete);
                case 5:
                    return (HyperVReplicationState.Recovered);
                case 6:
                    return (HyperVReplicationState.Commited);
                case 7:
                    return (HyperVReplicationState.Suspended);
                case 8:
                    return (HyperVReplicationState.Critical);
                case 9:
                    return (HyperVReplicationState.WaitingToStartResynchronization);
                case 10:
                    return (HyperVReplicationState.Resynchronizing);
                case 11:
                    return (HyperVReplicationState.ResynchronizationSuspended);
                case 12:
                    return (HyperVReplicationState.FailoverInProgress);
                case 13:
                    return (HyperVReplicationState.FailbackInProgress);
                case 14:
                    return (HyperVReplicationState.FailbackComplete);
            }
        }
        
        public static DateTime ConvertToDateTime(string wmiDateTime)
        {
            //Example: 20140128165650.256224-000
            //Year     2014                         0,4
            //Month        01                       4,2
            //Day            28                     6,2
            //Hour             16                   8,2
            //Minute             56                 10,2
            //Second               50               12,2

            DateTime ret = new DateTime(
                Convert.ToInt32(wmiDateTime.Substring(0, 4)),
                Convert.ToInt32(wmiDateTime.Substring(4, 2)),
                Convert.ToInt32(wmiDateTime.Substring(6, 2)),
                Convert.ToInt32(wmiDateTime.Substring(8, 2)),
                Convert.ToInt32(wmiDateTime.Substring(10, 2)),
                Convert.ToInt32(wmiDateTime.Substring(12, 2)));

            return (ret);
        }
    }
}
