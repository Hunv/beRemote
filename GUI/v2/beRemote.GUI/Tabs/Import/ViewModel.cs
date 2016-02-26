using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.Tabs.Import.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.Tabs.Import
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region local definitons
        #region Command Definition
        public CmdBrowseImpl CmdBrowse {get;set;}
        public CmdCancelImpl CmdCancel { get; set; }
        public CmdImportImpl CmdImport { get; set; }
        public CmdTabLoadedImpl CmdTabLoaded { get; set; }
        #endregion

        private readonly ResourceDictionary _LangDictionary = new ResourceDictionary(); //Contains the Language-Variables

        #region ConverterWorker
        ImportWorker.MRemoteWorker _MRemoteWorker;
        ImportWorker.FolderWorker _FolderWorker;
        ImportWorker.CsvWorker _CsvWorker;
        #endregion
        #endregion

        #region Constructor
        public ViewModel()
        {
            #region Load Language Dictionary
            var dictionaryFiles = new List<string>
                                           {
                                               "Tabs/Import/Language/language.xaml",
                                               "Tabs/Import/Language/language.de-DE.xaml",
                                               "Tabs/Import/Language/language.es-ES.xaml",
                                               "Tabs/Import/Language/language.fr-FR.xaml",
                                               "Tabs/Import/Language/language.it-IT.xaml",
                                               "Tabs/Import/Language/language.nl-NL.xaml",
                                               "Tabs/Import/Language/language.pl-PL.xaml",
                                               "Tabs/Import/Language/language.ru-RU.xaml",
                                               "Tabs/Import/Language/language.zh-CN.xaml",
                                               "Tabs/Import/Language/language.cs-CZ.xaml",
                                               "Tabs/Import/Language/language.ar-SA.xaml",
                                               "Tabs/Import/Language/language.bg-BG.xaml",
                                               "Tabs/Import/Language/language.dk-DK.xaml",
                                               "Tabs/Import/Language/language.el-GR.xaml",
                                               "Tabs/Import/Language/language.fa-IR.xaml",
                                               "Tabs/Import/Language/language.fi-FI.xaml",
                                               "Tabs/Import/Language/language.he-IL.xaml",
                                               "Tabs/Import/Language/language.hi-IN.xaml",
                                               "Tabs/Import/Language/language.hr-HR.xaml",
                                               "Tabs/Import/Language/language.hu-HU.xaml",
                                               "Tabs/Import/Language/language.ko-KR.xaml",
                                               "Tabs/Import/Language/language.nn-NO.xaml",
                                               "Tabs/Import/Language/language.se-SE.xaml",
                                               "Tabs/Import/Language/language.tr-TR.xaml",
                                               "Tabs/Import/Language/language.zh-CN.xaml"
                                           };

            foreach (var aLangfile in dictionaryFiles)
                _LangDictionary.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(aLangfile, UriKind.Relative) });

            #endregion

            #region Command initialisation
            CmdBrowse = new CmdBrowseImpl();
            CmdCancel = new CmdCancelImpl();
            CmdImport = new CmdImportImpl();
            CmdTabLoaded = new CmdTabLoadedImpl();

            CmdImport.StartImport += CmdImport_StartImport;
            CmdTabLoaded.TabLoaded += CmdTabLoaded_TabLoaded;
            CmdBrowse.StartBrowsing += CmdBrowse_StartBrowsing;
            #endregion
        }
        #endregion

        #region Command Hooks
        void CmdImport_StartImport(object sender, System.Windows.RoutedEventArgs e)
        {
            //Validate informations
            if (SourcePath.Length == 0 ||
                SelectedTarget == null || SelectedTarget.ConnectionID == 0)
            {
                StatusText = _LangDictionary["TabImportInformationMissing"].ToString();
                return;
            }

            if (IsModeMRemote)
            {
                _MRemoteWorker = new ImportWorker.MRemoteWorker();
                _MRemoteWorker.UpdateTriggered += Worker_UpdateTriggered;
                _MRemoteWorker.StartImport(SourcePath, SelectedTarget.ConnectionID);
            }
            else if (IsModeFolder)
            {
                _FolderWorker = new ImportWorker.FolderWorker();
                _FolderWorker.UpdateTriggered += Worker_UpdateTriggered;
                _FolderWorker.StartImport(SourcePath, SelectedTarget.ConnectionID);
            }
            else if (IsModeCsv)
            { }
        }
        
        void CmdBrowse_StartBrowsing(object sender, RoutedEventArgs e)
        {
            if (IsModeFolder)
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.ShowDialog();
                fbd.ShowNewFolderButton = false;
                SourcePath = fbd.SelectedPath;
                fbd.Dispose();
            }
            else
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Multiselect = false;
                dlg.ShowDialog();
                SourcePath = dlg.FileName;                
            }
        }

        private void CmdTabLoaded_TabLoaded(object sender, EventArgs.TabLoadedEventArgs e)
        {
            FolderList = GetFolderList();
            RaisePropertyChanged("FolderList");

            SetSourceExample("mremote");
        }
        #endregion

        #region Command Hook related Methods

        void Worker_UpdateTriggered(object sender, EventArgs.ConverterUpdateEventArgs e)
        {
            //Update Statustext
            StatusText = e.CurrentStatus;
            StatusTitle = e.Title;

            //If Import is finished
            if (e.CurrentStep > e.MaxSteps)
            {
                StatusStepCurrent = e.MaxSteps;

                Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                {
                    FolderList = GetFolderList();
                    RaisePropertyChanged("FolderList");
                }));

                //Reload the beRemote-ConnectionList
                OnRefreshConnectionList(new RoutedEventArgs());
            }
            //regular Update
            else
            {
                StatusStepCurrent = e.CurrentStep;
                StatusStepMax = e.MaxSteps;
            }
        }

        #region Get FolderList
        /// <summary>
        /// Gets a non-Stacked List of all Folders. So there is just one Collection, that includes all Items. The Subconnections-Property is Unused.
        /// </summary>
        private ObservableCollection<ConnectionItem> GetFolderList()
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

            return (new ObservableCollection<ConnectionItem>(rootList));
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
                    var listSubConnections = GetSubFolders(aFolder.Id, folderList, (byte)(rootLevel + 1));
                    conItem.SubConnections = new ObservableCollection<ConnectionItem>(listSubConnections);
                    conList.InsertRange(conList.Count, listSubConnections);
                }
            }

            conList.Insert(0, conItem);

            return (conList);
        }
        #endregion
        #endregion

        #region Properties

        #region IsModeMRemote
        private bool _IsModeMRemote = true;

        /// <summary>
        /// Is mRemote-Mode selected?
        /// </summary>
        public bool IsModeMRemote 
        {
            get { return _IsModeMRemote; }
            set 
            { 
                _IsModeMRemote = value;
                
                if (value)
                    SetSourceExample("mremote");

                RaisePropertyChanged("IsModeMRemote");
            } 
        }
        #endregion

        #region IsModeFolder
        private bool _IsModeFolder;

        /// <summary>
        /// Is Folder-Mode selected?
        /// </summary>
        public bool IsModeFolder
        {
            get { return _IsModeFolder; }
            set
            {
                _IsModeFolder = value;

                if (value)
                    SetSourceExample("folder");

                RaisePropertyChanged("IsModeFolder");
            }
        }
        #endregion

        #region IsModeCsv
        private bool _IsModeCsv;

        /// <summary>
        /// Is CSV-Mode selected?
        /// </summary>
        public bool IsModeCsv
        {
            get { return _IsModeCsv; }
            set
            {
                _IsModeCsv = value;

                if (value)
                    SetSourceExample("csv");

                RaisePropertyChanged("IsModeCsv");
            }
        }
        #endregion

        #region SourcePath
        private string _SourcePath = "";
        public string SourcePath
        {
            get { return _SourcePath; }
            set
            {
                _SourcePath = value;
                RaisePropertyChanged("SourcePath");
            }
        }
        #endregion

        #region SourceExample
        private string _SourceExample = "";
        public string SourceExample
        {
            get { return _SourceExample; }
            set
            {
                _SourceExample = value;
                RaisePropertyChanged("SourceExample");
            }
        }
        #endregion

        #region SelectedTarget
        private ConnectionItem _SelectedTarget;
        public ConnectionItem SelectedTarget
        {
            get { return _SelectedTarget; }
            set
            {
                if (value == null)
                {
                    _SelectedTarget = null;
                    RaisePropertyChanged("SelectedFolder");
                    return;
                }

                foreach (var conItm in FolderList)
                {
                    if (conItm.ConnectionID != value.ConnectionID)
                        continue;

                    _SelectedTarget = conItm;
                }

                RaisePropertyChanged("SelectedFolder"); 
            }
        }
        #endregion

        #region StatusText
        private string _StatusText = "";

        /// <summary>
        /// The StatusText of the current Import-Step
        /// </summary>
        public string StatusText
        {
            get { return _StatusText; }
            set
            {
                if (value == _StatusText)
                    return;

                _StatusText = value;
                RaisePropertyChanged("StatusText");
            }
        }
        #endregion

        #region StatusTitle
        private string _StatusTitle = "";

        /// <summary>
        /// The Title of the Import-Job
        /// </summary>
        public string StatusTitle
        {
            get { return _StatusTitle; }
            set
            {
                if (value == _StatusTitle)
                    return;

                _StatusTitle = value;
                RaisePropertyChanged("StatusTitle");
            }
        }
        #endregion

        #region StatusStepCurrent
        private int _StatusStepCurrent;

        /// <summary>
        /// The Number of the current step
        /// </summary>
        public int StatusStepCurrent
        {
            get { return _StatusStepCurrent; }
            set
            {
                if (value == _StatusStepCurrent)
                    return;

                _StatusStepCurrent = value;
                RaisePropertyChanged("StatusStepCurrent");
            }
        }
        #endregion

        #region StatusStepMax
        private int _StatusStepMax;

        /// <summary>
        /// The total Amount of steps
        /// </summary>
        public int StatusStepMax
        {
            get { return _StatusStepMax; }
            set
            {
                if (value == _StatusStepMax)
                    return;

                _StatusStepMax = value;
                RaisePropertyChanged("StatusStepMax");
            }
        }
        #endregion

        #region IsImportRunning
        private bool _IsImportRunning = false;

        /// <summary>
        /// Is an Import currently running?
        /// </summary>
        public bool IsImportRunning
        {
            get { return _IsImportRunning; }
            private set
            {
                if (value == _IsImportRunning)
                    return;

                _IsImportRunning = value;
                RaisePropertyChanged("IsImportRunning");
                RaisePropertyChanged("IsImportNotRunning");
            }
        }
        #endregion

        #region IsImportNotRunning
        private bool _IsImportNotRunning;

        /// <summary>
        /// Is an Import currently NOT running?
        /// </summary>
        public bool IsImportNotRunning
        {
            get { return !_IsImportRunning; }            
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
        #endregion

        #region Internal Methods
        /// <summary>
        /// Sets the Example of a mode, after the selected Mode was changed
        /// </summary>
        /// <param name="mode"></param>
        private void SetSourceExample(string mode)
        {
            switch (mode)
            {
                case "mremote":
                    SourceExample = _LangDictionary["TabImportMRemoteExample"].ToString();
                    break;
                case "folder":
                    SourceExample = _LangDictionary["TabImportFolderExample"].ToString();
                    break;
                case "csv":
                    SourceExample = _LangDictionary["TabImportCsvExample"].ToString();
                    break;
            }
        }
        #endregion

        #region Events
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

        #region RefreshConnectionList

        public delegate void RefreshConnectionListEventHandler(object sender, RoutedEventArgs e);

        public event RefreshConnectionListEventHandler RefreshConnectionList;

        protected virtual void OnRefreshConnectionList(RoutedEventArgs e)
        {
            var Handler = RefreshConnectionList;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion
        #endregion

        public void Dispose()
        {
            _LangDictionary.Clear();

            _MRemoteWorker = null;
            _FolderWorker = null;
            _CsvWorker = null;


            CmdImport.StartImport -= CmdImport_StartImport;
            CmdTabLoaded.TabLoaded -= CmdTabLoaded_TabLoaded;
            CmdBrowse.StartBrowsing -= CmdBrowse_StartBrowsing;

            CmdBrowse = null;
            CmdCancel = null;
            CmdImport = null;
            CmdTabLoaded = null;

            FolderList.Clear();
        }

    }
}
