using beRemote.Core.Common.LogSystem;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Tabs.Import.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace beRemote.GUI.Tabs.Import.ImportWorker
{
    public class MRemoteWorker
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

        #region Properties
        private bool _CancelLoading = false;
        public bool CancelLoading
        {
            get { return (_CancelLoading); }
            set { _CancelLoading = value; }
        }
        #endregion

        private int _MaxSteps = 0;
        private int _CurrentStep = 0;
        private string _CurrentStatus = "";
        private string _Title = "";

        /// <summary>
        /// Imports a confCons.xml, created by mRemote, to beRemote
        /// </summary>
        /// <param name="xmlPath">The Path to the confCons.xml</param>
        private Dictionary<string, int> ImportMRemoteXml(string xmlPath, long destinationFolderId)
        {
            //Loadingwindow Information
            _Title = "mRemote import";
            _MaxSteps = System.IO.File.ReadAllLines(xmlPath).Length;

            //Load folder to check if it is Public (SuperAdmin-Added Folders can be public)
            bool isFolderPublic = StorageCore.Core.GetFolder(destinationFolderId).IsPublic;

            //Check categories available and add them to the combobox            
            XmlReader xmlRd = XmlReader.Create(xmlPath);

            List<long> parentId = new List<long>();
            parentId.Add(destinationFolderId); //Add the root-ID (the ID of the DestinationFolder)

            int conSuccess = 0;
            int conFail = 0;
            int folderAdd = 0;
            int success = 1;

            //Read the mirror.xml
            while (xmlRd.Read() && _CancelLoading == false)
            {
                if (xmlRd.NodeType != XmlNodeType.Whitespace) _CurrentStep++; //Update for each No-Whitespace-Element

                if (xmlRd.NodeType == XmlNodeType.Element) //Only Elements are relevant
                {
                    _CurrentStatus = "Importing " + xmlRd.GetAttribute("Name");
                    triggerUpdate();

                    //Check if it is a Container (=Folder)
                    if (xmlRd.GetAttribute("Type") == "Container")
                    {
                        try
                        {
                            long folderId = 0; //Contains the Id of this Container/Folder

                            //Check if Folder already exists; create if not
                            if (StorageCore.Core.GetFolderExists(xmlRd.GetAttribute("Name"), StorageCore.Core.GetUserId(), parentId[parentId.Count - 1]) == false)
                            {
                                folderId = StorageCore.Core.AddFolder(xmlRd.GetAttribute("Name"), parentId[parentId.Count - 1], isFolderPublic);
                                folderAdd++;
                            }
                            else
                            {
                                folderId = StorageCore.Core.GetFolderId(xmlRd.GetAttribute("Name"));
                            }
                            parentId.Add(folderId);
                        }
                        catch (Exception)
                        {
                            Logger.Log(LogEntryType.Warning, "Error importing a mRemote-Folder; canceling");
                            success = 0;
                            break;
                        }
                    }
                    else if (xmlRd.GetAttribute("Type") == "Connection") //If it is a Connection
                    {
                        try
                        {
                            string protName = getLocalProtocolName(xmlRd.GetAttribute("Protocol"));

                            if (protName == "") //Protocol not implemented
                            {
                                conFail++;
                                continue;
                            }

                            long conId = StorageCore.Core.AddConnection(
                                xmlRd.GetAttribute("Hostname"),
                                xmlRd.GetAttribute("Name"),
                                xmlRd.GetAttribute("Descr"),
                                0, parentId[parentId.Count - 1],
                                StorageCore.Core.GetUserId(),
                                false,
                                0);

                            StorageCore.Core.AddConnectionSetting(
                                conId,
                                protName,
                                Convert.ToInt32(xmlRd.GetAttribute("Port")));

                            conSuccess++;
                        }
                        catch (Exception)
                        {
                            Logger.Log(LogEntryType.Warning, "Error on importing a connection; continueing");
                            conFail++;
                            continue;
                        }
                    }
                }
                else if (xmlRd.NodeType == XmlNodeType.EndElement)
                    parentId.RemoveAt(parentId.Count - 1); //Remove the current latest folder
            }

            if (_CancelLoading == true)
            {
                Logger.Log(LogEntryType.Info, "mRemote-Connectionimport was canceled by user");
            }


            Dictionary<string, int> ret = new Dictionary<string, int>();
            ret.Add("success", success);
            ret.Add("Folders imported", folderAdd);
            ret.Add("Connections imported", conSuccess);
            ret.Add("Connections failed", conFail);

            triggerFinish();

            return (ret);
        }

        private string getLocalProtocolName(string mRemoteProtocolName)
        {
            SortedList<string, beRemote.Core.ProtocolSystem.ProtocolBase.Protocol> availableProtocols = beRemote.Core.Kernel.GetAvailableProtocols();
            switch (mRemoteProtocolName)
            {
                default:          
                case "Rlogin":
                case "RAW":
                case "ICA":
                case "IntApp":
                    return ("");

                case "SSH1":
                case "SSH2":
                    if (availableProtocols.ContainsKey("beRemote.VendorProtocols.SSH"))
                        return ("beRemote.VendorProtocols.SSH");
                    else
                        return "";  

                case "HTTP":
                case "HTTPS":
                    if (availableProtocols.ContainsKey("beRemote.VendorProtocols.IETrident"))
                        return ("beRemote.VendorProtocols.IETrident");
                    else if (availableProtocols.ContainsKey("beRemote.VendorProtocols.Chromium"))
                        return ("beRemote.VendorProtocols.Chromium");
                    else
                        return "";

                case "VNC":
                    if (availableProtocols.ContainsKey("beeVNC.beRemoteProtocol"))
                        return ("beeVNC.beRemoteProtocol");
                    else
                        return "";

                case "RDP":
                    if (availableProtocols.ContainsKey("beRemote.VendorProtocols.RDP.RDProtocol"))
                        return ("beRemote.VendorProtocols.RDP.RDProtocol");
                    else
                        return ("");

                case "Telnet":
                    if (availableProtocols.ContainsKey("beRemote.VendorProtocols.Telnet.TelnetProtocol"))
                        return ("beRemote.VendorProtocols.Telnet.TelnetProtocol");
                    else
                        return ("");
            }

        }

        /// <summary>
        /// Starts the Import of a mRemote XML-File in a seperate thread
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="destinationFolderId"></param>
        public async void StartImport(string xmlPath, long destinationFolderId)
        {
            var myTask = Task.Run(
                () =>
                {
                    ImportMRemoteXml(xmlPath, destinationFolderId);
                });            

            await myTask;
        }
    }
}
