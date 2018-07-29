using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.ViewModel.Worker
{
    public static class Startup
    {
        /// <summary>
        /// Checks Resources beRemote requires to run. If they not exists, they will be created
        /// </summary>
        public static Tuple<bool, string> CheckResources()
        {
            //Check %appdata%\beRemote-Folder for existence
            if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\beRemote"))
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\beRemote");

            //Check %localappdata%\beRemote-Folder for existence
            if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\beRemote"))
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\beRemote");

            //Check Database-Version. Is it smaller than 10, it is incompatible
            try
            {
                if (Convert.ToInt32(StorageCore.Core.GetSetting("version")) < 10)
                {
                    return
                        (new Tuple<bool, string>(false,
                            "Database Version is old. Please remove the \"beRemote.db\" from %appdata%\\beRemote. If you like to import your connections, get the Tool \"Database Converter\" from the Plugin Directory"));
                }

            }
            catch (Exception ex)
            {
                Logger.Log(LogEntryType.Info, "No Database available: Version lookup failed", ex);
            }
            
            return (new Tuple<bool, string>(true, ""));
        }
    }
}
