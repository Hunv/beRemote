using beRemote.Core;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Tabs.Import.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.GUI.Tabs.Import.ImportWorker
{
    public class FolderWorker
    {
        #region Events
        public delegate void UpdateEventHandler(object sender, ConverterUpdateEventArgs e);
        public event UpdateEventHandler UpdateTriggered;

        private void triggerUpdate()
        {
            if (UpdateTriggered != null)
            {
                var eventArgs = new ConverterUpdateEventArgs();
                eventArgs.CurrentStatus = _CurrentStatus;
                eventArgs.MaxSteps = _MaxSteps;
                eventArgs.CurrentStep = _CurrentStep;
                eventArgs.Title = _Title;

                this.UpdateTriggered(this, eventArgs);
            }
        }

        private void triggerFinish()
        {
            if (UpdateTriggered != null)
            {
                var eventArgs = new ConverterUpdateEventArgs();
                eventArgs.CurrentStatus = _CurrentStatus;
                eventArgs.Title = _Title;
                eventArgs.CurrentStep = _MaxSteps + 1; //Indicator, that the Import finished

                this.UpdateTriggered(this, eventArgs);
            }
        }
        #endregion

        #region Local Variables
        private int _MaxSteps = 0;
        private int _CurrentStep = 0;
        private string _CurrentStatus = "";
        private string _Title = "";
        #endregion

        /// <summary>
        /// Starts a new Import for compatible file-formats of contents of Directories
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="targetFolderId"></param>
        public async void StartImport(string folderPath, long targetFolderId)
        {
            Logger.Log(LogEntryType.Info, "Starting an Import of a folder");

            var myTask = Task.Run(
                    () =>
                    {
                        _Title = "Import of directory content";
                        _MaxSteps = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories).Length +
                            Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Length;
                        triggerUpdate();

                        Logger.Log(LogEntryType.Verbose, _MaxSteps.ToString() + " Items will be created (Folders + Connections)");

                        ImportDirectory(folderPath, targetFolderId);

                        _CurrentStatus = "Import finished";
                        triggerFinish();

                        Logger.Log(LogEntryType.Info, "Import finished");
                    });

            await myTask;
        }

        /// <summary>
        /// Imports a directory, its containing valid connectionfiles and all subdirectories
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="targetFolderId"></param>
        private void ImportDirectory(string folderPath, long targetFolderId)
        {
            _CurrentStep++;
            _CurrentStatus = "Importing directory " + folderPath;
            triggerUpdate();

            Logger.Log(LogEntryType.Verbose, "Importing directory " + folderPath);

            var subDirectories = Directory.GetDirectories(folderPath);
            var subFiles = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly);

            //Import the directories
            foreach (string aDir in subDirectories)
            {
                Logger.Log(LogEntryType.Verbose, "Creating directory " + aDir);
                var newId = StorageCore.Core.AddFolder(aDir.Split('\\')[aDir.Split('\\').Length-1], targetFolderId, false);
                Logger.Log(LogEntryType.Verbose, "... Created");

                //Import the subdirectories and subconnections
                ImportDirectory(aDir, newId);
            }
            
            //Import the connections
            foreach (string aFile in subFiles)
            {
                string fileending = aFile.Split('.')[aFile.Split('.').Length - 1].ToLower();

                switch (fileending)
                { 
                    default:
                        _CurrentStep++;
                        _CurrentStatus = "Unsupported file-format " + aFile;
                        triggerUpdate();

                        Logger.Log(LogEntryType.Info, "File " + aFile + " is not a supported filetype");
                        break;
                    case "rdp":
                        ImportFileRdp(aFile, targetFolderId);
                        break;
                    case "url":
                        ImportFileUrl(aFile, targetFolderId);
                        break;
                    case "vnc":
                        ImportFileVnc(aFile, targetFolderId);
                        break;
                }

            }
        }

        /// <summary>
        /// Imports a RDP-File to the given Folder
        /// </summary>
        /// <param name="path">The path to the RDP-File</param>
        /// <param name="parentFolder">The ID of the parent folder the connection will be stored in</param>
        private void ImportFileRdp(string path, long parentFolder)
        {
            _CurrentStep++;
            _CurrentStatus = "Importing RDP-File " + path;
            triggerUpdate();
            Logger.Log(LogEntryType.Verbose, "Importing RDP-File " + path);

            //Check if RDP-Protocol is available
            if (!Kernel.GetAvailableProtocols().ContainsKey("beRemote.VendorProtocols.RDP.RDProtocol"))
            {
                _CurrentStatus = "No RDP-Plugin available";
                triggerUpdate();

                Logger.Log(LogEntryType.Verbose, "... canceled: RDP-Protocol not available");
                return;
            }

            //Variables for the final Connection
            string hostname = "";
            string displayname = Path.GetFileNameWithoutExtension(path);
            int port = 3389;
            Dictionary<string, object> connectionOptions = new Dictionary<string, object>();

            //Open the RDP-File
            var sR = new StreamReader(path, Encoding.Default);
            
            //Read each line of the RDP-File
            while (sR.Peek() >= 0)
            {
                string line = sR.ReadLine();

                //Search the targeting Computername
                if (line.StartsWith("full address:s:"))
                {
                    hostname = line.Split(':')[line.Split(':').Length - 1];
                }
                else if (line.StartsWith("session bpp:i:"))
                {
                    connectionOptions.Add("rdp.color.qty", Convert.ToInt32(line.Split(':')[line.Split(':').Length - 1]));
                }
                else if (line.StartsWith("disable wallpaper:i:"))
                {
                    var mode = line.Split(':')[line.Split(':').Length - 1];
                    connectionOptions.Add("rdp.performance.drawbackground", mode != "0");
                }
                else if (line.StartsWith("allow font smoothing:i:"))
                {
                    var mode = line.Split(':')[line.Split(':').Length - 1];
                    connectionOptions.Add("rdp.performance.fontsmothing", mode != "0");
                }
                else if (line.StartsWith("disable themes:i:"))
                {
                    var mode = line.Split(':')[line.Split(':').Length - 1];
                    connectionOptions.Add("rdp.performance.windowsthemes", mode != "0");
                }
                else if (line.StartsWith("disable menu anims:i:"))
                {
                    var mode = line.Split(':')[line.Split(':').Length - 1];
                    connectionOptions.Add("rdp.performance.showanimations", mode != "0");
                }
                else if (line.StartsWith("audiomode:i:"))
                {
                    var mode = Convert.ToInt32(line.Split(':')[line.Split(':').Length - 1]);
                    connectionOptions.Add("rdp.redirect.sound", mode != 0);
                }
                else if (line.StartsWith("redirectprinters:i:"))
                {
                    var mode = Convert.ToInt32(line.Split(':')[line.Split(':').Length - 1]);
                    connectionOptions.Add("rdp.redirect.printer", mode != 0 );
                }
                else if (line.StartsWith("drivestoredirect:s:"))
                {
                    var drives = line.Split(':')[line.Split(':').Length - 1];
                    connectionOptions.Add("rdp.redirect.drives", drives.Length > 0);
                }
                else if (line.StartsWith("negotiate security layer:i:"))
                {
                    var mode = line.Split(':')[line.Split(':').Length - 1];
                    connectionOptions.Add("rdp.security.authentication.nla", mode != "0");
                }
                else if (line.StartsWith("gatewayhostname:s:"))
                {
                    var gateway = line.Split(':')[line.Split(':').Length - 1].ToString();

                    //No Gateway
                    if (gateway.Length == 0)
                    {
                        connectionOptions.Add("rdp.gateway.enabled", false);
                        connectionOptions.Add("rdp.gateway.credentials", 0);
                        connectionOptions.Add("rdp.gateway.server", "");
                        continue;
                    }

                    //Has a Gateway
                    connectionOptions.Add("rdp.gateway.enabled", true);
                    connectionOptions.Add("rdp.gateway.credentials", 0);
                    connectionOptions.Add("rdp.gateway.server", gateway);
                }
            }

            //Resolution will be allways ignored and smartsize will be activated
            connectionOptions.Add("rdp.resolution", "fittotab");
            connectionOptions.Add("rdp.smartsize", true);
            connectionOptions.Add("rdp.protocol.useconsole", false);
            connectionOptions.Add("rdp.protocol.authentication", false);

            sR.Close();

            //Portdefinition is available
            if (hostname.Contains(':'))
            {
                port = Convert.ToInt32(hostname.Split(':')[1]);
                hostname = hostname.Split(':')[0];
            }

            //Verify data
            if (hostname.Length == 0 ||
                displayname.Length == 0 ||
                parentFolder == 0)
            {
                Logger.Log(LogEntryType.Verbose, String.Format("... canceled. Inconsistend RDP-Data. Details: Host:\"{0}\" Displayname:\"{1}\" Parent:\"{2}\"", hostname, displayname, parentFolder));
                return;
            }

            if (Kernel.GetAvailableProtocols()["beRemote.VendorProtocols.RDP.RDProtocol"].GetBaseSettings().Count != connectionOptions.Count)
            {
                Logger.Log(LogEntryType.Verbose, "... canceled. Missing RDP-Settings");
                return;
            }

            //Create the connection
            var newConnectionId = StorageCore.Core.AddConnection(hostname, displayname, "Imported connection", 0, parentFolder, StorageCore.Core.GetUserId(), false, 0);
            var newConnectionProtocolId = StorageCore.Core.AddConnectionSetting(newConnectionId, "beRemote.VendorProtocols.RDP.RDProtocol", port);

            foreach(var kvp in connectionOptions)
            {
                StorageCore.Core.ModifyConnectionOption(kvp.Value, kvp.Key, newConnectionProtocolId);
            }

            Logger.Log(LogEntryType.Verbose, "... finished");
        }

        /// <summary>
        /// Imports URL-Files with links to websites
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parentFolder"></param>
        private void ImportFileUrl(string path, long parentFolder)
        {
            _CurrentStep++;
            _CurrentStatus = "Importing URL-File " + path;
            triggerUpdate();

            Logger.Log(LogEntryType.Verbose, "Importing URL-File " + path);
            
            //Check if RDP-Protocol is available
            if (!Kernel.GetAvailableProtocols().ContainsKey("beRemote.VendorProtocols.IETrident.IETridentProtocol"))
            {
                _CurrentStatus = "No URL-Plugin available";
                triggerUpdate();

                Logger.Log(LogEntryType.Verbose, "... canceled. No URL-Plugin available");
                return;
            }

             //Variables for the final Connection
            string hostname = "";
            string displayname = Path.GetFileNameWithoutExtension(path);
            int port = 80;
            Dictionary<string, object> connectionOptions = new Dictionary<string, object>();

            //Open the File
            var sR = new StreamReader(path, Encoding.Default);
            
            //Read each line of the RDP-File
            while (sR.Peek() >= 0)
            {
                string line = sR.ReadLine();

                //Search the targeting Computername
                if (line.StartsWith("URL="))
                {
                    hostname = line.Substring(4);
                }
            }

            sR.Close();
            
            //Verify data
            if (hostname.Length == 0 ||
                displayname.Length == 0 ||
                parentFolder == 0)
            {
                Logger.Log(LogEntryType.Verbose, String.Format("... canceled. Inconsistend URL-Data. Details: Host:\"{0}\" Displayname:\"{1}\" Parent:\"{2}\"", hostname, displayname, parentFolder));
                return;
            }

            if (Kernel.GetAvailableProtocols()["beRemote.VendorProtocols.IETrident.IETridentProtocol"].GetBaseSettings().Count != connectionOptions.Count)
            {
                Logger.Log(LogEntryType.Verbose, "... canceled. Missing URL-Settings");
                return;
            }

            //Create the connection
            var newConnectionId = StorageCore.Core.AddConnection(hostname, displayname, "Imported connection", 0, parentFolder, StorageCore.Core.GetUserId(), false, 0);
            var newConnectionProtocolId = StorageCore.Core.AddConnectionSetting(newConnectionId, "beRemote.VendorProtocols.IETrident.IETridentProtocol", port);

            foreach (var kvp in connectionOptions)
            {
                StorageCore.Core.ModifyConnectionOption(kvp.Value, kvp.Key, newConnectionProtocolId);
            }

            Logger.Log(LogEntryType.Verbose, "... finished");
        }

        /// <summary>
        /// Imports VNC-Files
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parentFolder"></param>
        private void ImportFileVnc(string path, long parentFolder)
        {
            _CurrentStep++;
            _CurrentStatus = "Importing VNC-File " + path;
            triggerUpdate();

            Logger.Log(LogEntryType.Verbose, "Importing VNC-File " + path);

            //Check if RDP-Protocol is available
            if (!Kernel.GetAvailableProtocols().ContainsKey("beRemote.VendorProtocols.VNC.VNCProtocol"))
            {
                _CurrentStatus = "No VNC-Plugin available";
                triggerUpdate();

                Logger.Log(LogEntryType.Verbose, "... canceled. No VNC-Plugin available");

                return;
            }

            //Variables for the final Connection
            string hostname = "";
            string displayname = Path.GetFileNameWithoutExtension(path);
            int port = 5900;
            Dictionary<string, object> connectionOptions = new Dictionary<string, object>();

            //Open the File
            var sR = new StreamReader(path, Encoding.Default);

            //Read each line of the RDP-File
            while (sR.Peek() >= 0)
            {
                string line = sR.ReadLine();

                //Search the targeting Computername
                if (line.StartsWith("host="))
                {
                    hostname = line.Substring(5);
                }
                else if (line.StartsWith("port="))
                {
                    port = Convert.ToInt32(line.Substring(5));
                }
            }

            sR.Close();

            //Verify data
            if (hostname.Length == 0 ||
                displayname.Length == 0 ||
                parentFolder == 0)
            {
                Logger.Log(LogEntryType.Verbose, String.Format("... canceled. Inconsistend VNC-Data. Details: Host:\"{0}\" Displayname:\"{1}\" Parent:\"{2}\"", hostname, displayname, parentFolder));
                return;
            }

            if (Kernel.GetAvailableProtocols()["beRemote.VendorProtocols.VNC.VNCProtocol"].GetBaseSettings().Count != connectionOptions.Count)
            {
                Logger.Log(LogEntryType.Verbose, "... canceled. Missing VNC-Settings");
                return;
            }

            //Create the connection
            var newConnectionId = StorageCore.Core.AddConnection(hostname, displayname, "Imported connection", 0, parentFolder, StorageCore.Core.GetUserId(), false, 0);
            var newConnectionProtocolId = StorageCore.Core.AddConnectionSetting(newConnectionId, "beRemote.VendorProtocols.VNC.VNCProtocol", port);

            foreach (var kvp in connectionOptions)
            {
                StorageCore.Core.ModifyConnectionOption(kvp.Value, kvp.Key, newConnectionProtocolId);
            }

            Logger.Log(LogEntryType.Verbose, "... finished");
        }
    }
}
