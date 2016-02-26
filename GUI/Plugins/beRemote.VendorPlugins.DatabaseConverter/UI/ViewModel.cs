using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.VendorPlugins.DatabaseConverter.UI.Commands;


namespace beRemote.VendorPlugins.DatabaseConverter.UI
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Local Definitons
        public CmdLoadedImpl CmdLoaded { get; set; }
        public CmdStartImpl CmdStart { get; set; }
        public CmdImportImpl CmdImport { get; set; }
        #endregion

        public ViewModel()
        {
            CmdLoaded = new CmdLoadedImpl();
            CmdLoaded.Loaded += CmdLoaded_Loaded;

            CmdStart = new CmdStartImpl();
            CmdStart.StartImport += CmdStart_StartImport;

            CmdImport = new CmdImportImpl();
            CmdImport.Import += CmdImport_Import;
        }


        /// <summary>
        /// Get the User Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CmdStart_StartImport(object sender, RoutedEventArgs e)
        {
            try
            {
                //Initialize connection
                InitConnection();

                #region Get Userinformation

                //Was Connection successfull?
                if (_DbConnection.State != ConnectionState.Open)
                    return;

                //Initialize the Command
                var slCmd = new SQLiteCommand(_DbConnection);
                slCmd.CommandText = "SELECT id, name FROM user;";

                lock (slCmd)
                {
                    //Execute the command and store the result in slDataReader
                    var slDataReader = slCmd.ExecuteReader();

                    //Read the first line of the returned value                
                    while (slDataReader.Read())
                    {
                        var ret = slDataReader.GetValues();

                        var id = ret.GetValues(0)[0];
                        var name = ret.GetValues(1)[0];

                        if (UserList == null)
                            UserList = new List<User>();

                        UserList.Add(new User(Convert.ToInt32(id), EnfuseString(name), "", "", new DateTime(), 0, new DateTime(), 0, "", new DateTime(), 0, false));
                    }

                    slDataReader.Close();
                    slDataReader.Dispose();
                }

                slCmd.Dispose();
                RaisePropertyChanged("UserList");

                IsDbSelected = true;
                RaisePropertyChanged("IsDbSelected");
            }
            catch (Exception)
            {
                SourceDbPath = "Cannot read Databasefile.";
                RaisePropertyChanged("SourceDbPath");
            }

            

            #endregion
        }

        /// <summary>
        /// Import the connections of the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CmdImport_Import(object sender, RoutedEventArgs e)
        {
            ImportFolder(0, 0);
            OnRefreshConnectionList(new RoutedEventArgs());
        }

        #region Import Methods
        /// <summary>
        /// Imports a Folder and all Subfolders into the current Database
        /// </summary>
        /// <param name="id">Folder RootID of the to-import-database</param>
        /// <param name="importId">The FolderID of the current Databse, where this folder should be imported to</param>
        private void ImportFolder(long id, long importId)
        {
            //is Connection successfull?
            if (_DbConnection.State != ConnectionState.Open)
                return;

            //Initialize the Command
            var slCmd = new SQLiteCommand(_DbConnection);
            slCmd.CommandText = "SELECT id, foldername, ispublic FROM folder WHERE owner=" + SelectedUserId + " AND parent=" + id + ";";

            lock (slCmd)
            {
                //Execute the command and store the result in slDataReader
                var slDataReader = slCmd.ExecuteReader();

                //Read the first line of the returned value                
                while (slDataReader.Read())
                {
                    var ret = slDataReader.GetValues();

                    var folderId = Convert.ToInt64(ret.GetValues(0)[0]);
                    var name = ret.GetValues(1)[0];
                    var pub = (ret.GetValues(2)[0] == "1");

                    var newId = StorageCore.Core.AddFolder(EnfuseString(name), importId, pub);

                    ImportConnections(folderId, newId); //Import containing Connections
                    ImportFolder(folderId, newId); //Import containing Folders
                }

                slDataReader.Close();
                slDataReader.Dispose();
            }

            slCmd.Dispose();
        }

        /// <summary>
        /// Imports all connections from the given Source-folder
        /// </summary>
        /// <param name="sourceFolderId"></param>
        /// <param name="targetFolderId"></param>
        private void ImportConnections(long sourceFolderId, long targetFolderId)
        {
            //is Connection successfull?
            if (_DbConnection.State != ConnectionState.Open)
                return;

            //Initialize the Command
            var slCmd = new SQLiteCommand(_DbConnection);
            slCmd.CommandText = "SELECT id, host, name, description, ispublic FROM connections WHERE owner=" + SelectedUserId + " AND folder=" + sourceFolderId + ";";

            lock (slCmd)
            {
                //Execute the command and store the result in slDataReader
                var slDataReader = slCmd.ExecuteReader();

                //Read the first line of the returned value                
                while (slDataReader.Read())
                {
                    var ret = slDataReader.GetValues();

                    var conId = Convert.ToInt64(ret.GetValues(0)[0]);
                    var host = ret.GetValues(1)[0];
                    var name = ret.GetValues(2)[0];
                    var desc = ret.GetValues(3)[0];
                    var pub = ret.GetValues(4)[0] == "1";

                    var newId = StorageCore.Core.AddConnection(EnfuseString(host), EnfuseString(name), EnfuseString(desc), 0, targetFolderId, StorageCore.Core.GetUserId(), pub, 0);

                    //Import Connectionsetting
                    ImportConnectionSettings(conId, newId);

                }

                slDataReader.Close();
                slDataReader.Dispose();
            }

            slCmd.Dispose();
        }

        /// <summary>
        /// Imports all connections from the given Source-folder
        /// </summary>
        /// <param name="sourceConnectionId"></param>
        /// <param name="targetConnectionId"></param>
        private void ImportConnectionSettings(long sourceConnectionId, long targetConnectionId)
        {
            //is Connection successfull?
            if (_DbConnection.State != ConnectionState.Open)
                return;

            //Initialize the Command
            var slCmd = new SQLiteCommand(_DbConnection);
            slCmd.CommandText = "SELECT id, protocol,port FROM connectionsetting WHERE connectionid=" + sourceConnectionId + ";";

            lock (slCmd)
            {
                //Execute the command and store the result in slDataReader
                var slDataReader = slCmd.ExecuteReader();

                //Read the first line of the returned value                
                while (slDataReader.Read())
                {
                    var ret = slDataReader.GetValues();

                    var setId = Convert.ToInt64(ret.GetValues(0)[0]);
                    var prot = ret.GetValues(1)[0];
                    var port = Convert.ToInt32(ret.GetValues(2)[0]);

                    var newId = StorageCore.Core.AddConnectionSetting(targetConnectionId, prot, port);

                    //Import ConnectionOptions
                    ImportConnectionOptions(setId, newId);

                }

                slDataReader.Close();
                slDataReader.Dispose();
            }

            slCmd.Dispose();
        }

        /// <summary>
        /// Imports all connections from the given Source-folder
        /// </summary>
        /// <param name="sourceConnectionSetId"></param>
        /// <param name="targetConnectionSetId"></param>
        private void ImportConnectionOptions(long sourceConnectionSetId, long targetConnectionSetId)
        {
            //is Connection successfull?
            if (_DbConnection.State != ConnectionState.Open)
                return;

            //Initialize the Command
            var slCmd = new SQLiteCommand(_DbConnection);
            slCmd.CommandText = "SELECT optionname, value FROM connectionoptions WHERE connectionsettingid=" + sourceConnectionSetId + ";";

            lock (slCmd)
            {
                //Execute the command and store the result in slDataReader
                var slDataReader = slCmd.ExecuteReader();

                //Read the first line of the returned value                
                while (slDataReader.Read())
                {
                    //var opti = slDataReader.GetValue(0).ToString();
                    //var val = slDataReader.GetValue(1);
                    var ret = slDataReader.GetValues();

                    var opti = ret.GetValues(0)[0];
                    var val = ret.GetValues(1)[0];

                    var cleanVal = Helper.DeserializeBase64(EnfuseString(val));

                    StorageCore.Core.ModifyConnectionOption(cleanVal, opti, targetConnectionSetId);
                }

                slDataReader.Close();
                slDataReader.Dispose();
            }

            slCmd.Dispose();
        }
        #endregion
        
        #region Helper
        /// <summary>
        /// Replaces Special-Signs in strings, that had been removed
        /// </summary>
        /// <param name="toEnfuse">Defused String</param>
        /// <returns>Original String</returns>
        public string EnfuseString(string toEnfuse)
        {
            //Check if a Specialsign is containing; if not: return original
            if (!toEnfuse.Contains("#"))
                return (toEnfuse);

            string enfusedString = "";

            for (int i = 0; i < toEnfuse.Length; i++)
            {
                //If a converted Specialsign follows
                if (toEnfuse.Length > i + 4 && toEnfuse.Substring(i, 2) == "-#")
                {
                    //get the sign:
                    string sign = toEnfuse.Substring(i + 2).Split(new[] { '#' }, 2)[0];

                    i += 2 + sign.Length; //2 => for the -# and the end #

                    enfusedString += HexStringToString(sign);
                }
                else //No Specialsign
                {
                    enfusedString += toEnfuse.Substring(i, 1);
                }
            }

            return (enfusedString);
        }

        /// <summary>
        /// Converts a Hex to Ascii
        /// </summary>
        /// <param name="hexString">Hex String</param>
        /// <returns>Ascii-String</returns>
        private string HexStringToString(string hexString)
        {
            string result = "";
            int count = hexString.Length / 2;
            int s;

            for (s = 0; s < count; s++)
            {
                string sign = hexString.Substring(s * 2, 2);
                result += (char)Convert.ToUInt16(sign, 16);
            }

            return result;
        }

        #endregion

        void CmdLoaded_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        #region SQLite-Handling
        /// <summary>
        /// The Connection to the Database
        /// </summary>
        SQLiteConnection _DbConnection;


        /// <summary>
        /// Initialisation of the Database-Connection
        /// </summary>
        public void InitConnection()
        {
            var connectionString = "Data Source=" + SourceDbPath + ";"; //Password=beRemote2012;";

            Logger.Log(LogEntryType.Verbose, "Connecting to Database using ConnectionString: " + connectionString.Replace("beRemote2012", "*********"), "DatabaseConverter");

            //Set Connection Parameter
            try
            {
                _DbConnection = new SQLiteConnection();
                _DbConnection.StateChange += DbConnectionStateChange;
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Unable to connect to to SqLite Database. This usally happens, if your are using the wrong architecture-files for your OS (i.e. 32Bit instead of 64Bit). Please download the Downloadpack that fits to your environment!", ea);
            }
            _DbConnection.ConnectionString = connectionString;
            _DbConnection.Open();

            Logger.Log(LogEntryType.Verbose, "Connected...", "DatabaseConverter");
        }

        void DbConnectionStateChange(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState != ConnectionState.Open &&
                _DbConnection.ConnectionString.Length > 0)
                _DbConnection.Open();
        }
        #endregion
        
        #region Properties
        /// <summary>
        /// The DB to import
        /// </summary>
        public string SourceDbPath { get; set; }

        /// <summary>
        /// The Selected UserId
        /// </summary>
        public int SelectedUserId { get; set; }

        /// <summary>
        /// Available Users in the beRemote Database
        /// </summary>
        public List<User> UserList { get; set; }

        /// <summary>
        /// Is a database Selected?
        /// </summary>
        public bool IsDbSelected { get; set; }
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

        #region events
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
            CmdLoaded.Loaded -= CmdLoaded_Loaded;
            CmdStart.StartImport -= CmdStart_StartImport;
            CmdImport.Import -= CmdImport_Import;

            CmdLoaded = null;
            CmdStart = null;
            CmdImport = null;
            if (UserList != null)
                UserList.Clear();
        }
    }
}
