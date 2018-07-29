using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.Command;

namespace beRemote.GUI.Tabs.ManageFolder
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Command Implementation
        public CmdTabManageFolderLoadedImpl CmdTabManageFolderLoaded { get; set; }
        public CmdTabManageFolderAddFolderClickImpl CmdTabManageFolderAddFolderClick { get; set; }
        #endregion

        #region Constructor

        public ViewModel()
        {
            CmdTabManageFolderLoaded = new CmdTabManageFolderLoadedImpl();
            CmdTabManageFolderLoaded.TabManageFolderLoaded += CmdTabManageFolderLoaded_TabManageFolderLoaded;

            CmdTabManageFolderAddFolderClick = new CmdTabManageFolderAddFolderClickImpl();
            CmdTabManageFolderAddFolderClick.TabManageFolderAddFolderClick += CmdTabManageFolderAddFolderClick_TabManageFolderAddFolderClick;
        }

        #endregion

        #region Event executions

        void CmdTabManageFolderLoaded_TabManageFolderLoaded(object sender, RoutedEventArgs e)
        {
            GetFolderList();

            if (StartupFolder == null)
                return;

            SelectedFolder = StartupFolder;

            return;
        }


        void CmdTabManageFolderAddFolderClick_TabManageFolderAddFolderClick(object sender, FolderAddEventArgs e)
        {
            if ((SelectedFolder == null && IsRoot == false) || NewFolderName.Length == 0)
                return;

            StorageCore.Core.AddFolder(NewFolderName, IsRoot ? 0 : SelectedFolder.ConnectionID, IsPublic);
            e.View.RefreshConnectionList();

            if (KeepOpen == false)
            {
                e.View.CloseTab();
            }
            else
            {
                NewFolderName = "";
                var selFolderBeforeRefresh = SelectedFolder;
                GetFolderList();
                SelectedFolder = selFolderBeforeRefresh;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a non-Stacked List of all Folders. So there is just one Collection, that includes all Items. The Subconnections-Property is Unused.
        /// </summary>
        private void GetFolderList()
        {
            var listFolder = StorageCore.Core.GetFolders(); //Contains all folders, that are not directed to a parent
            var rootList = new List<ConnectionItem>(); //Contains all folders, that are directed to a parent
            
            foreach (var aFolder in listFolder)
            {
                if (aFolder.ParentId == 0)
                {
                    rootList.InsertRange(rootList.Count, GetSubFolders(aFolder.Id, listFolder, 0));
                }
            }

            FolderList = new ObservableCollection<ConnectionItem>(rootList);
        }

        private List<ConnectionItem> GetSubFolders(long mainFolderId, List<Folder> folderList, byte rootLevel)
        {
            var conItem = new ConnectionItem("tempname");
            var conList = new List<ConnectionItem>();

            //Foreach Folder
            foreach (var aFolder in folderList)
            {
                //Search the folder itself
                if (aFolder.Id == mainFolderId)
                {
                    conItem.ConnectionName = aFolder.Name;
                    conItem.ConnectionID = aFolder.Id;
                    conItem.RootLevel = rootLevel;
                }

                //Search the Folder-Childs
                if (aFolder.ParentId == mainFolderId)
                {
                    var listSubConnections = GetSubFolders(aFolder.Id, folderList, (byte)(rootLevel+1));
                    conItem.SubConnections = new ObservableCollection<ConnectionItem>(listSubConnections);
                    conList.InsertRange(conList.Count, listSubConnections);
                }
            }

            conList.Insert(0, conItem);

            return (conList);
        }

        /// <summary>
        /// Gets a FolderList, where the Root-Items are the only containing Items. This Root-Items containing the next Level of items and so on
        /// </summary>
        private void GetStackedFolderList()
        {
            var listFolder = StorageCore.Core.GetFolders(); //Contains all folders, that are not directed to a parent
            var rootList = new List<ConnectionItem>(); //Contains all folders, that are directed to a parent

            foreach (var aFolder in listFolder)
            {
                if (aFolder.ParentId == 0)
                {
                    rootList.Add(GetStackedSubFolders(aFolder.Id, listFolder, 0));
                }
            }

            FolderList = new ObservableCollection<ConnectionItem>(rootList);
        }

        private ConnectionItem GetStackedSubFolders(long mainFolderId, List<Folder> folderList, byte rootLevel)
        {
            var conItem = new ConnectionItem("tempname");

            //Foreach Folder
            foreach (var aFolder in folderList)
            {
                //Search the folder itself
                if (aFolder.Id == mainFolderId)
                {
                    conItem.ConnectionName = aFolder.Name;
                    conItem.ConnectionID = aFolder.Id;
                    conItem.RootLevel = rootLevel;
                }

                //Search the Folder-Childs
                if (aFolder.ParentId == mainFolderId)
                {
                    conItem.SubConnections.Add(GetStackedSubFolders(aFolder.Id, folderList, rootLevel++));
                }
            }

            return (conItem);
        }

        #endregion

        #region Properties

        #region NewFolderName
        private string _NewFolderName = "";

        /// <summary>
        /// The Name of the new Folder
        /// </summary>
        public string NewFolderName
        {
            get { return _NewFolderName; }
            set
            {
                _NewFolderName = value;
                RaisePropertyChanged("NewFolderName");
            }
        }
        #endregion

        #region FolderList
        private ObservableCollection<ConnectionItem> _FolderList = new ObservableCollection<ConnectionItem>();

        /// <summary>
        /// Contains the FolderList
        /// </summary>
        public ObservableCollection<ConnectionItem> FolderList
        {
            get { return _FolderList; }
            set
            {
                _FolderList = value;
                RaisePropertyChanged("FolderList");
            }
        }
        #endregion

        #region SelectedFolder
        private ConnectionItem _SelectedFolder;
        public ConnectionItem SelectedFolder 
        {
            get { return _SelectedFolder; } 
            set 
            { 
                //_SelectedFolder = value;
                if (value == null)
                {
                    _SelectedFolder = null;
                    RaisePropertyChanged("SelectedFolder");
                    return;
                }

                foreach (var conItm in FolderList)
                {
                    if (conItm.ConnectionID != value.ConnectionID)
                        continue;

                    _SelectedFolder = conItm;

                    //If the preselected folder is public, check IsPublic-Checkbox
                    if (StorageCore.Core.GetFolder(conItm.ConnectionID).IsPublic)
                    {
                        IsPublic = true;
                        RaisePropertyChanged("IsPublic");
                    }
                }

                RaisePropertyChanged("SelectedFolder"); 
            } 
        }
        #endregion

        #region Public
        public bool IsPublic { get;set;}
        #endregion

        #region IsRoot

        private bool _IsRoot;
        public bool IsRoot
        {
            get { return _IsRoot; }
            set
            {
                _IsRoot = value;
                RaisePropertyChanged("IsRoot");
            }
        }
        #endregion

        #region KeepOpen
        public bool KeepOpen { get; set; }
        #endregion

        #region StartupFolder
        public ConnectionItem StartupFolder { get; set; }
        #endregion

        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        /// <summary>
        /// Helper for Triggering PropertyChanged
        /// </summary>
        /// <param name="triggerControl">The Name of the Property to update</param>
        private void RaisePropertyChanged(string triggerControl)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(triggerControl));
            }
        }
        #endregion

        public void Dispose()
        {
            CmdTabManageFolderLoaded.TabManageFolderLoaded -= CmdTabManageFolderLoaded_TabManageFolderLoaded;
            CmdTabManageFolderAddFolderClick.TabManageFolderAddFolderClick -= CmdTabManageFolderAddFolderClick_TabManageFolderAddFolderClick;

            CmdTabManageFolderLoaded = null;
            CmdTabManageFolderAddFolderClick = null;

            _FolderList.Clear();
            _SelectedFolder = null;
            StartupFolder = null;
        }
    }
}
