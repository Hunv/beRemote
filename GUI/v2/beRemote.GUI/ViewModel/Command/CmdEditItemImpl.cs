using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdEditItemImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            var evArgs = new AddTabEventArgs();
            var parameter = new object[0];
            var conItem = (ConnectionItem)sender;

            switch (conItem.ConnectionType)
            {
                default:
                    return;
                case ConnectionTypeItems.connection:
                    //Whats happening: Figure out the Protocol, that will be called by default and preselect it in the Edit-Dialog

                    var defaultProtocolId = (long)0;
                    var conProts = StorageCore.Core.GetConnectionSettings(conItem.ConnectionID);
                    var userSettings = StorageCore.Core.GetUserSettings();

                    //Check each applied protocol
                    foreach (var prot in conProts)
                    {
                        //Check if it is default protocol. If it is, set Id
                        if (prot.getProtocol() == userSettings.getDefaultProtocol())
                        {
                            defaultProtocolId = prot.getId();
                            break;
                        }
                    }

                    //Checks if the default Protocol was configured. If not: Take first entry. If no entrys available: cancel
                    if (defaultProtocolId == 0 && conProts.Count > 0)
                        defaultProtocolId = conProts[0].getId();
                    //Cancel here if the Connection has no valid protocol
                    else if (conProts.Count == 0)
                        return;
                    
                    var connSet = StorageCore.Core.GetConnectionSetting(defaultProtocolId);

                    parameter = new object[2];
                    parameter[0] = connSet.getConnectionId(); //The ID of the protocol
                    parameter[1] = connSet.getId(); //The ID of the connection for the protocol
                    break;
                case ConnectionTypeItems.protocol:
                    //Whats happening: Preselect the selected Protocol
                    parameter = new object[2];
                    parameter[0] = conItem.ConnectionParent.ConnectionID; //The ID of the protocol
                    parameter[1] = conItem.ConnectionID; //The ID of the connection for the protocol
                    break;
            }

            evArgs.Tab = new Tabs.ManageConnection.TabManageConnection(parameter);

            OnAddTab(evArgs);
        }
    }
}
