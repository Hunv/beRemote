using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using beRemote.Core;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.ProtocolSystem.ProtocolBase.Declaration;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.Controls.TreeView;

namespace beRemote.GUI.ViewModel.Worker
{
    public class ConnectionList
    {
        /// <summary>
        /// Loads the ConnectionList from the Database
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ConnectionItem> GetConnectionList()
        {
            return (GetConnectionList(0));
        }

        /// <summary>
        /// Loads the ConnectionList from the Database
        /// </summary>
        /// <param name="filterSetId"></param>
        /// <returns></returns>
        public ObservableCollection<ConnectionItem> GetConnectionList(long filterSetId)
        {
            var connectionFolderList = StorageCore.Core.GetFolders();
            var connectionFolderDic = new Dictionary<long, beTreeViewOrphanItem>();
            var rootIds = new List<long>(); //Contains the Root-Items

            //First Step: Create a Dictionary for all Folders and create ConnectionItems
            foreach (var aFolder in connectionFolderList)
            {
                var orphanItem = new beTreeViewOrphanItem();
                orphanItem.ParentId = aFolder.ParentId;
                orphanItem.OrphanItem = new ConnectionItem(aFolder.Name, ConnectionTypeItems.folder, null, null, null, aFolder.Id, "", "", "");

                //Add public overlay, if Item is public
                if (aFolder.getIsPublic())
                {
                    var overlayList = orphanItem.OrphanItem.ConnectionIconOverlays;
                    overlayList.Add(OverlayIconPublic);
                    orphanItem.OrphanItem.ConnectionIconOverlays = overlayList;
                }            

                connectionFolderDic.Add(aFolder.Id, orphanItem);

                if (aFolder.ParentId == 0)
                    rootIds.Add(aFolder.Id);
            }

            //Second Step: Set the Parent-Property of the Orphan Items
            foreach (var orphanItem in connectionFolderDic.Values)
            {
                //If it is root, continue
                if (orphanItem.ParentId == 0) continue;

                connectionFolderDic[orphanItem.ParentId].OrphanItem.SubConnections.Add(orphanItem.OrphanItem);
                orphanItem.OrphanItem.ConnectionParent = connectionFolderDic[orphanItem.ParentId].OrphanItem;
            }


            #region Load Protocols and Cache them
            var connectionProtocol = StorageCore.Core.GetConnectionSettings(); //Get all ConnectionSettings, that belong to one of the user-connections
            var connectionProtocolDic = new Dictionary<long, List<ConnectionProtocol>>(); //Stores the Connectionsettings. Key = ID

            foreach (var conProt in connectionProtocol)
            {
                if (!connectionProtocolDic.ContainsKey(conProt.ConnectionId)) //New Entry
                    connectionProtocolDic.Add(conProt.ConnectionId, new List<ConnectionProtocol>(1) { conProt });
                else //Existing Entry
                    connectionProtocolDic[conProt.ConnectionId].Add(conProt);
            }

            #endregion

            #region Load CredentialSetNames and Cache them
            var credentialSetNames = new Dictionary<long, string>(); //Stores all CredentialSets to accelerate the CredentialSetName-Queries

            foreach (var uC in StorageCore.Core.GetUserCredentialsAll())
            {
                credentialSetNames.Add(uC.Id, uC.Description);
            }
            #endregion

            #region LoadConnections
            var connectionList = new List<ConnectionHost>();

            if (filterSetId != 0) //If there is a filterID, the Connectionlist should be filtered
            {
                connectionList = StorageCore.Core.GetFilterResult(StorageCore.Core.GetFilterSets(filterSetId)[0], Kernel.GetAvailableProtocols().Values.ToList().Select(p => p.GetProtocolIdentifer()).ToList());
            }
            else //Show unfiltered connection
            {
                connectionList = StorageCore.Core.GetConnections();
            }

            var defaultProtocol = StorageCore.Core.GetUserSettings().getDefaultProtocol();
            ImageSource defaultIcon = null; //Preload it to avoid loading every time it is used

            if (Kernel.GetAvailableProtocols().ContainsKey(defaultProtocol)) //Only preload if defaultProtocol is available locally
            {
                defaultIcon = Kernel.GetAvailableProtocols()[defaultProtocol].ProtocolIconSmall;

                defaultIcon.Freeze();
            }

            //Create all Host-Items including Protocols and the Icons
            foreach (var cHost in connectionList)
            {
                //If a Connection is in a Node the current user has no access to
                if (!connectionFolderDic.ContainsKey(cHost.Folder))
                    continue;
                
                var conItem = new ConnectionItem(cHost.Name, ConnectionTypeItems.connection, connectionFolderDic[cHost.Folder].OrphanItem, null, null, cHost.ID, cHost.Host, cHost.Description, "");

                //Add public overlay, if Item is public
                if (cHost.IsPublic)
                {
                    var overlayList = conItem.ConnectionIconOverlays;
                    overlayList.Add(OverlayIconPublic);
                    conItem.ConnectionIconOverlays = overlayList;
                }

                //If the current connection is already handled, continue
                if (!connectionProtocolDic.ContainsKey(cHost.ID))                
                    continue;
                
                var iconSet = false;

                //Add Protocols to Connection
                foreach (var conProt in connectionProtocolDic[cHost.ID])
                {
                    //If the Connection already contains this Protocol , continue
                    if (!Kernel.GetAvailableProtocols().ContainsKey(conProt.Protocol)) continue;


                    //Check if it is the default Protocol and the default Protocol is available
                    if (conProt.Protocol == defaultProtocol && defaultIcon != null)
                    {
                        //Set the Icon of the Connection with the default protocol
                        conItem.ConnectionIcon = defaultIcon;
                        iconSet = true;
                    }

                    var credName = ""; //The Name of the CredentialSet, that is assigned to this Protocol
                    if (conProt.UserCredentialId != 0) //If the Connection has a CredentialSet-Name: Get it
                    {
                        if (credentialSetNames.ContainsKey(conProt.UserCredentialId))
                            credName = credentialSetNames[conProt.UserCredentialId];
                    }

                    //Create a new Protocol-Item
                    var protItem = new ConnectionItem(Kernel.GetAvailableProtocols()[conProt.Protocol].GetProtocolName(),
                        ConnectionTypeItems.protocol,
                        conItem,
                        Kernel.GetAvailableProtocols()[conProt.Protocol].ProtocolIconSmall,
                        null,
                        conProt.Id,
                        cHost.Name,         //The Hostname of the Connection of this Protocol
                        cHost.Description,  //The Description of the Connection of this Protocol
                        credName            //The Name of the CredentialSet of this Protocol
                        );
                    
                    //Add Protocol to connection
                    conItem.SubConnections.Add(protItem);
                }

                if (iconSet == false && conItem.SubConnections.Count > 0) //Use the first available Protocol, because no default protocol is available
                {
                    var connectionIcon = conItem.SubConnections[0].ConnectionIcon;
                    if (connectionIcon != null)
                    {
                        connectionIcon.Freeze();

                        conItem.ConnectionIcon = connectionIcon;
                    }
                }

                connectionFolderDic[cHost.Folder].OrphanItem.SubConnections.Add(conItem);
            }

            #endregion

            #region get the Root-Items and add the to one list
            var rootList = new ObservableCollection<ConnectionItem>();
            foreach (var rootId in rootIds)
            {
                rootList.Add(connectionFolderDic[rootId].OrphanItem);
            }

            #region Delete empty directories if filters are active
            if (filterSetId != 0)
            {
                for (var i = 0; i < rootList.Count; i++)
                {
                    rootList[i] = LoadFilteredListHelper(rootList[i]);
                }
            }
            #endregion
            #endregion

            return (rootList);
        }

