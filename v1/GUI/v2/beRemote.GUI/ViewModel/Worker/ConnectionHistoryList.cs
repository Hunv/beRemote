using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using beRemote.Core;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.ProtocolSystem.ProtocolBase.Declaration;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.ViewModel.Worker
{
    public static class ConnectionHistoryList
    {
        public static DataTable GetHistoryList(int entryCount)
        {
            var dtHistory = new DataTable();
            dtHistory.Columns.Add(new DataColumn("Image", typeof(ImageSource)));
            dtHistory.Columns.Add(new DataColumn("Host"));
            dtHistory.Columns.Add(new DataColumn("conId", typeof(long)));

            try
            {
                var history = StorageCore.Core.GetUserHistory(StorageCore.Core.GetUserId(), entryCount, 0);
                foreach (var uhe in history)
                {
                    var dR = dtHistory.NewRow();

                    if (!Kernel.GetAvailableProtocols().ContainsKey(uhe.Protocol))
                        continue;

                    dR["Image"] = Kernel.GetAvailableProtocols()[uhe.Protocol].GetProtocolIcon(IconType.SMALL); //Get Icon
                    dR["Host"] = uhe.Name; //Get the Displayname
                    dR["conId"] = uhe.ConnectionId; //Get the ID of the connection

                    dtHistory.Rows.Add(dR);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogEntryType.Warning, "Exception while loading history.", ex);
            }

            return(dtHistory);
        }
    }
}
