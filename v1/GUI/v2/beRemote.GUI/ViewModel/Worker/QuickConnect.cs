using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core;
using beRemote.Core.Common.Helper;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Worker
{
    public static class QuickConnect
    {
        public static ConnectionItem DoQuickConnect(string connectionPath)
        {
            return (DoQuickConnect(connectionPath, ""));
        }

        public static ConnectionItem DoQuickConnect(string connectionPath, string protocolIdentifier)
        {
            //the selected/default-Protocol
            var protocolString = "";

            //the selected/default-Port
            var protocolPort = 0;

            //the host to connect to
            var host = "";

            //Check if Parameter is null; if so: get default Protocol
            if (String.IsNullOrEmpty(protocolIdentifier))
            {
                protocolString = StorageCore.Core.GetUserSettings().getDefaultProtocol();

                //if there was no default protocol, use first protocol found
                if (Kernel.GetAvailableProtocols().Count > 0 && String.IsNullOrEmpty(protocolString))
                    protocolString = Kernel.GetAvailableProtocols().Values[0].GetProtocolIdentifer();
            }
            else
            {
                protocolString = protocolIdentifier;
            }

            //if there still is no protocol: cancel
            if (String.IsNullOrEmpty(protocolString))
                return(null);

            //Check for Port-Definitions in the Text
            if (connectionPath.Contains(':'))
            {
                var pathParts = connectionPath.Split(':');

                //Verify that the last part is a Number; if not, the whole string is the Server-Address
                if (Helper.IsInteger(pathParts[pathParts.Length - 1]))
                {
                    //The string except the part after the last : is the hostname
                    for (var i = 0; i < pathParts.Length - 1; i++)
                        host = host + pathParts[i];

                    //The last part is the port
                    protocolPort = Convert.ToInt32(pathParts[pathParts.Length - 1]);
                }
            }

            //Is the host given, if not: take it
            if (string.IsNullOrEmpty(host))
                host = connectionPath;

            //Is there a defined port? if not: get default protocol port
            if (protocolPort == 0)
                protocolPort = Kernel.GetAvailableProtocols()[protocolString].GetDefaultProtocolPort();

            //Get the Quick-Connect-Folder
            var quickConnectFolder = (long)0;
            if (StorageCore.Core.GetFolderExists("Quick Connect", StorageCore.Core.GetUserId(), 0))
                quickConnectFolder = StorageCore.Core.GetFolderId("Quick Connect");
            else
                quickConnectFolder = StorageCore.Core.AddFolder("Quick Connect", 0, false);

            //Create Connection
            var newConnectionId = StorageCore.Core.AddConnection(host, host, "Quick Connect added connection", 0, quickConnectFolder, StorageCore.Core.GetUserId(), false, 0);

            //Create ConnectionSetting (Protocol)
            var newConnectionSettingId = StorageCore.Core.AddConnectionSetting(newConnectionId, protocolString, protocolPort);

            //Connect
            //emulate a ConnectionItem
            var conItem = new ConnectionItem(host);
            conItem.ConnectionType = ConnectionTypeItems.protocol;
            conItem.ConnectionName = host;
            conItem.ConnectionID = newConnectionSettingId;

            //Return the "virtual" connection Item
            return (conItem);
        }
    }
}
