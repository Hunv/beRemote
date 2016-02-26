using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using beRemote.Core;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.ViewModel.Worker
{
    public class Heartbeat
    {
        private DispatcherTimer _TmrHeartbeat;

        /// <summary>
        /// Starts the Heartbeat
        /// </summary>
        public void StartHeartbeat()
        {
            while (Kernel.IsKernelReady() == false)
                System.Threading.Thread.Sleep(100);

            var heartbeatInterval = StorageCore.Core.GetSetting("heartbeat");
            
            //Is sometimes emtpy
            while (heartbeatInterval == "")
                heartbeatInterval = StorageCore.Core.GetSetting("heartbeat");

            _TmrHeartbeat = new DispatcherTimer();
            _TmrHeartbeat.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(heartbeatInterval));
            _TmrHeartbeat.Tick += tmrHeartbeat_Elapsed;
            tmrHeartbeat_Elapsed(null, null);
            _TmrHeartbeat.Start();
        }

        /// <summary>
        /// Stops the Heartbeat
        /// </summary>
        public void StopHeartbeat()
        {
            _TmrHeartbeat.Stop();
        }

        /// <summary>
        /// Performs a Heartbeat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void tmrHeartbeat_Elapsed(object sender, EventArgs args)
        {
            //Update Heartbeat
            StorageCore.Core.UpdateUserHeartbeat();

            //todo: redesign, so there has to be only one sql-query for performing heartbeat and get a flag, if there are things to do (like maintmode)
            //Check for Maintananace mode
            //switch (StorageCore.Core.GetSetting("maintmode"))
            //{
            //    default:
            //    case "0":
            //        break;
            //    case "1":
            //        MessageBox.Show("The beRemote Database was set to Maintanance-Mode by your Administrator." + Environment.NewLine +
            //                "beRemote now has to close. At the next start, you possibly have to update your beRemote-Client.", "Mainanance Mode activated", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        break;
            //}
        }
    }
}
