using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace beRemote.GUI.ViewModel.Worker
{
    public static class SaveLoad
    {
        /// <summary>
        /// Saves the Current GridLayout of AvalonDock to the Database
        /// </summary>
        /// <param name="dockMgr">The Dockmanager the Layout should saved</param>
        public static void SaveSettingsDock(DockingManager dockMgr)
        {
            var serializer = new XmlLayoutSerializer(dockMgr);

            //If memorystream makes problems, check http://stackoverflow.com/questions/78181/how-do-you-get-a-string-from-a-memorystream
            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms);

                ms.Position = 0;

                var strmReader = new StreamReader(ms);
                var settingDic = new Dictionary<string, object>();
                settingDic.Add("gridlayout", strmReader.ReadToEnd());

                //Contains the Full Layout inclusive the DocumentLayout (open Tabs)
                var fullLayout = settingDic["gridlayout"].ToString().Replace("\r", string.Empty).Split('\n');

                //The Variable for a copy of fullLayout without the DocumentLayouts
                var nonDocumentLayout = "";

                //Copy the fullLayout to nonDocumentLayout and remove the layout Documents
                foreach (var aLine in fullLayout)
                {
                    if (aLine.Trim().StartsWith("<LayoutDocument "))
                        continue;

                    if (aLine.Trim().StartsWith("<LayoutDocumentPane ") && !aLine.Trim().EndsWith("/>")) //Don't remove "<LayoutDocumentPane />" but <LayoutDocumentPane Id="121312321" />
                        continue;

                    if (aLine.Trim() == "<LayoutDocumentPane>")
                        continue;

                    if (aLine.Trim().StartsWith("</LayoutDocumentPane>"))
                    {
                        nonDocumentLayout += "<LayoutDocumentPane />" + System.Environment.NewLine;
                        continue;
                    }

                    nonDocumentLayout += aLine + System.Environment.NewLine;
                }

                //Set the new value
                settingDic["gridlayout"] = nonDocumentLayout;

                //Run the Save async. If you login to beRemote and imediate close beRemote it would result in an application-hang
                Task.Run(() => StorageCore.Core.SetUserVisual(settingDic));

                strmReader.Close();
            }
        }

        /// <summary>
        /// Loads the visual settings of the AvalonDock
        /// </summary>
        /// <param name="dockMgr"></param>
        public static void LoadSettingsDock(DockingManager dockMgr)
        {
            var serializer = new XmlLayoutSerializer(dockMgr);

            try
            {
                using (var ms = new MemoryStream())
                {
                    serializer.LayoutSerializationCallback += (s, e) =>
                                                              {
                                                                  //if (e.Model.ContentId == FileStatsViewModel.ToolContentId)
                                                                  //    e.Content = Workspace.This.FileStats;
                                                                  //else if (!string.IsNullOrWhiteSpace(e.Model.ContentId) &&
                                                                  //    File.Exists(e.Model.ContentId))
                                                                  //    e.Content = Workspace.This.Open(e.Model.ContentId);
                                                              };

                    //Wait for the UserVisuals, if they are not already available
                    while (StorageCore.Core.GetUserVisuals() == null || StorageCore.Core.GetUserVisuals().GridLayout == null)
                        System.Threading.Thread.Sleep(10);

                    var sW = new StreamWriter(ms);
                    sW.Write(StorageCore.Core.GetUserVisuals().GridLayout);
                    sW.Flush();

                    if (ms.Length <= 0)
                        return;

                    ms.Position = 0;
                    var strmReader = new StreamReader(ms);
                    serializer.Deserialize(strmReader);

                    strmReader.Close();
                }
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Warning, "Error on loading Grid-Settings. " + ea.Message, ea);
            }

        }

        /// <summary>
        /// Saves the QAT-Buttons of the Ribbon
        /// </summary>
        /// <param name="qatItems"></param>
        public static void SaveSettingsQatRibbon(ObservableCollection<RibbonButton> qatItems)
        {
            var serialized = Helper.SerializeBase64(qatItems);
            var settingDic = new Dictionary<string, object>();
            settingDic.Add("ribbonqat", serialized);
            StorageCore.Core.SetUserVisual(settingDic);
        }

        /// <summary>
        /// Saves the current TreeViewState
        /// </summary>
        /// <param name="state"></param>
        public static void SaveSettingsTreeViewState(string state)
        {
            //Save the state
            var newDic = new Dictionary<string, object>();
            newDic.Add("expandednodes", state);
            StorageCore.Core.SetUserVisual(newDic);
        }

        /// <summary>
        /// LOads the TreeView-State of a User
        /// </summary>
        /// <returns></returns>
        public static string LoadSettingsTreeViewState()
        {
            return (StorageCore.Core.GetUserVisuals().ExpandedNodes);
        }

        /// <summary>
        /// Loads the Watermark for the Ribbonbar
        /// </summary>
        /// <returns></returns>
        public static ImageSource LoadRibbonWatermark()
        {
            var ribImgId = 0;
            Int32.TryParse(StorageCore.Core.GetSetting("ribbonimg"), out ribImgId);

            //Return beRemote-Logo
            if (ribImgId == 0)
            {
                return (null);
            }


            //Return custom-Logo
            var biWatermark = ByteToBitmapImage(StorageCore.Core.GetData(ribImgId),
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\beRemote",
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\beRemote\\watermark.png");

            if (biWatermark == null)
                return null;

            //var biWatermark = ByteToBitmapImage2(StorageCore.Core.GetData(ribImgId));
            biWatermark.Freeze();
            return (biWatermark);
                                                                  
        }

        /// <summary>
        /// Converts a Byte[] to BitmapImage with temporary Folderpath
        /// </summary>
        /// <param name="data"></param>
        /// <param name="folderpath"></param>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        private static BitmapImage ByteToBitmapImage(byte[] data, string folderpath, string fullpath)
        {
            if (!Directory.Exists(folderpath))
                Directory.CreateDirectory(folderpath);

            if (data == null || data.Length == 0)
                return null;

            var fS = new FileStream(fullpath, FileMode.Create, FileAccess.ReadWrite);
            var bW = new BinaryWriter(fS);

            bW.Write(data);

            bW.Close();
            bW.Dispose();
            fS.Close();
            fS.Dispose();

            var bI = new BitmapImage();
            bI.BeginInit();
            bI.UriSource = new Uri(fullpath, UriKind.RelativeOrAbsolute);
            bI.EndInit();

            return bI;
        }

        //Used to get Image without local file creation. This is not done to avoid using the image on every startup (in a future Release; state: RP3)
        //private static ImageSource ByteToBitmapImage2(byte[] data)
        //{
        //    var biImg = new BitmapImage();
        //    var ms = new MemoryStream(data);
        //    biImg.BeginInit();
        //    biImg.StreamSource = ms;
        //    biImg.EndInit();

        //    var imgSrc = biImg as ImageSource;

        //    return imgSrc;
        //}

        /// <summary>
        /// Saves the state of the Window (maximized or not)
        /// </summary>
        /// <param name="state"></param>
        public static void SaveWindowState(WindowState state)
        {
            var setting = new Dictionary<string, object>();
            setting.Add("mainwindowmax", state == WindowState.Maximized ? true : false);

            StorageCore.Core.SetUserVisual(setting);
        }

        /// <summary>
        /// Loads the state of the Window (maximized or not)
        /// </summary>
        /// <returns></returns>
        public static WindowState LoadWindowState()
        {
            return (LoadWindowState(StorageCore.Core.GetUserVisuals().MainWindowMax));
        }

        /// <summary>
        /// Loads the state of the Window (maximized or not)
        /// </summary>
        /// <returns></returns>
        public static WindowState LoadWindowState(bool isMaximized)
        {
            return (isMaximized ? WindowState.Maximized : WindowState.Normal);
        }

        /// <summary>
        /// Saves the Position of the MainWindow
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SaveWindowPosition(int left, int top, int width, int height)
        {
            var setting = new Dictionary<string, object>();
            setting.Add("mainwindowx", left);
            setting.Add("mainwindowy", top);
            setting.Add("mainwindowwidth", width);
            setting.Add("mainwindowheight", height);

            StorageCore.Core.SetUserVisual(setting);
        }
    }
}