        /// <summary>
        /// Helps loadConList to cleanup the tree from empty directories
        /// </summary>
        /// <param name="toCleanup">the Item, that Subitems has to be cleaned up</param>
        /// <returns></returns>
        private ConnectionItem LoadFilteredListHelper(ConnectionItem toCleanup)
        {
            for (int i = 0; i < toCleanup.SubConnections.Count; i++)
            {
                //If the subitem is a folder, check it first
                if (toCleanup.SubConnections[i].ConnectionType == ConnectionTypeItems.folder)
                    toCleanup.SubConnections[i] = LoadFilteredListHelper(toCleanup.SubConnections[i]);

                //If the subitem hasn't any subconnections anymore, remove it
                if (toCleanup.SubConnections[i].SubConnections.Count == 0)
                {
                    toCleanup.SubConnections.RemoveAt(i);
                    i--;
                }
            }

            return (toCleanup);
        }

        #region OverlayIcon
        private ImageSource _OverlayIconPublic;

        /// <summary>
        /// Returns the Overlay-Icon
        /// </summary>
        private ImageSource OverlayIconPublic
        {
            get
            {
                if (_OverlayIconPublic != null) 
                    return (_OverlayIconPublic);

                Application.Current.Dispatcher.Invoke(() =>
                                                      {
                                                          //Load the public-overlay-Icon (small guy in the bottom right corner)
                                                          var iconUri = new Uri("pack://application:,,,/Images/public_overlay.png", UriKind.RelativeOrAbsolute);
                                                          var iconBitmap = BitmapFrame.Create(iconUri);
                                                          iconBitmap.Freeze();
                                                          _OverlayIconPublic = iconBitmap;
                                                      });
                return (_OverlayIconPublic);
            }
        }
        #endregion
    }
}
