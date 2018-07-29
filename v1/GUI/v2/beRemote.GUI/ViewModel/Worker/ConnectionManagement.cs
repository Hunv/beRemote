using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.ViewModel.Worker
{
    public class ConnectionManagement
    {
        /// <summary>
        /// Removes a Protocol, Connection or Folder
        /// </summary>
        public static bool RemoveItem(ConnectionItem removeItem)
        {
            Logger.Log(LogEntryType.Info, String.Format("Delete ItemID {0} of type {1} by user {2}", removeItem.ConnectionID, removeItem.ConnectionType, StorageCore.Core.GetUserId()));

            try
            {
                switch (removeItem.ConnectionType)
                {
                    case ConnectionTypeItems.protocol:
                        RemoveProtocol(removeItem);
                        break;
                    case ConnectionTypeItems.connection:
                        RemoveConnection(removeItem);
                        break;
                    case ConnectionTypeItems.folder:
                        RemoveFolder(removeItem);
                        break;
                }
            }
            catch (Exception ea)
            {
                Logger.Log(
                    LogEntryType.Exception, 
                    String.Format(
                        "Error on deleting an Item (ItemID {0} of type {1} by user {2})", 
                        removeItem.ConnectionID, 
                        removeItem.ConnectionType, 
                        StorageCore.Core.GetUserId()), 
                    ea);
                return (false);
            }
            return (true);
        }


        /// <summary>
        /// Deletes a Protocol and its options
        /// </summary>
        /// <param name="removeItem"></param>
        private static void RemoveProtocol(ConnectionItem removeItem)
        {
            if (removeItem.ConnectionType != ConnectionTypeItems.protocol)
                return;

            RemoveProtocol(removeItem.ConnectionID);
        }

        /// <summary>
        /// Deletes a Protocol and its options
        /// </summary>
        /// <param name="removeItemId"></param>
        private static void RemoveProtocol(long removeItemId)
        {
            //Delete all options for this protocol
            var optionList = StorageCore.Core.GetConnectionOptions(removeItemId);
            foreach (var cpo in optionList)
            {
                StorageCore.Core.DeleteConnectionOption(cpo.getId());
            }

            //Delete the protocol
            StorageCore.Core.DeleteConnectionSetting(removeItemId);
        }

        /// <summary>
        /// Deletes a Connection, its protocols and their options
        /// </summary>
        /// <param name="removeItem"></param>
        public static void RemoveConnection(ConnectionItem removeItem)
        {
            if (removeItem.ConnectionType != ConnectionTypeItems.connection)
                return;

            RemoveConnection(removeItem.ConnectionID);
        }

        /// <summary>
        /// Deletes a Connection, its protocols and their settings and options
        /// </summary>
        /// <param name="removeItemId"></param>
        public static void RemoveConnection(long removeItemId)
        {
            //Delete all Protocols
            var settingList = StorageCore.Core.GetConnectionSettings(removeItemId);
            foreach (var setting in settingList)
            {
                RemoveProtocol(setting.getId());
            }

            //Delete Connection
            StorageCore.Core.DeleteConnection(removeItemId);
        }

        /// <summary>
        /// Deletes a folder, its subfolders and their connections, settings and options
        /// </summary>
        /// <param name="removeItem"></param>
        private static void RemoveFolder(ConnectionItem removeItem)
        {
            if (removeItem.ConnectionType != ConnectionTypeItems.folder)
                return;

            RemoveFolder(removeItem.ConnectionID);
        }

        /// <summary>
        /// Deletes a folder, its subfolders and their connections, settings and options
        /// </summary>
        /// <param name="removeItemId"></param>
        private static void RemoveFolder(long removeItemId)
        {
            //Delete Connections
            var connectionList = StorageCore.Core.GetConnectionsInFolder(removeItemId);
            foreach (var connection in connectionList)
            {
                RemoveConnection(connection.ID);
            }

            //Delete Folders
            var folderList = StorageCore.Core.GetSubfolders(removeItemId);
            foreach (var folder in folderList)
            {
                RemoveFolder(folder.Id);
            }
            
            //Delete Folder
            StorageCore.Core.DeleteFolder(removeItemId);
        }

        /// <summary>
        /// Removes the QuickConnects, if it is activated in the userprofile or force is set to true
        /// </summary>
        /// <param name="force">Force delete?</param>
        public static void RemoveQuickConnects(bool force)
        {
            if (force || StorageCore.Core.GetUserSettings().DeleteQuickConnect)
            {
                //Get QuickConnect Folder.ID
                var qcFolderId = StorageCore.Core.GetFolderId("Quick Connect");

                //If QuickConnect Folder not exists
                if (qcFolderId == 0)
                    return;

                //Delete the folder
                RemoveFolder(qcFolderId);
            }
        }
    }
}
