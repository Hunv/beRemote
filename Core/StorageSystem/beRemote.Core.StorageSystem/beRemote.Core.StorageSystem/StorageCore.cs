using System;
using System.Collections.Generic;
using System.Text;
using beRemote.Core.Common.SimpleSettings;
using System.Reflection;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using System.Security.Permissions;
using System.Security;

namespace beRemote.Core.StorageSystem.StorageBase
{
    public class StorageCore
    {
        //The only possible instance of this class
        private static StorageCore _StorageCore;
        private static IDbPlugin _StoragePlugin;

        private StorageCore()
        {
            Logger.Log(LogEntryType.Verbose, "Creating StorageCore");

            //Load configfile
            var Config = Helper.GetApplicationConfiguration();

            //Load DatabaseDLL-Path
            var databaseDll = Config.GetValue("database", "dbinterface");

            //Unblock Database-Plugin
            Helper.UnblockFile(databaseDll);

            if (System.IO.File.Exists("SQLite.Interop.dll"))
                Helper.UnblockFile("SQLite.Interop.dll");

            Logger.Log(LogEntryType.Verbose, "Loading Database-Plugin " + databaseDll);
            var dbAssembly = Assembly.LoadFrom(databaseDll);
            Logger.Log(LogEntryType.Verbose, "   ... Loaded " + databaseDll);

            try
            {
                //Check Assemblytypes
                foreach (var type in dbAssembly.GetTypes())
                {
                    //If type is IDbPlugin-Interface
                    if (type.GetInterface("IDbPlugin") == null)
                        continue;

                    //Create Instance
                    _StoragePlugin = (IDbPlugin)Activator.CreateInstance(type);

                    if (_StoragePlugin.InitPlugin() == false)
                    {
                        Logger.Log(LogEntryType.Exception, "Cannot initialize StoragePlugin. Please check the requirements of the plugin and the logfiles.", "StorageCore");
                    }
                    else
                    {
                        Logger.Log(LogEntryType.Debug, "StoragePlugin initialized", "StorageCore");
                    }
                    break;
                }
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Unable to initialize Database Interface!", ea);                
            }
        }

        /// <summary>
        /// Get the current StorageCore-Instance
        /// </summary>
        public static IDbPlugin Core
        {
            get 
            {
                if (false ==
                    new System.Diagnostics.StackTrace(true).GetFrame(1)
                        .GetMethod()
                        .DeclaringType.FullName.StartsWith("beRemote."))
                {
                    // If this comes true we are going to interupt the caller without any information about any reasons....
                    return null;
                }

                //Check if there is already an Instance
                if (_StorageCore != null) 
                    return (_StoragePlugin);

                //Create a new instance
                Logger.Log(LogEntryType.Verbose, "No StorageCore initialized, creating new instance");

                //Initialize the StorageCore
                _StorageCore = new StorageCore();

                Logger.Log(LogEntryType.Verbose, "Storagecore initialized");

                //Return the existing instance
                return (_StoragePlugin);
            }
        }

        public static void UpdateDatabase()
        {
            Logger.Log(LogEntryType.Info, "Check for updates", "StorageCore");

            //Check, if the Database is up to date
            if (DatabaseDefinition.UpdateDatabaseStatus(true))
            {
                Logger.Log(LogEntryType.Info, "... No Update available", "StorageCore");
            }
            else
            {
                Logger.Log(LogEntryType.Info, "... Update required", "StorageCore");

                //Don't handle old Database-Format
                try
                {
                    if (Convert.ToInt32(Core.GetSetting("version")) < 10)
                    {
                        Logger.Log(LogEntryType.Info, "... Update not available. Database Version is old. Please remove the \"beRemote.db\" from %appdata%\\beRemote. If you like to import your connections, get the Tool \"Database Converter\" from the Plugin Directory", "StorageCore");
                        return;
                    }
                }
                catch (Exception)
                {
                    
                }
              

                //perform update
                DatabaseDefinition.UpdateDatabaseStatus(false);
            }
        }
    }
}
