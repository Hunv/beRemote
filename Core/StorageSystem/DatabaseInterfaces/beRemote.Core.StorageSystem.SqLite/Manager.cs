using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using System.Runtime.Serialization;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.Definitions.Classes.UpdateDatabase;
using beRemote.Core.Definitions.Enums.Filter;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.Core.StorageSystem.SqLite
{
    /// <summary>
    /// Manages the in- and output of the SqLite-interface
    /// </summary>
    class SqLiteMgr : StorageBase.IDbPlugin
    {
        /// <summary>
        /// Provides the Communicator-Class for Database-Communication
        /// </summary>
        private SqLiteCom _Com = new SqLiteCom();

        /// <summary>
        /// The Logger-Context. This will shown in the log, if messages were send to the log. Don't change this.
        /// </summary>
        private String _loggerContext = "StorageSystem";

        /// <summary>
        /// The Name of this Database-Interface
        /// </summary>
        private String _WrapperName = "SQLite";

        /// <summary>
        /// Initialisations on Load
        /// </summary>
        public bool InitPlugin()
        {
            try
            {
                Logger.Log(LogEntryType.Verbose, "Trying to Initialize database-connection...");

                //Initialise Databaseconnection
                _Com.InitConnection();
                
                //Sets the UserId or creates one, if not existing
                _Com.SetUserId();

                Logger.Log(LogEntryType.Verbose, "Initialize database-connection completed");

                return (true);
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "You need the Microsoft Visual C++ 2010 Redistributable Package to run a SQLite Database." + Environment.NewLine +
                    "x86: http://www.microsoft.com/en-us/download/details.aspx?id=5555" + Environment.NewLine +
                    "x64: http://www.microsoft.com/en-us/download/details.aspx?id=14632" + Environment.NewLine +
                    "ia64: http://www.microsoft.com/en-us/download/details.aspx?id=2054", ea);
                
                return (false);
            }
        }

        /// <summary>
        /// Sets a (new) Database GUID for this database
        /// </summary>
        /// <param name="overwriteExistingGuid">Should the GUID recreated, if it already exists?</param>
        /// <returns></returns>
        public void InitDatabaseGuid(bool overwriteExistingGuid)
        {
            _Com.CreateDatabaseGuid(overwriteExistingGuid);
        }

        /// <summary>
        /// Get the Database GUID
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseGuid()
        {
            return (_Com.GetDatabaseGuid());
        }

        /// <summary>
        /// Gets the Name of the Strage-Wrapper Name
        /// </summary>
        /// <returns>The name of the Wrapper</returns>
        public string GetWrapperName()
        {
            Logger.Log(LogEntryType.Info, "Database Wrapper Name: " + _WrapperName, _loggerContext);
            return (_WrapperName);
        }

        /// <summary>
        /// Gets the version of the Database
        /// </summary>
        /// <returns>Version of Databasecontent</returns>
        public Int32 GetDatabaseVersion()
        {
            Logger.Log(LogEntryType.Info, "Database Version: " + _Com.GetDatabaseVersion(), _loggerContext);
            return (_Com.GetDatabaseVersion());
        }


        /// <summary>
        /// Get all DatabaseTables
        /// </summary>
        /// <returns>All Tables of the Database</returns>
        public DataTable GetDatabaseTables()
        {
            if (GetUserSuperadmin()) //This is only allowed to superadmins!
                return (_Com.GetDatabaseTables());
            
            return (null);
        }

        /// <summary>
        /// Gets the complete content of a Databasetable
        /// </summary>
        /// <param name="table">Tablename to query</param>
        /// <returns>Complete content of the table</returns>
        public DataTable GetDatabaseTableContent(string table)
        {
            if (GetUserSuperadmin()) //This is only allowed to superadmins!
                return (_Com.GetDatabaseTableContent(table));
            
            return (null);
        }

        /// <summary>
        /// Modifies a Cellvalue (used for Database-Browser)
        /// </summary>
        /// <param name="table">Tablename</param>
        /// <param name="column">Columnname</param>
        /// <param name="value">Value</param>
        /// <param name="isString">Is the Value a String (add ' ???)</param>
        /// <param name="currentDataRow">A Dictionary, that contains all current Row-Values. Key=columnname, value=cellvalue (with ' if nessessary)</param>
        public void ModifyDatabaseTableContent(string table, string column, string value, bool isString, Dictionary<string, string> currentDataRow)
        {
            if (GetUserSuperadmin()) //This is only allowed to superadmins!
            {
                string whereClause = "";

                //Build the Where-Clause for the SQL-Query
                foreach (KeyValuePair<string, string> kvp in currentDataRow)
                {
                    whereClause += kvp.Key + "=" + kvp.Value + ", ";
                }

                //Remove last ", "
                whereClause = whereClause.Substring(0, whereClause.Length - 2);

                //Query the Cell-Changes in the given table and column
                _Com.ModifyDatabaseTableContent(table, column, value, isString, whereClause);
            }
        }




        /// <summary>
        /// Gets the UserId of the current User
        /// </summary>
        /// <returns>The UserId</returns>
        public int GetUserId()
        {
            return (_Com.GetUserId());
        }

        /// <summary>
        /// Gets the Id of the User "username"
        /// </summary>
        /// <param name="username">The Username of the User the ID is queried</param>
        /// <returns>The UserId of the queried user</returns>
        public int GetUserId(string username)
        {
            username = DefuseString(username); //Defusing string to prohibit SQL-Injections

            return (_Com.GetUserId(username.ToLower())); //The username is not case-sensitive!
        }

        /// <summary>
        /// Returns a Dictionary with UserId as Key and Name as Value
        /// </summary>
        /// <returns>Returns a Dictionary with UserId as Key and Name as Value</returns>
        public Dictionary<int, string> GetUserList()
        {
            //Get the UserList as a DataTable
            DataTable dt = _Com.GetUserList();

            //Initialisation of the return-variable
            Dictionary<int, string> ret = new Dictionary<int, string>();

            //Convert each Row in the DataTable to an Entry in the Dictionary
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ret.Add(Convert.ToInt32(dt.Rows[i].ItemArray[0]),
                    EnfuseString(dt.Rows[i].ItemArray[1].ToString())); //Create the original string of the Name by using enfuseString
            }

            return (ret);
        }

        /// <summary>
        /// Changes the UserId, if the password is correct
        /// </summary>
        /// <param name="username">Username (i.e. Domain\Username)</param>
        /// <param name="hashedPassword">hashed Password</param>
        /// <returns>The ID of the User</returns>
        public int ChangeUser(string username, byte[] hashedPassword)
        {
            username = DefuseString(username).ToLower(); //Defusing string to prohibit SQL-Injections

            //If Password is correct, Change ID in the Cache and return it
            if (_Com.CheckUserPassword(_Com.GetUserId(username), hashedPassword))
                return (_Com.ChangeUser(username));
            
            return (0);
        }

        /// <summary>
        /// Adds a new User
        /// </summary>
        /// <param name="username">Username (i.e. Domain\Username)</param>
        /// <param name="displayname">Displayname in beRemote (i.e. Nickname)</param>
        /// <param name="hashedPassword">Hashed and salted Password</param>
        /// <returns>UserId of the new User</returns>
        public long AddUser(string username, string displayname, byte[] hashedPassword)
        {
            username = DefuseString(username).ToLower(); //Defusing string to prohibit SQL-Injections; save user as lower-letters-only
            displayname = DefuseString(displayname); //Defusing string to prohibit SQL-Injections

            //Add the new user and store the ID
            long newUserId = _Com.AddUser(username, displayname, hashedPassword);

            //Add a private Connectionfolder for the new User
            long prvFolder = AddFolder("Private", 0, (int)newUserId, false);

            //Defines Defaultfolder for the User
            ModifyUserDefaultFolder((int)newUserId, prvFolder);

            return (newUserId);
        }

        /// <summary>
        /// Changes the Default Folder for a User
        /// </summary>
        /// <param name="userId">ID of the User</param>
        /// <param name="newDefaultFolder">ID of the new Default folder</param>
        public void ModifyUserDefaultFolder(long userId, long newDefaultFolder)
        {
            _Com.ModifyUserDefaultFolder(userId, newDefaultFolder);
        }

        /// <summary>
        /// Gets the Default Folder ID for the CURRENT UserId
        /// </summary>        
        /// <returns>FolderID of the Defaultfolder</returns>
        public int GetUserDefaultFolder()
        {
            return(GetUserDefaultFolder(GetUserId()));
        }

        /// <summary>
        /// Gets the Default Folder ID for the given UserId
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>FolderID of the Defaultfolder</returns>
        public int GetUserDefaultFolder(long userId)
        {
            return(_Com.GetUserDefaultFolder(userId));
        }

        /// <summary>        
        /// Modifies a User; identified by username        
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="name">The new Displayname (i.e. Nickname)</param>
        /// <param name="hashedPassword">The new hashed Password</param>
        public void ModifyUser(string username, string name, byte[] hashedPassword)
        {
            username = DefuseString(username).ToLower(); //Defusing string to prohibit SQL-Injections
            name = DefuseString(name); //Defusing string to prohibit SQL-Injections

            _Com.ModifyUser(_Com.GetUserId(username), name, hashedPassword);
        }

        /// <summary>
        /// Modifes a User, identified by ID
        /// </summary>
        /// <param name="userId">The UserId of the User</param>
        /// <param name="name">The new Displayname (i.e. Nickname)</param>
        /// <param name="hashedPassword">The new hashed Password</param>
        /// <param name="autologin">Autologin Enabled?</param>
        public void ModifyUser(long userId, string name, byte[] hashedPassword)
        {
            name = DefuseString(name); //Defusing string to prohibit SQL-Injections

            _Com.ModifyUser(userId, name, hashedPassword);
        }

        /// <summary>
        /// Modifies the display- and username
        /// </summary>
        /// <param name="userId">UserID to modifiy</param>
        /// <param name="name">New Displayname</param>
        /// <param name="username">New Username</param>        
        public void ModifyUser(long userId, string name, string username)
        {
            name = DefuseString(name); //Defusing string to prohibit SQL-Injections
            username = DefuseString(username).ToLower(); //Defusing string to prohibit SQL-Injections

            _Com.ModifyUser(userId, name, username);
        }

        /// <summary>
        /// Sets if a user is superadmin
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="isSuperadmin">Is Superadmin?</param>
        public void ModifyUserSuperadmin(long userId, bool isSuperadmin)
        {
            if (isSuperadmin) //Only allowed for Superadmins!
                _Com.ModifyUserSuperadmin(userId, true);
        }

        /// <summary>
        /// Gets the Usersettings of the current User
        /// </summary>
        /// <returns>All Settings; Column as Key, Value as Value</returns>
        public User GetUserSettings()
        {            
            return (GetUserSettings(GetUserId()));
        }

        /// <summary>
        /// Gets Usersettings by Username except PasswordHash and IsSuperadmin
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>All Settings; Column as Key, Value as Value</returns>
        public User GetUserSettings(string username)
        {
            username = DefuseString(username); //Defusing string to prohibit SQL-Injections

            return (GetUserSettings(GetUserId(username.ToLower())));
        }

        /// <summary>
        /// Gets Usersettings by userId except PasswordHash and IsSuperadmin
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>All Settings; Column as Key, Value as Value</returns>
        public User GetUserSettings(long userId)
        {
            //If user is not logged in, handle userId 0
            if (userId == 0)
            {
                return (new User());
            }

            //Get Usersettings
            Dictionary<string, object> userSettings = _Com.GetUserSettings(userId);

            //Catch Error. This happens if Plugins are not completely loaded when this is called
            while (userSettings.Count == 0)
            {
                Logger.Log(LogEntryType.Warning, "Startup was too fast. Wait a moment and try again");
                System.Threading.Thread.Sleep(100);
                userSettings = _Com.GetUserSettings(userId);
            }

            //Create the User-Variable with queried information
            return (new User(
                Convert.ToInt32(userSettings["id"]),
                EnfuseString(userSettings["name"].ToString()),
                EnfuseString(userSettings["winname"].ToString()),
                userSettings["lastmachine"].ToString(),
                Convert.ToDateTime(userSettings["lastlogin"]),
                Convert.ToInt32(userSettings["logincount"]),
                Convert.ToDateTime(userSettings["lastlogout"]),
                Convert.ToInt32(userSettings["defaultfolder"]),
                userSettings["defaultprotocol"].ToString(),
                Convert.ToDateTime(userSettings["heartbeat"]),
                Convert.ToInt32(userSettings["updatemode"]),
                Convert.ToBoolean(userSettings["deletequickconnect"])));

        }
        
        /// <summary>
        /// Sets a usersetting
        /// </summary>
        /// <param name="settingname"></param>
        /// <param name="value"></param>
        public void SetUserSettings(string settingname, object value)
        {
            settingname = settingname.ToLower();

            //Don't allow to change system-properties this way
            if (settingname == "id" ||
                settingname == "name" ||
                settingname == "winname" ||
                settingname == "password" ||
                settingname.StartsWith("salt")||
                settingname.StartsWith("proxy") ||
                settingname == "lastlogin" ||
                settingname == "lastlogout" ||
                settingname == "logincount" ||
                settingname == "heartbeat" ||
                settingname == "lastmachine")
                return;

            var strValue = "";

            if (value is string)
            {
                strValue = "\"" + DefuseString(value.ToString()) + "\"";
            }
            else if (value is DateTime)
            {
                strValue = "\"";
                strValue += ((DateTime) value).Year + ToString() + "-";
                strValue += ((DateTime) value).Month + ToString() + "-";
                strValue += ((DateTime) value).Day + ToString() + " ";
                strValue += ((DateTime) value).Hour + ToString() + ":";
                strValue += ((DateTime) value).Minute + ToString() + ":";
                strValue += ((DateTime) value).Second + ToString() + "\"";
            }
            else if (value is bool)
            {
                strValue = (bool)value ? "1" : "0";
            }
            else if (value is byte[])
                throw new Exception("This is not possible.");
            else
                strValue = DefuseString(value.ToString());

            _Com.SetUserSetting(GetUserId(), settingname, strValue);
        }

        /// <summary>
        /// Modifies the Password of a User
        /// </summary>
        /// <param name="userId">The UserId</param>
        /// <param name="hashedPassword">The hashed Password</param>
        public void ModifyUserPassword(long userId, byte[] hashedPassword)
        {
            _Com.ModifyUserPassword(userId, hashedPassword);
        }

        /// <summary>
        /// Sets the Salt of the current User
        /// </summary>
        /// <param name="hash">The hash of the new salt</param>
        public void SetUserSalt1(byte[] hash)
        {
            SetUserSalt1(GetUserId(), hash);
        }

        /// <summary>
        /// Sets the Salt of a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="hash">Hash of the Salt</param>
        public void SetUserSalt1(long userId, byte[] hash)
        {
            _Com.SetUserSalt1(userId, hash);
        }

        /// <summary>
        /// Gets the Salt of the current User
        /// </summary>
        /// <returns></returns>
        public byte[] GetUserSalt1()
        {
            return (GetUserSalt1(GetUserId()));
        }

        /// <summary>
        /// Gets the UserSalt of the given User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public byte[] GetUserSalt1(long userId)
        {
            return(_Com.GetUserSalt1(userId));
        }

        /// <summary>
        /// Sets the Salt of the current User
        /// </summary>
        /// <param name="hash">The hash of the new salt</param>
        public void SetUserSalt2(byte[] hash)
        {
            SetUserSalt2(GetUserId(), hash);
        }

        /// <summary>
        /// Sets the Salt of a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="hash">Hash of the Salt</param>
        public void SetUserSalt2(long userId, byte[] hash)
        {
            _Com.SetUserSalt2(userId, hash);
        }

        /// <summary>
        /// Gets the Salt of the current User
        /// </summary>
        /// <returns></returns>
        public byte[] GetUserSalt2()
        {
            return (GetUserSalt2(GetUserId()));
        }

        /// <summary>
        /// Gets the UserSalt of the given User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public byte[] GetUserSalt2(long userId)
        {
            return (_Com.GetUserSalt2(userId));
        }

        /// <summary>
        /// Sets the Salt of the current User
        /// </summary>
        /// <param name="hash">The hash of the new salt</param>
        public void SetUserSalt3(byte[] hash)
        {
            SetUserSalt3(GetUserId(), hash);
        }

        /// <summary>
        /// Sets the Salt of a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="hash">Hash of the Salt</param>
        public void SetUserSalt3(long userId, byte[] hash)
        {
            _Com.SetUserSalt3(userId, hash);
        }

        /// <summary>
        /// Gets the Salt of the current User
        /// </summary>
        /// <returns></returns>
        public byte[] GetUserSalt3()
        {
            return (GetUserSalt3(GetUserId()));
        }

        /// <summary>
        /// Gets the UserSalt of the given User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public byte[] GetUserSalt3(long userId)
        {
            return (_Com.GetUserSalt3(userId));
        }

        /// <summary>
        /// Get if the current user is Superadmin
        /// </summary>        
        /// <returns>yes/no (true/false)</returns>
        public bool GetUserSuperadmin()
        {
            return (_Com.GetUserSuperadmin(_Com.GetUserId()));
        }

        /// <summary>
        /// Get if a specified user is Superadmin
        /// </summary>
        /// <param name="userId">UserId to check</param>
        /// <returns>yes/no (true/false)</returns>
        public bool GetUserSuperadmin(long userId)
        {
            return (_Com.GetUserSuperadmin(userId));
        }

        /// <summary>
        /// Increases the LoginCount of the current user by one
        /// </summary>        
        public void UserLoggedIn()
        {
            UserLoggedIn(GetUserId());            
        }

        /// <summary>
        /// Increases the LoginCount of userId by one
        /// </summary>
        /// <param name="userId">UserId</param>
        public void UserLoggedIn(long userId)
        {
            _Com.IncreaseUserLoginCount(userId);
            _Com.ModifyUserLoginparameter(userId, Environment.MachineName);
        }

        /// <summary>
        /// Validates the Password for the current User
        /// </summary>        
        /// <param name="hashedPassword">hashed Password</param>
        /// <returns>matches/not matches (true/false)</returns>
        public bool CheckUserPassword(byte[] hashedPassword)
        {
            return (CheckUserPassword(_Com.GetUserId(), hashedPassword));
        }

        /// <summary>
        /// Validates the Password
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="hashedPassword">hashed Password</param>
        /// <returns>matches/not matches (true/false)</returns>
        public bool CheckUserPassword(long userId, byte[] hashedPassword)
        {
            return (_Com.CheckUserPassword(userId,hashedPassword));
        }

        /// <summary>
        /// Modfies the selected default protocol of a user
        /// </summary>        
        /// <param name="newDefaultProtocol">Protocol-Identifier</param>
        public void ModifyUserDefaultProtocol(string newDefaultProtocol)
        {
            ModifyUserDefaultProtocol(GetUserId(), newDefaultProtocol);
        }

        /// <summary>
        /// Modfies the selected default protocol of a user
        /// </summary>
        /// <param name="userid">ID of the user</param>
        /// <param name="newDefaultProtocol">Protocol-Identifier</param>
        public void ModifyUserDefaultProtocol(long userid, string newDefaultProtocol)
        {
            _Com.ModifyUserDefaultProtocol(userid, newDefaultProtocol);
        }

        /// <summary>
        /// Gets the default Protocol of the current user
        /// </summary>        
        /// <returns>The internal name of the protocol</returns>
        public string GetUserDefaultProtocol()
        {
            return (GetUserDefaultProtocol(GetUserId()));
        }

        /// <summary>
        /// Gets the default Protocol of a user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>The internal name of the protocol</returns>
        public string GetUserDefaultProtocol(long userId)
        {
            return (_Com.GetUserDefaultProtocol(userId));
        }

        /// <summary>
        /// Updates the Userheartbeat to the current timestamp
        /// </summary>        
        public void UpdateUserHeartbeat()
        {
            _Com.UpdateUserHeartbeat(GetUserId());
        }

        /// <summary>
        /// Sets the lastlogout-value to the current timestamp
        /// </summary>
        public void LogoutUser()
        {
            _Com.LogoutUser(GetUserId());
        }

        /// <summary>
        /// Modifies the Updatemode. 0=Stable, 1=Nightly
        /// </summary>
        /// <param name="updatemode">The Mode-ID</param>
        public void ModifyUserUpdatemode(int updatemode)
        {
            ModifyUserUpdatemode(GetUserId(), updatemode);
        }

        /// <summary>
        /// Modifies the Updatemode. 0=Stable, 1=Nightly
        /// </summary>
        /// <param name="userId">ID user to modify</param>
        /// <param name="updatemode">The Mode-ID</param>
        public void ModifyUserUpdatemode(long userId, int updatemode)
        {
            _Com.ModifyUserUpdatemode(userId, updatemode);
        }

        /// <summary>
        /// Gets a list of usernames, that were online in the heartbeatinterval and not logged out in the heartbeat interval
        /// </summary>
        /// <returns>A List of all logged in Usernames</returns>
        public List<string> GetUsersOnline()
        {
            //Get Online Users
            DataTable dT = _Com.GetUsersOnline();
            List<string> ret = new List<string>();

            //Enfuse the Strings to make usernames readable
            for (int i = 0; i < dT.Rows.Count; i++)
            {
                ret.Add(dT.Rows[i].ItemArray[1].ToString() + " (" + EnfuseString(dT.Rows[i].ItemArray[0].ToString()) + ")");
            }

            return (ret);
        }

        /// <summary>
        /// Get the current users visual settings
        /// </summary>        
        /// <returns>The Visualsettings of the User</returns>
        public UserVisuals GetUserVisuals()
        {
            return (GetUserVisuals(GetUserId()));
        }

        /// <summary>
        /// Get the users visual settings
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>The Visualsettings of the User</returns>
        public UserVisuals GetUserVisuals(long userId)
        {
            //Get Settings as a Datatable
            DataTable dT = _Com.GetUserVisuals(userId);

            if (dT.Rows.Count > 0)
            {
                //Convert Datatable to a UserVisuals-Object
                return (new UserVisuals(
                    Convert.ToInt32(dT.Rows[0]["mainwindowx"]),
                    Convert.ToInt32(dT.Rows[0]["mainwindowy"]),
                    Convert.ToBoolean(dT.Rows[0]["mainwindowmax"]),
                    Convert.ToInt32(dT.Rows[0]["mainwindowwidth"]),
                    Convert.ToInt32(dT.Rows[0]["mainwindowheight"]),
                    dT.Rows[0]["ribbonexpanded"].ToString(),
                    dT.Rows[0]["expandednodes"].ToString(),
                    Convert.ToInt32(dT.Rows[0]["statusbarsetting"]),
                    dT.Rows[0]["favorites"].ToString(),
                    dT.Rows[0]["gridlayout"].ToString(),
                    dT.Rows[0]["ribbonqat"].ToString()
                    ));
            }
            
            return (null);
        }

        /// <summary>
        /// Set the UserVisuals-Parameter of the current User
        /// </summary>        
        /// <param name="values">Dictionary with visual settings</param>
        public void SetUserVisual(Dictionary<string, object> values)
        {
            SetUserVisual(GetUserId(), values);
        }

        /// <summary>
        /// Set the UserVisuals-Parameter
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="values">Dictionary with visual settings</param>
        public void SetUserVisual(long userId, Dictionary<string, object> values)
        {
            _Com.SetUserVisual(userId, values);
        }



        /// <summary>
        /// Gets a Setting from the settings-Table
        /// </summary>
        /// <param name="setting">Name of the setting</param>
        /// <returns>The containing value to the setting</returns>
        public string GetSetting(string setting)
        {
            string id = _Com.GetSetting(setting);
            return(id);
        }

        /// <summary>
        /// Sets a global Setting from the settings-Table
        /// </summary>        
        /// <returns>The containing value to the setting</returns>
        public void SetSetting(string setting, string value)
        {
            if (GetUserSuperadmin()) //Only allowed for Superadmins!
            {
                if (_Com.GetSetting(setting) != "")
                    _Com.SetSetting(setting, value);
                else
                    _Com.AddSetting(setting, value);
            }
        }

        /// <summary>
        /// Sets the SingleUserMode. Can only be triggered once!
        /// </summary>
        /// <param name="enable">Enable SingleUsermode?</param>
        /// <param name="parameter">Parameters, if SingleUserMode is enabled</param>
        public void SetSingleUserMode(bool enable, string parameter)
        {
            if (_Com.GetSetting("singleusermode") != "" && !GetUserSuperadmin())
                return;

            if (enable)
            {
                _Com.AddSetting("singleusermode", "1");
                _Com.AddSetting("singleuserpara", parameter);
            }
            else
            {
                _Com.AddSetting("singleusermode", "0");
            }

        }


        /// <summary>
        /// Adds or Updates a Setting of a Plugin
        /// </summary>
        /// <param name="settingvalue">The new value of the setting</param>
        /// <param name="settingname">The name of the setting</param>
        /// <param name="connectionSettingId">The ID of the Connectionsetting</param>
        /// <returns>The new ConnectionOptionId (if added) or 0 (if updated)</returns>
        public long ModifyConnectionOption(object settingvalue, string settingname, long connectionSettingId)
        {
            string strSettingvalue = Helper.SerializeBase64(settingvalue);            

            strSettingvalue = DefuseString(strSettingvalue); //Defusing string to prohibit SQL-Injections

            if (GetConnectionOptionExists(settingname, connectionSettingId))
            {
                //Update
                _Com.UpdateConnectionOption(strSettingvalue, settingname, connectionSettingId);
            }
            else
            { 
                //Add
                return (_Com.AddConnectionOption(strSettingvalue, settingname, connectionSettingId));
            }
            return (0);
        }

        /// <summary>
        /// Check if a Setting exists
        /// </summary>
        /// <param name="settingname">The Settingname</param>
        /// <param name="connectionSettingId">The ConnectionSettingId</param>
        /// <returns>yes/no (true/false)</returns>
        private bool GetConnectionOptionExists(string settingname, long connectionSettingId)
        {
            return (_Com.GetConnectionOptionExists(settingname, connectionSettingId));
        }

        /// <summary>
        /// Gets the public or private setting, where the ConnectionSettingId is given
        /// </summary>
        /// <param name="connectionSettingId">The ConnectionID where the Settingname belongs to</param>
        /// <param name="settingname">The Settingname to get</param>
        /// <returns>the Setting, if exists; if not exists NULL will be returned</returns>
        public object GetConnectionOption(long connectionSettingId, string settingname)
        {
            string ReturnValue = _Com.GetConnectionOption(connectionSettingId, settingname);

            if (ReturnValue == "")
                return (null);

            return (Helper.DeserializeBase64(EnfuseString(ReturnValue)));
        }

        /// <summary>
        /// Gets all Options related to this ConnectionSetting
        /// </summary>
        /// <param name="connectionSettingId">The ID of the ConnectionSetting</param>
        /// <returns>All Options related to the ConnectionSettingId</returns>
        public List<ConnectionProtocolOption> GetConnectionOptions(long connectionSettingId)
        {            
            //Get the Settings
            DataTable options = _Com.GetConnectionOptions(connectionSettingId);

            //Convert the Datatable to the List<ConnectionOption>
            List<ConnectionProtocolOption> ReturnValue = new List<ConnectionProtocolOption>();
            for (int i = 0; i < options.Rows.Count; i++)
            {
                ConnectionProtocolOption newOption = new ConnectionProtocolOption(Convert.ToInt32(options.Rows[i]["id"]),
                    Convert.ToInt32(options.Rows[i]["connectionsettingid"]),
                    options.Rows[i]["optionname"].ToString(),
                    Helper.DeserializeBase64(EnfuseString(options.Rows[i]["value"].ToString())));

                ReturnValue.Add(newOption);
            }
            return (ReturnValue);
        }

        /// <summary>
        /// Deletes a ConnectionOption
        /// </summary>
        /// <param name="optionId">ID to Delete</param>
        public void DeleteConnectionOption(long optionId)
        {            
            //Delete ConnectionOption
            _Com.DeleteConnectionOption(optionId);
        }



        /// <summary>
        /// Adds a new ConnectionSetting
        /// </summary>
        /// <param name="connectionId">The ID of the Connection</param>
        /// <param name="protocol">Protocolname (Pluginname)</param>
        /// <param name="port">The Port where the Protocol connects to</param>
        /// <returns>the Id of the new ConnectionSetting</returns>
        public long AddConnectionSetting(long connectionId, string protocol, int port)
        {
            return (AddConnectionSetting(connectionId, protocol, port, 0));
        }

        /// <summary>
        /// Adds a new ConnectionSetting
        /// </summary>
        /// <param name="connectionId">The ID of the Connection</param>
        /// <param name="protocol">Protocolname (Pluginname)</param>
        /// <param name="port">The Port where the Protocol connects to</param>
        /// <param name="credentialId">The ID of the used Credential</param>
        /// <returns>the Id of the new ConnectionSetting</returns>
        public long AddConnectionSetting(long connectionId, string protocol, int port, int credentialId)
        {
            return(_Com.AddConnectionSetting(connectionId, protocol, port, credentialId));
        }

        /// <summary>
        /// Sets the ConnectionSetting; Every Parameter will be set except the ID - this is the identifier
        /// </summary>
        /// <param name="id">The ConnectionID that should get the new settings</param>        
        /// <param name="port">The Port where the Plugin should connect to</param>
        public void ModifyConnectionSetting(long id, int port)
        {
            _Com.ModifyConnectionSetting(id, port);
        }

        /// <summary>
        /// Sets the ConnectionSetting; Every Parameter will be set except the ID - this is the identifier
        /// </summary>
        /// <param name="id">The ConnectionID that should get the new settings</param>        
        /// <param name="port">The Port where the Plugin should connect to</param>
        /// <param name="credentialId">ID of the used Credential</param>
        public void ModifyConnectionSetting(long id, int port, int credentialId)
        {
            _Com.ModifyConnectionSetting(id, port, credentialId);
        }

        /// <summary>
        /// Removes a ConnectionSettingCredential-Value
        /// </summary>
        public void DeleteConnectionSettingCredential(long connectionSetting)
        {
            _Com.DeleteConnectionSettingCredential(connectionSetting);
        }

        /// <summary>
        /// Gets a ConnectionSetting
        /// </summary>
        /// <param name="id">The Id of the Connectionsetting</param>
        /// <returns>The Setting with id of a Connection</returns>
        public ConnectionProtocol GetConnectionSetting(long id)
        {
            Dictionary<string, object> ConnectionSet = _Com.GetConnectionSetting(id);
            if (ConnectionSet.Count > 0)
            {
                //Convert Dictionary to ConnectionProtocol
                return(new ConnectionProtocol(Convert.ToInt64(ConnectionSet["id"]),
                                            Convert.ToInt64(ConnectionSet["connectionid"]),
                                            ConnectionSet["protocol"].ToString(),
                                            Convert.ToInt32(ConnectionSet["port"]),
                                            Convert.ToInt32(ConnectionSet["credentialid"])));
            }
            
            return (null);
        }
        
        /// <summary>
        /// Gets all Settings(Plugins) for a Connection
        /// </summary>
        /// <param name="connectionId">The ID of the Connection</param>
        /// <returns>List of all ConnectionSettings releated to the ConnectionId</returns>
        public List<ConnectionProtocol> GetConnectionSettings(long connectionId)
        {
            DataTable connectionSet = _Com.GetConnectionSettings(connectionId);

            //Convert the Datatable to the List<ConnectionOption>
            List<ConnectionProtocol> ReturnValue = new List<ConnectionProtocol>();

            for (int i = 0; i < connectionSet.Rows.Count; i++)
            {
                ConnectionProtocol newOption = new ConnectionProtocol(Convert.ToInt64(connectionSet.Rows[i]["id"]),
                    Convert.ToInt64(connectionSet.Rows[i]["connectionid"]),
                    connectionSet.Rows[i]["protocol"].ToString(),
                    Convert.ToInt32(connectionSet.Rows[i]["port"]),
                    Convert.ToInt32(connectionSet.Rows[i]["credentialid"]));

                ReturnValue.Add(newOption);
            }
            return (ReturnValue);
        }

        /// <summary>
        /// Gets all Settings(Plugins)
        /// </summary>
        /// <returns>List of all ConnectionSettings</returns>
        public List<ConnectionProtocol> GetConnectionSettings()
        {
            DataTable connectionSet = _Com.GetConnectionSettings();

            //Convert the Datatable to the List<ConnectionOption>
            List<ConnectionProtocol> ReturnValue = new List<ConnectionProtocol>();
            for (int i = 0; i < connectionSet.Rows.Count; i++)
            {
                ConnectionProtocol newOption = new ConnectionProtocol(Convert.ToInt64(connectionSet.Rows[i]["id"]),
                    Convert.ToInt64(connectionSet.Rows[i]["connectionid"]),
                    connectionSet.Rows[i]["protocol"].ToString(),
                    Convert.ToInt32(connectionSet.Rows[i]["port"]),
                    Convert.ToInt32(connectionSet.Rows[i]["credentialid"]));

                ReturnValue.Add(newOption);
            }
            return (ReturnValue);
        }

        /// <summary>
        /// Deletes a ConnectionSetting and all containing ConnectionOptions
        /// </summary>
        /// <param name="settingId">The SettingID</param>
        public void DeleteConnectionSetting(long settingId)
        {
            List<ConnectionProtocolOption> ConnectionOptions = GetConnectionOptions(settingId);

            //Check for containing ConnectionOptions and delte
            if (ConnectionOptions.Count > 0)
            {
                for (int i = 0; i < ConnectionOptions.Count; i++)
                    DeleteConnectionOption(ConnectionOptions[i].getId());

            }

            //Delete ConnectionSetting
            _Com.DeleteConnectionSetting(settingId);
        }


        /// <summary>
        /// Modifies an existing connection; identified by the ID
        /// </summary>
        /// <param name="id">The ID of the connection to modify</param>
        /// <param name="host">Hostname or IP of the Host</param>
        /// <param name="name">Displayname of the Host</param>
        /// <param name="description">Description of the host</param>
        /// <param name="os">Operating System</param>
        /// <param name="owner">OwnerID of this connection</param>
        /// <param name="isPublic">Is this Connection visible for all?</param>
        public void ModifyConnection(long id, string host, string name, string description, int os, int owner, bool isPublic, int vpn)
        {
            host = DefuseString(host); //Defusing string to prohibit SQL-Injections
            name = DefuseString(name); //Defusing string to prohibit SQL-Injections
            description = DefuseString(description); //Defusing string to prohibit SQL-Injections

            _Com.ModifyConnection(id, host, name, description, os, owner, isPublic, vpn);
        }

        /// <summary>
        /// Modifies the ParentFolder of a connection
        /// </summary>
        /// <param name="id">The ID of the connection</param>
        /// <param name="parentFolder">The ID of the parent folder</param>
        /// <returns>query successfull/failed</returns>
        public bool ModifyConnection(long id, long parentFolder)
        {
            try
            {
                _Com.ModifyConnection(id, parentFolder);
                return (true);
            }
            catch (Exception)
            {
                return (false);
            }
            
        }

        /// <summary>
        /// Modifies the Sortorder of the ParentFolder of a connection
        /// </summary>
        /// <param name="id">ID of the connection</param>        
        /// <param name="sortOrder">The current Sortorder of the Id</param>
        /// <param name="sortUp">Sort the Id Up or Down in the Connectionview?</param>        
        /// <returns>query successfull/failed</returns>
        public bool ModifyConnection(long id, long sortOrder, bool sortUp)
        {
            try
            {
                if (sortUp)
                {
                    //Get the previous entry
                    long prevId = _Com.HasConnectionUp(id);

                    if (prevId != 0)
                    {
                        _Com.ModifyConnectionSortOrderUp(id);
                        _Com.ModifyConnectionSortOrder(prevId, sortOrder);
                    }
                }
                else
                {
                    //Get the previous entry
                    long nextId = _Com.HasConnectionDown(id);

                    if (nextId != 0)
                    {
                        _Com.ModifyConnectionSortOrderDown(id);
                        _Com.ModifyConnectionSortOrder(nextId, sortOrder);
                    }
                }


                return (true);
            }
            catch (Exception)
            {
                return (false);    
            }
        }

        /// <summary>
        /// Adds a new Connection
        /// </summary>
        /// <param name="host">Hostname or IP-Address of the Host</param>
        /// <param name="name">Displayname of the Host</param>
        /// <param name="description">Description of the Host</param>
        /// <param name="folderId">Folder Id</param>
        /// <param name="owner">OwnerID of this connection</param>
        /// <param name="isPublic">Is this Connection visible for all?</param>
        /// <param name="os">Operating System</param>
        /// <param name="vpn"></param>
        /// <returns>The id of the new connection</returns>
        public long AddConnection(string host, string name, string description, int os, long folderId, int owner, bool isPublic, int vpn)
        {
            host = DefuseString(host); //Defusing string to prohibit SQL-Injections
            name = DefuseString(name); //Defusing string to prohibit SQL-Injections
            description = DefuseString(description); //Defusing string to prohibit SQL-Injections

            return (_Com.AddConnection(host, name, description, os, folderId, owner, isPublic, vpn));
        }

        /// <summary>
        /// Gets a connection
        /// </summary>
        /// <param name="id">The ID of the Connection</param>
        /// <returns>The Connection Parameters</returns>
        public ConnectionHost GetConnection(long id)
        {
            var conn = _Com.GetConnection(id);

            if (conn.Count != 0)
            {
                return (new ConnectionHost(Convert.ToInt32(conn["id"]),
                                           EnfuseString(conn["host"].ToString()),
                                           EnfuseString(conn["name"].ToString()),
                                           EnfuseString(conn["description"].ToString()),
                                           Convert.ToInt32(conn["folder"]),
                                           Convert.ToInt32(conn["sortorder"]),
                                           Convert.ToInt32(conn["os"]),
                                           Convert.ToInt32(conn["owner"]),
                                           Convert.ToBoolean(conn["ispublic"]),
                                           Convert.ToInt32(conn["vpn"])));
            }
            
            return(null);
        }

        /// <summary>
        /// Returns a Datatable with all Connections of the current User
        /// </summary>
        /// <returns>All Connections of the current user</returns>
        public List<ConnectionHost> GetConnections()
        {
            return(GetConnections(GetUserId()));
        }

        /// <summary>
        /// Gets all connections of the current user that has the given displayname
        /// </summary>
        /// <param name="displayname">The Displayname that is queried</param>
        /// <returns>All connections with the given Displayname</returns>
        public List<ConnectionHost> GetConnections(string displayname)
        {
            DataTable connectionSet = _Com.GetConnections(displayname, GetUserId());

            List<ConnectionHost> ReturnValue = getConnectionsHelper(connectionSet);

            return (ReturnValue);
        }

        /// <summary>
        /// Returns a Datatable with all Connections of the current User
        /// </summary>        
        /// <param name="userId">The UserID of the User the connectionlist is queried from</param>
        /// <returns>All Connections of the given User</returns>
        public List<ConnectionHost> GetConnections(long userId)
        {
            DataTable connectionSet = _Com.GetConnections(userId);

            List<ConnectionHost> ReturnValue = getConnectionsHelper(connectionSet);

            return (ReturnValue);
        }

        /// <summary>
        /// Get all Connections, that matching the filters
        /// </summary>
        /// <param name="filterList"></param>
        /// <returns></returns>
        public List<ConnectionHost> GetConnections(List<FilterClass> filterList)
        {
            return (GetConnections(filterList, GetUserId()));
        }

        /// <summary>
        /// Get all Connections, that matching the filters
        /// </summary>
        /// <param name="filterList"></param>
        /// <param name="userId">The Id of the user</param>
        /// <returns></returns>
        public List<ConnectionHost> GetConnections(List<FilterClass> filterList, long userId)
        {

            List<List<string>> comData = new List<List<string>>();
            string wildcard = "";

            //Translating the Settings for the Communicator
            foreach (FilterClass aFilter in filterList)
            {
                string conditionString = aFilter.ConditionType.ToString(); //Contains Name, IP, Description, OS, ...

                //aFilter.IsNot
                //aFilter.IsOr
                //aFilter.IsLike
                //aFilter.Description
                //aFilter.Value

                string prefix = "";
                string compare = "";
                string link = "AND";

                if (aFilter.Operation == FilterOperation.Equal)
                {
                    compare = "=";
                }
                else if (aFilter.Operation == FilterOperation.NotEqual)
                {
                    prefix = "NOT";
                    compare = "=";
                }
                else if (aFilter.Operation == FilterOperation.Like)
                {
                    compare = "LIKE";
                    wildcard = "%";
                }
                else if (aFilter.Operation == FilterOperation.NotLike)
                {
                    compare = "NOT LIKE";
                    wildcard = "%";
                }

                if (aFilter.IsOr)
                {
                    link = "OR";
                }

                string filterValue = "";
                if (aFilter.Value is string)
                {
                    filterValue = DefuseString(aFilter.Value.ToString());
                }

                var comDataPart = new List<string>();
                comDataPart.Add(prefix);
                comDataPart.Add(compare);
                comDataPart.Add(conditionString);
                comDataPart.Add(filterValue);
                comDataPart.Add(link);

                comData.Add(comDataPart);
            }

            DataTable connections = _Com.GetConnections(comData, wildcard, GetUserId());

            List<ConnectionHost> ReturnValue = getConnectionsHelper(connections);

            return (ReturnValue);
        }

        /// <summary>
        /// Converts DataTables of connections into ConnectionHost
        /// </summary>
        /// <param name="connectionsTable"></param>
        /// <returns></returns>
        private List<ConnectionHost> getConnectionsHelper(DataTable connectionsTable)
        {
            //Convert the Datatable to the List<ConnectionOption>
            List<ConnectionHost> ReturnValue = new List<ConnectionHost>();
            for (int i = 0; i < connectionsTable.Rows.Count; i++)
            {
                ConnectionHost newOption = new ConnectionHost(Convert.ToInt32(connectionsTable.Rows[i]["id"]),
                    EnfuseString(connectionsTable.Rows[i]["host"].ToString()),
                    EnfuseString(connectionsTable.Rows[i]["name"].ToString()),
                    EnfuseString(connectionsTable.Rows[i]["description"].ToString()),
                    Convert.ToInt32(connectionsTable.Rows[i]["folder"]),
                    Convert.ToInt32(connectionsTable.Rows[i]["sortorder"]),
                    Convert.ToInt32(connectionsTable.Rows[i]["os"]),
                    Convert.ToInt32(connectionsTable.Rows[i]["owner"]),
                    Convert.ToBoolean(connectionsTable.Rows[i]["ispublic"]),
                    Convert.ToInt32(connectionsTable.Rows[i]["vpn"]));

                ReturnValue.Add(newOption);
            }
            return (ReturnValue);
        }

        /// <summary>
        /// Gets all Protocols of a User 
        /// </summary>
        /// <param name="userId">UserID the Connection-Protocol-Settings is queried from</param>
        /// <returns>All Protocols for the given user</returns>
        public List<ConnectionProtocol> GetConnectionsWithSettings(long userId)
        {   
            return( GetConnectionSettings());
        }

        /// <summary>
        /// Returns a Datatable with all Connections
        /// </summary>        
        /// <param name="folderId">The folder ID the connections are searched from</param>
        /// <returns>All Connections</returns>
        public List<ConnectionHost> GetConnectionsInFolder(long folderId)
        {
            DataTable connectionSet = _Com.GetConnectionsInFolder(folderId);

            //Convert the Datatable to the List<ConnectionOption>
            List<ConnectionHost> ReturnValue = new List<ConnectionHost>();
            for (int i = 0; i < connectionSet.Rows.Count; i++)
            {
                ConnectionHost newOption = new ConnectionHost(Convert.ToInt32(connectionSet.Rows[i]["id"]),
                    EnfuseString(connectionSet.Rows[i]["host"].ToString()),
                    EnfuseString(connectionSet.Rows[i]["name"].ToString()),
                    EnfuseString(connectionSet.Rows[i]["description"].ToString()),
                    Convert.ToInt32(connectionSet.Rows[i]["folder"]),
                    Convert.ToInt32(connectionSet.Rows[i]["sortorder"]),
                    Convert.ToInt32(connectionSet.Rows[i]["os"]),
                    Convert.ToInt32(connectionSet.Rows[i]["owner"]),
                    Convert.ToBoolean(connectionSet.Rows[i]["ispublic"]),
                    Convert.ToInt32(connectionSet.Rows[i]["vpn"]));

                ReturnValue.Add(newOption);
            }
            return (ReturnValue);
        }

        /// <summary>
        /// Deletes a Connection and all Containing Settings
        /// </summary>
        /// <param name="connectionId">ConnectionId</param>
        public void DeleteConnection(long connectionId)
        {
            List<ConnectionProtocol> connectionSettings = GetConnectionSettings(connectionId);

            //Check if there are Settings
            if (connectionSettings.Count > 0)
            {
                for (int i = 0; i < connectionSettings.Count; i++)
                    DeleteConnectionSetting(connectionSettings[i].Id);
            }

            //Delete Connection
            _Com.DeleteConnection(connectionId);
        }



        /// <summary>
        /// Gets a Folder
        /// </summary>
        /// <param name="folderId">ID of the Folder to get</param>
        /// <returns></returns>
        public Folder GetFolder(long folderId)
        {
            DataTable connectionSet = _Com.GetFolder(folderId);

            if (connectionSet.Rows.Count > 0)
            {
                //Convert the Datatable to the Folder
                var ReturnValue = new Folder(Convert.ToInt32(connectionSet.Rows[0]["id"]),
                        EnfuseString(connectionSet.Rows[0]["foldername"].ToString()),
                        Convert.ToInt32(connectionSet.Rows[0]["parent"]),
                        Convert.ToInt32(connectionSet.Rows[0]["sortorder"]),
                        Convert.ToInt32(connectionSet.Rows[0]["owner"]),
                        Convert.ToBoolean(connectionSet.Rows[0]["ispublic"]));


                return (ReturnValue);
            }
            
            return (null);
        }

        /// <summary>
        /// Gets all Folders of the current user
        /// </summary>
        /// <returns>A list of all Folders of the current User</returns>
        public List<Folder> GetFolders()
        {
            DataTable connectionSet = _Com.GetFolders();

            List<Folder> ret = new List<Folder>();

            for (int i = 0; i < connectionSet.Rows.Count; i++)            
            {
                //Convert the Datatable to the Folder
                Folder folderValue = new Folder(Convert.ToInt32(connectionSet.Rows[i]["id"]),
                        EnfuseString(connectionSet.Rows[i]["foldername"].ToString()),
                        Convert.ToInt32(connectionSet.Rows[i]["parent"]),
                        Convert.ToInt32(connectionSet.Rows[i]["sortorder"]),
                        Convert.ToInt32(connectionSet.Rows[i]["owner"]),
                        Convert.ToBoolean(connectionSet.Rows[i]["ispublic"]));

                ret.Add(folderValue);                
            }

            return (ret);
        }

        /// <summary>
        /// Checks if a Folder with the given Parameter exists for the current user
        /// </summary>
        /// <param name="foldername">Name of the Folder</param>        
        /// <returns>Folder already exists (true) or not (false)</returns>
        public bool GetFolderExists(string foldername)
        {
            return (GetFolderExists(foldername, GetUserId()));
        }

        /// <summary>
        /// Checks if a Folder with the given Parameter exists
        /// </summary>
        /// <param name="foldername">Name of the Folder</param>
        /// <param name="userId">ID of the Owner</param>
        /// <returns>Folder already exists (true) or not (false)</returns>
        public bool GetFolderExists(string foldername, long userId)
        {
            int FolderId = _Com.GetFolderId(DefuseString(foldername), userId);

            if (FolderId != 0)
                return (true);
            
            return (false);
        }

        /// <summary>
        /// Returns if a Folder in an explicific folder exists
        /// </summary>
        /// <param name="foldername">Foldername to check</param>
        /// <param name="userId">UserID the Folder should belongs to</param>
        /// <param name="parentFolderId">The folder where this folder will be created in</param>
        /// <returns>Folder already exists (true) or not (false)</returns>
        public bool GetFolderExists(string foldername, long userId, long parentFolderId)
        {
            int FolderId = _Com.GetFolderId(DefuseString(foldername), userId, parentFolderId);

            if (FolderId != 0)
                return (true);
            
            return (false);
        }

        /// <summary>
        /// Gets all Folders in a List for a Text-Based-List
        /// </summary>
        /// <returns>A List of all Folders so long</returns>
        public List<Folder> GetSubfolders(List<Folder> returnValue, long parentId, string prefix)
        {
            //Getting all Rootfolders
            List<Folder> connectionSet = GetSubfolders(parentId);

            foreach (Folder aFolder in connectionSet)
            {
                aFolder.Name = prefix + "└" + aFolder.Name;
                returnValue.Add(aFolder);
                returnValue = GetSubfolders(returnValue, aFolder.getId(), prefix + " ");
            }

            return (returnValue);
        }

        /// <summary>
        /// Gets all Subfolders from parentId
        /// </summary>
        /// <param name="parentId">The Parent-Folder-Id</param>
        /// <returns>A Datatable with the containing informations</returns>
        public List<Folder> GetSubfolders(long parentId)
        {
            DataTable connectionSet = _Com.GetSubfolders(parentId);

            //Convert the Datatable to the List<Folder>
            var ReturnValue = new List<Folder>();
            for (int i = 0; i < connectionSet.Rows.Count; i++)
            {
                var newFolder = new Folder(Convert.ToInt32(connectionSet.Rows[i]["id"]),
                    EnfuseString(connectionSet.Rows[i]["foldername"].ToString()),
                    Convert.ToInt32(connectionSet.Rows[i]["parent"]),
                    Convert.ToInt32(connectionSet.Rows[i]["sortorder"]),
                    Convert.ToInt32(connectionSet.Rows[i]["owner"]),
                    Convert.ToBoolean(connectionSet.Rows[i]["ispublic"]));

                ReturnValue.Add(newFolder);
            }
            return (ReturnValue);
        }

        /// <summary>
        /// Deletes a Folder and all containing childs
        /// </summary>
        /// <param name="folderId">FolderId</param>
        public void DeleteFolder(long folderId)
        {
            List<ConnectionHost> connectionsInFolder = GetConnectionsInFolder(folderId);

            //Check for containing Connections
            if (connectionsInFolder.Count > 0)
            { 
                for (int i = 0; i < connectionsInFolder.Count; i++)
                    DeleteConnection(connectionsInFolder[i].ID);                
            }

            //Check for containing Folder
            List<Folder> subfolder = GetSubfolders(folderId);
            if (subfolder.Count > 0)
            {
                for (int i = 0; i < subfolder.Count; i++)
                    DeleteFolder(subfolder[i].getId());
            }
            
            //Delete folder
            _Com.DeleteFolder(folderId);
        }

        /// <summary>
        /// Adds a Folder for the current user
        /// </summary>
        /// <param name="name">Name of the Folder</param>
        /// <param name="parentId">ID of the parent</param>
        /// <param name="ispublic">Is it a public folder?</param>
        /// <returns>Added ID</returns>
        public long AddFolder(string name, long parentId, bool ispublic)
        {
            return (AddFolder(name, parentId, _Com.GetUserId(), ispublic));
        }

        /// <summary>
        /// Adds a Folder
        /// </summary>
        /// <param name="name">Name of the Folder</param>
        /// <param name="parentId">ID of the parent</param>
        /// <param name="owner">OwnerID</param>
        /// <param name="ispublic">Is it a public folder?</param>
        /// <returns>Added ID</returns>
        public long AddFolder(string name, long parentId, int owner, bool ispublic)
        {
            name = DefuseString(name); //Defusing string to prohibit SQL-Injections

            return (_Com.AddFolder(name, parentId, owner, ispublic));
        }

        /// <summary>
        /// Gets the FolderId of a Folder by Name
        /// </summary>
        /// <param name="name">The Name of the folder the ID is searched for (attention: this can not be unique!)</param>        
        /// <returns>The ID of the Folder</returns>
        public int GetFolderId(string name)
        {
            return (GetFolderId(name, GetUserId()));
        }

        /// <summary>
        /// Gets the FolderId of a Folder by Name
        /// </summary>
        /// <param name="name">The Name of the folder the ID is searched for (attention: this can not be unique!)</param>        
        /// <param name="userId">The UserID the folder belongs to</param>
        /// <returns>The ID of the Folder</returns>
        public int GetFolderId(string name, long userId)
        {
            return (_Com.GetFolderId(DefuseString(name), userId));
        }

        /// <summary>
        /// Moves a folder to another folder
        /// </summary>
        /// <param name="folderId">Folder to move</param>
        /// <param name="newParentFolder">Targeted folder</param>
        /// <returns>success/fail</returns>
        public bool ModifyFolderParent(long folderId, long newParentFolder)
        {
            try
            {
                _Com.ModifyFolderParent(folderId, newParentFolder);
                return (true);
            }
            catch
            {
                return (false);
            }
            
        }

        /// <summary>
        /// Resort a Folder
        /// </summary>
        /// <param name="folderId">Folder to sort</param>
        /// <param name="sortOrder">The new Index</param>
        /// <param name="sortUp">Sort it up or down</param>
        /// <returns>success/fail</returns>
        public bool ModifyFolderSortOrder(long folderId, long sortOrder, bool sortUp)
        {
            try
            {
                if (sortOrder > 0 && sortUp)
// ReSharper disable once RedundantAssignment
                    _Com.ModifyFolderSortOrder(folderId, sortOrder--);
                else if (sortUp == false)
// ReSharper disable once RedundantAssignment
                    _Com.ModifyFolderSortOrder(folderId, sortOrder++);
                return (true);
            }
            catch
            {
                return (false);    
            }
        }


        /// <summary>
        /// Gets the Information for a specific versionId
        /// </summary>
        /// <param name="versionId">The ID of the versionentry</param>
        /// <returns>Versioninformation</returns>
        public OSVersion GetVersion(int versionId)
        {
            DataTable connectionSet = _Com.GetVersion(versionId);

            return (new OSVersion(Convert.ToInt32(connectionSet.Rows[0]["id"]),
                connectionSet.Rows[0]["family"].ToString(),
                connectionSet.Rows[0]["distribution"].ToString(),
                connectionSet.Rows[0]["version"].ToString()));
        }

        /// <summary>
        /// Gets a list of all Versions
        /// </summary>
        /// <returns>A List of all OSVersions</returns>
        public List<OSVersion> GetVersionlist()
        {
            DataTable connectionSet = _Com.GetVersionlist();

            //Convert the Datatable to the List<ConnectionOption>
            List<OSVersion> ReturnValue = new List<OSVersion>();
            for (int i = 0; i < connectionSet.Rows.Count; i++)
            {
                OSVersion newFolder = new OSVersion(Convert.ToInt32(connectionSet.Rows[i]["id"]),
                    connectionSet.Rows[i]["family"].ToString(),
                    connectionSet.Rows[i]["distribution"].ToString(),
                    connectionSet.Rows[i]["version"].ToString());

                ReturnValue.Add(newFolder);
            }
            return (ReturnValue);
        }


        /// <summary>
        /// Adds new Credentials to the Database for the current user
        /// </summary>
        /// <param name="username">The Username to login</param>
        /// <param name="password">The Password as a SecureString</param>
        /// <param name="domain">The Domain of the Username, if used</param>
        /// <param name="description">Description</param>
        /// <returns>The ID of the Added Credentials</returns>
        public long AddUserCredentials(string username, byte[] password, string domain, string description)
        {   
            return (AddUserCredentials(username, password, domain, description, GetUserId()));
        }

        /// <summary>
        /// Adds new Credentials to the Database for a specific user
        /// </summary>
        /// <param name="username">The Username to login (i.e. Windows-Username)</param>
        /// <param name="password">The Password as a SecureString</param>
        /// <param name="domain">The Domain of the Username, if used</param>
        /// <param name="description">Description</param>
        /// <param name="owner">The ID of the User, that owns this credentials</param>
        /// <returns>The ID of the Added Credentials</returns>
        public long AddUserCredentials(string username, byte[] password, string domain, string description, int owner)
        {
            username = DefuseString(username); //Defusing string to prohibit SQL-Injections
            domain = DefuseString(domain); //Defusing string to prohibit SQL-Injections
            description = DefuseString(description); //Defusing string to prohibit SQL-Injections

            return (_Com.AddUserCredentials(username, password, domain, description, owner));
        }

        /// <summary>
        /// Get the Credentials of a specific ID
        /// </summary>
        /// <param name="id">The ID of the Credential</param>
        /// <returns>Returns the Credentials</returns>
        public UserCredential GetUserCredentials(int id)
        {
            if (id == 0)
                return (null);

            var credentialSet = _Com.GetUserCredentials(id);

            if (credentialSet.Rows.Count > 0)
            {
                var newCred = new UserCredential(Convert.ToInt32(credentialSet.Rows[0]["id"]),
                        EnfuseString(credentialSet.Rows[0]["username"].ToString()),
                        (byte[])credentialSet.Rows[0]["password"],
                        EnfuseString(credentialSet.Rows[0]["domain"].ToString()),
                        EnfuseString(credentialSet.Rows[0]["description"].ToString()),
                        Convert.ToInt32(credentialSet.Rows[0]["owner"]));

                return (newCred);
            }

            return (null);
        }

        /// <summary>
        /// Get the Credentials of the current User
        /// </summary>        
        /// <returns>Returns the Credentials</returns>
        public List<UserCredential> GetUserCredentialsAll()
        {
            return (GetUserCredentialsAll(GetUserId()));
        }

        /// <summary>
        /// Get the Credentials of a specific ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>Returns the Credentials</returns>
        public List<UserCredential> GetUserCredentialsAll(long userid)
        {
            var credentialSet = _Com.GetUserCredentialsAll(userid);

            //Convert the Datatable to the List<ConnectionOption>
            var ReturnValue = new List<UserCredential>();
            for (var i = 0; i < credentialSet.Rows.Count; i++)
            {
                //If the current user gets credentials, get the credentials with password, else a dummy-pw
                var password = credentialSet.Rows[i]["password"];;

                var newCred = new UserCredential(Convert.ToInt32(credentialSet.Rows[i]["id"]),
                    EnfuseString(credentialSet.Rows[i]["username"].ToString()),
                    (byte[])password,
                    EnfuseString(credentialSet.Rows[i]["domain"].ToString()),
                    EnfuseString(credentialSet.Rows[i]["description"].ToString()),
                    Convert.ToInt32(credentialSet.Rows[i]["owner"]));

                ReturnValue.Add(newCred);
            }
            return (ReturnValue);
        }


        /// <summary>
        /// Deletes a UserCredential
        /// </summary>
        /// <param name="id">Id of the Credential to delete</param>
        public void DeleteUserCredential(int id)
        {
            _Com.DeleteUserCredential(id);            
        }

        /// <summary>
        /// Modifies a Credential
        /// </summary>
        /// <param name="id">ID of the Credential</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="domain">Domain</param>
        /// <param name="owner">Owner</param>
        /// <param name="description">Description</param>
        public void ModifyUserCredential(int id, string username, byte[] password, string domain, int owner, string description)
        {
            username = DefuseString(username); //Defusing string to prohibit SQL-Injections
            domain = DefuseString(domain); //Defusing string to prohibit SQL-Injections
            description = DefuseString(description); //Defusing string to prohibit SQL-Injections

            _Com.ModifyUserCredential(id, username, password, domain, owner, description);
        }

        /// <summary>
        /// Modifies a Credential
        /// </summary>
        /// <param name="id">ID of the Credential</param>
        /// <param name="username">Username</param>
        /// <param name="domain">Domain</param>
        /// <param name="owner">Owner</param>
        /// <param name="description">Description</param>
        public void ModifyUserCredential(int id, string username, string domain, int owner, string description)
        {
            username = DefuseString(username); //Defusing string to prohibit SQL-Injections
            domain = DefuseString(domain); //Defusing string to prohibit SQL-Injections
            description = DefuseString(description); //Defusing string to prohibit SQL-Injections

            _Com.ModifyUserCredential(id, username, domain, owner, description);
        }

        /// <summary>
        /// Resets a Credential-Password. Required on password-resets
        /// </summary>
        /// <param name="id"></param>
        public void ResetUserCredentialPassword(long id)
        {
            _Com.ResetUserCredentialPassword(id);
        }




        /// <summary>
        /// Enable or Disable the Proxy for a user
        /// </summary>
        /// <param name="enabled"></param>
        public void SetUserProxySettings(bool enabled)
        {
            _Com.SetUserProxySettings(false, "", 0, 0, 0, GetUserId());
        }

        /// <summary>
        /// Enable a Proxy by using System Settings
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="useSystemSettings"></param>
        public void SetUserProxySettings(bool enabled, bool useSystemSettings)
        {
            _Com.SetUserProxySettings(true, "", 0, 0, 2, GetUserId());
        }

        /// <summary>
        /// Set Proxysettings of a User
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="bypasslocal"></param>
        public void SetUserProxySettings(string host, int port, bool bypasslocal)
        {
            SetUserProxySettings(host, port, 0, bypasslocal);
        }

        /// <summary>
        /// Set Proxysettings of a User
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="credentialid">The ID of the credentialset to use. 0=No Credentials, -1=Current Credentials</param>
        /// <param name="bypasslocal"></param>
        public void SetUserProxySettings(string host, int port, int credentialid, bool bypasslocal)
        {
            int flags = 0;
            if (bypasslocal)
            {
                flags += 1;
            }

            _Com.SetUserProxySettings(true, DefuseString(host), port, credentialid, flags, GetUserId());
        }

        /// <summary>
        /// Get UserProxy-Settings
        /// </summary>
        /// <returns></returns>
        public UserProxySetting GetUserProxySettings()
        {
            var ret = new UserProxySetting();
            var dt = _Com.GetUserProxySettings(GetUserId());

            //If any error occures
            if (dt.Rows.Count > 0)
            {
                //If no proxy is set
                if (dt.Rows[0]["proxyenabled"].ToString() == "False")
                {
                    return (null);
                }

                //Get Flags; advance here if required
                int flags = Convert.ToInt32(dt.Rows[0]["proxyflags"]);
                bool bypass = false;
                bool useSystem = false;                
                if (flags >= 2)
                {
                    useSystem = true;
                    flags -= 2;
                }
                if (flags == 1)
                {
                    bypass = true;
                }

                

                //Set Credentials
                ret.ConfiguredProxy = new System.Net.WebProxy();                

                if (useSystem)
                {
#pragma warning disable 618
                    ret.ConfiguredProxy = System.Net.WebProxy.GetDefaultProxy();
#pragma warning restore 618
                    //wP = System.Net.WebRequest.GetSystemWebProxy(); //Is better, but currently not applicable

                    ret.UseSystemSettings = true;
                }
                else
                {
                    //Get Proxy-Settings
                    var wpub = new UriBuilder();

                    wpub.Host = EnfuseString(dt.Rows[0]["proxyhost"].ToString());
                    wpub.Port = Convert.ToInt32(dt.Rows[0]["proxyport"]);

                    ret.ConfiguredProxy = new System.Net.WebProxy(wpub.Uri);
                    ret.ConfiguredProxy.BypassProxyOnLocal = bypass;


                    if (dt.Rows[0]["proxycredentials"].ToString() == "-1")
                    {
                        ret.ConfiguredProxy.UseDefaultCredentials = true;                        
                    }
                    else if (Convert.ToInt32(dt.Rows[0]["proxycredentials"]) > 0)
                    {
                        var uC = GetUserCredentials(Convert.ToInt32(dt.Rows[0]["proxycredentials"]));
                        ret.ConfiguredProxy.Credentials = new System.Net.NetworkCredential(uC.Username, Helper.DecryptStringFromBytes(uC.Password, Helper.GetHash1(StorageCore.Core.GetUserSalt1()), Encoding.UTF8.GetBytes(_Com.GetDatabaseGuid()), GetUserSalt3()), uC.Domain);
                        ret.UserCredentialId = uC.Id;
                    }
                }

                return (ret);
            }
            return (null);
        }





        /// <summary>
        /// Gets a List of all available OperatingSystems
        /// </summary>
        /// <returns>A List of al UperatingSystems</returns>
        public List<OSVersion> GetOperatingSystemList()
        {
            List<OSVersion> ret = new List<OSVersion>();
            DataTable dT = _Com.GetOperatingSystemList();

            for (int i = 0; i < dT.Rows.Count; i++)
            {
                ret.Add(new OSVersion(Convert.ToInt32(dT.Rows[i][0]), dT.Rows[i][1].ToString(), dT.Rows[i][2].ToString(), dT.Rows[i][3].ToString()));
            }

            return (ret);
        }

        /// <summary>
        /// Gets the OSVersion-Element of the given ID
        /// </summary>
        /// <param name="id">The ID of the OS</param>
        /// <returns>The OSVersion-Element of id</returns>
        public OSVersion GetOperatingSystem(int id)
        { 
            DataTable dT = _Com.GetOperatingSystem(id);
            return (new OSVersion(Convert.ToInt32(dT.Rows[0][0]), dT.Rows[0][1].ToString(), dT.Rows[0][2].ToString(), dT.Rows[0][3].ToString()));
        }

        /// <summary>
        /// Adds a new Operatingsystem to the list
        /// </summary>
        /// <param name="id">The ID of the OS</param>
        /// <param name="family">The Family</param>
        /// <param name="distribution">The Distribution</param>
        /// <param name="version">The Version </param>
        public void AddOperatingSystem(int id, string family, string distribution, string version)
        {
            _Com.AddOperatingSystem(id, family, distribution, version);
        }



        /// <summary>
        /// Get a List of all Licenses of the current user
        /// </summary>
        /// <returns>a List of all Licenses of the current user</returns>
        public List<License> GetLicenses()
        {
            List<License> ret = new List<License>();

            DataTable dt = _Com.GetLicenses();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ret.Add(new License(dt.Rows[i][0].ToString(),
                    dt.Rows[i][1].ToString(),
                    dt.Rows[i][2].ToString(),
                    dt.Rows[i][3].ToString(),
                    Convert.ToInt32(dt.Rows[i][4])));
            }

            return (ret);
        }

        /// <summary>
        /// Get a List of all Licenses of a special User
        /// </summary>
        /// <returns>a List of all Licenses of a specific user</returns>
        public List<License> GetUserLicenses(long userId)
        {
            List<License> ret = new List<License>();

            DataTable dt = _Com.GetUserLicenses(userId);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ret.Add(new License(dt.Rows[i][0].ToString(),
                    dt.Rows[i][1].ToString(),
                    dt.Rows[i][2].ToString(),
                    dt.Rows[i][3].ToString(),
                    Convert.ToInt32(dt.Rows[i][4])));
            }

            return (ret);
        }

        /// <summary>
        /// Adds a new License to the Database
        /// </summary>
        /// <param name="license">the new License-Information</param>
        public void AddUserLicense(License license)
        {
            _Com.AddUserLicense(license.getFirstname(), license.getLastname(), license.getEmail(), license.getSecret(), license.getUserId());
        }

        /// <summary>
        /// Deletes a License of a user
        /// </summary>
        /// <param name="license">The License-Infromation to delete</param>
        public void DeleteUserLicense(License license)
        {
            _Com.DeleteUserLicense(license.getFirstname(), license.getLastname(), license.getEmail(), license.getSecret(), license.getUserId());
        }



        /// <summary>
        /// Adds an Entry to the history
        /// </summary>
        /// <param name="connectionSettingId">The ConnectionSettingId that the user connected to</param>
        public void AddUserHistoryEntry(long connectionSettingId)
        {
            AddUserHistoryEntry(GetUserId(), connectionSettingId);
        }

        /// <summary>
        /// Adds an Entry to the history
        /// </summary>
        /// <param name="userId">The UserId that connected to any system</param>
        /// <param name="connectionSettingId">The ConnectionSettingId that the user connected to</param>
        public void AddUserHistoryEntry(long userId, long connectionSettingId)
        {
            _Com.AddHistoryEntry(userId, connectionSettingId);
        }

        /// <summary>
        /// Cleanup Old Userhistoryinformation
        /// </summary>
        /// <param name="historyLimit">The maximum number of history-entries for this user</param>
        public void TidyUpUserHistory(int historyLimit)
        {
            TidyUpUserHistory(GetUserId(), historyLimit);
        }

        /// <summary>
        /// Cleanup Old Userhistoryinformation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="historyLimit">The maximum number of history-entries for this user</param>
        public void TidyUpUserHistory(long userId, int historyLimit)
        {
            int entriesCount = _Com.GetHistoryEntriesCount(userId);

            if (entriesCount > historyLimit)
            {
                DataTable lastEntry = _Com.GetHistoryEntryNumber(userId, historyLimit);
                _Com.TidyUpHistory(userId, (int)lastEntry.Rows[0]["id"]);
            }
        }

        /// <summary>
        /// Get the complete UserHistory of the current User
        /// </summary>
        /// <returns>A list of all UserHistory-Entries</returns>
        public List<UserHistoryEntry> GetUserHistory()
        {
            return (GetUserHistory(GetUserId()));
        }

        /// <summary>
        /// Get the complete UserHistory of a User
        /// </summary>
        /// <param name="userId">The UserId the List should be queried from</param>
        /// <returns>A list of all UserHistory-Entries</returns>
        public List<UserHistoryEntry> GetUserHistory(long userId)
        {
            DataTable dt = _Com.GetHistory(userId);
            List<UserHistoryEntry> ret = new List<UserHistoryEntry>();
            
            foreach (DataRow dR in dt.Rows)
            {
                UserHistoryEntry uhe = new UserHistoryEntry();
                uhe.Id = (int)dR["id"];
                uhe.UserId = (int)dR["userid"];
                uhe.ConnectionSettingId = (int)dR["connectionsettingid"];
                uhe.PointOfTime = (DateTime)dR["pointoftime"];
                uhe.Name = EnfuseString((string)dR["name"]);
                uhe.Protocol = (string)dR["protocol"];
                uhe.ConnectionId = (int)dR["connectionid"];
                uhe.Port = (int)dR["port"];
                uhe.Host = EnfuseString((string)dR["host"]);

                ret.Add(uhe);
            }

            return(ret);
        }

        /// <summary>
        /// Get a specified number of History-Entries of a user
        /// </summary>
        /// <param name="userId">The UserId the History is queried</param>
        /// <param name="count">The maximum number of entries</param>
        /// <param name="offset">The Offset</param>
        /// <returns>A range of the UserHistory</returns>
        public List<UserHistoryEntry> GetUserHistory(long userId, int count, int offset)
        {
            DataTable dt = _Com.GetHistoryCount(userId,count, offset);
            List<UserHistoryEntry> ret = new List<UserHistoryEntry>();

            foreach (DataRow dR in dt.Rows)
            {
                UserHistoryEntry uhe = new UserHistoryEntry();
                uhe.Id = Convert.ToInt32(dR["id"]);
                uhe.UserId = Convert.ToInt32(dR["userid"]);
                uhe.ConnectionSettingId = Convert.ToInt32(dR["connectionsettingid"]);
                uhe.PointOfTime = Convert.ToDateTime(dR["pointoftime"]);
                uhe.Name = EnfuseString((string)dR["name"]);
                uhe.Protocol = (string)dR["protocol"];
                uhe.ConnectionId = Convert.ToInt32(dR["connectionid"]);
                uhe.Port = Convert.ToInt32(dR["port"]);
                uhe.Host = EnfuseString((string)dR["host"]);

                ret.Add(uhe);
            }

            return (ret);
        }

        /// <summary>
        /// Returns all History-Entries of the given Date
        /// </summary>
        /// <param name="userId">The UserId the History is queried from</param>
        /// <param name="date">The Date that is queried</param>
        /// <returns>All Entries of the given date</returns>
        public List<UserHistoryEntry> GetHistoryDate(long userId, DateTime date)
        {
            DataTable dt = _Com.GetHistoryDate(userId, date);
            List<UserHistoryEntry> ret = new List<UserHistoryEntry>();

            foreach (DataRow dR in dt.Rows)
            {
                UserHistoryEntry uhe = new UserHistoryEntry();
                uhe.Id = Convert.ToInt32(dR["id"]);
                uhe.UserId = Convert.ToInt32(dR["userid"]);
                uhe.ConnectionSettingId = Convert.ToInt32(dR["connectionsettingid"]);
                uhe.PointOfTime = Convert.ToDateTime(dR["pointoftime"]);
                uhe.Name = EnfuseString((string)dR["name"]);
                uhe.Protocol = (string)dR["protocol"];
                uhe.ConnectionId = Convert.ToInt32(dR["connectionid"]);
                uhe.Port = Convert.ToInt32(dR["port"]);
                uhe.Host = EnfuseString((string)dR["host"]);

                ret.Add(uhe);
            }

            return (ret);
        }

        /// <summary>
        /// Get a specified History-Entry
        /// </summary>
        /// <param name="entryId">The ID of the History-Entry</param>
        /// <returns>The History-Entry</returns>
        public UserHistoryEntry GetUserHistoryEntry(int entryId)
        {
            DataTable dt = _Com.GetHistoryEntry(entryId);

            UserHistoryEntry uhe = new UserHistoryEntry();
            uhe.Id = (int)dt.Rows[0]["id"];
            uhe.UserId = (int)dt.Rows[0]["userid"];
            uhe.ConnectionSettingId = (int)dt.Rows[0]["connectionsettingid"];
            uhe.PointOfTime = (DateTime)dt.Rows[0]["pointoftime"];
            uhe.Name = EnfuseString((string)dt.Rows[0]["name"]);
            uhe.Protocol = (string)dt.Rows[0]["protocol"];
            uhe.ConnectionId = (int)dt.Rows[0]["connectionid"];
            uhe.Port = (int)dt.Rows[0]["port"];
            uhe.Host = EnfuseString((string)dt.Rows[0]["host"]);

            

            return (uhe);
        }

        /// <summary>
        /// Deletes the given connectionId from the history
        /// </summary>
        /// <param name="connectionSettingId"></param>
        public void DeleteUserHistoryEntry(long connectionSettingId)
        {
            _Com.DeleteUserHistoryConnectionSettingId(connectionSettingId);
        }

        /// <summary>
        /// Gets all VPN connectionsnames and its IDs
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetUserVpnConnectionsShort()
        {
            var retVal = new Dictionary<int, string>();

            DataTable dt = GetUserVpnConnections();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                retVal.Add(Convert.ToInt32(dt.Rows[i]["Id"]), dt.Rows[i]["name"].ToString());
            }

            return(retVal);
        }

        /// <summary>
        /// Gets all VPNs of a User
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserVpnConnections()
        {
            return(_Com.GetUserVpnConnections(GetUserId()));
        }

        /// <summary>
        /// Gets the Definde VPN
        /// </summary>
        /// <param name="id">ID of the VPN</param>
        /// <returns></returns>
        public DataTable GetUserVpnConnection(int id)
        {
            return(_Com.GetUserVpnConnection(id));
        }

        /// <summary>
        /// Adds a new VPN
        /// </summary>
        /// <param name="type">Type of the VPN</param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <param name="parameter4"></param>
        /// <param name="parameter5"></param>
        /// <param name="parameter6"></param>
        /// <param name="parameter7"></param>
        /// <param name="parameter8"></param>
        /// <param name="parameter9"></param>
        /// <param name="parameter10"></param>
        /// <param name="name">Name of the VPN</param>
        /// <returns></returns>
        public long AddUserVpnConnection(int type, string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10, string name)
        {
            return(_Com.AddUserVpnConnection(
                GetUserId(),
                type,
                parameter1,
                parameter2,
                parameter3,
                parameter4,
                parameter5,
                parameter6,
                parameter7,
                parameter8,
                parameter9,
                parameter10,
                name));
        }

        /// <summary>
        /// Deletes the VPN with the given ID
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUserVpnConnection(int id)
        {
            _Com.DeleteUserVpnConnection(id);
        }

        /// <summary>
        /// Edit the parameter of an existing VPN
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <param name="parameter4"></param>
        /// <param name="parameter5"></param>
        /// <param name="parameter6"></param>
        /// <param name="parameter7"></param>
        /// <param name="parameter8"></param>
        /// <param name="parameter9"></param>
        /// <param name="parameter10"></param>
        /// <param name="name"></param>
        public void EditUserVpnConnection(int id, int type, string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10, string name)
        {
            _Com.EditUserVpnConnection(
                id,
                type,
                parameter1,
                parameter2,
                parameter3,
                parameter4,
                parameter5,
                parameter6,
                parameter7,
                parameter8,
                parameter9,
                parameter10,
                name);
        }





        /// <summary>
        /// Sets data to the database
        /// </summary>
        /// <param name="data">The serialized data to store in the database</param>        
        public long AddData(byte[] data)
        {
            return (_Com.AddData(data));
        }

        /// <summary>
        /// Sets data to the database
        /// </summary>
        /// <param name="data">The serialized data to store in the database</param>
        /// <param name="currentId">The current ID of the DataSet to overwrite</param>
        public void SetData(byte[] data, int currentId)
        {
            _Com.SetData(data, currentId);
        }

        /// <summary>
        /// Gets stored data
        /// </summary>
        /// <param name="id">The Data-ID to get</param>
        /// <returns>The stored serialialized dataset</returns>
        public byte[] GetData(int id)
        {
            return (_Com.GetData(id));
        }




        /// <summary>
        /// Exports the Database in a singe file (only Superadmins!)
        /// </summary>
        /// <returns>Returns the absolute path of the Databaseexport</returns>
        public string ExportDatabase()
        {
            if (GetUserSuperadmin())
            {
                //SQLite returns the Database-File; Other Databases has to do an Export of the Database and store it somewhere and return the path
                File.Copy(_Com.DBName, _Com.DBName + ".export",true);

                return (_Com.DBName + ".export");
            }
            
            return ("");
        }

        /// <summary>
        /// Imports the Database, that is Exported previously
        /// </summary>        
        public void ImportDatabase()
        {
            ImportDatabase(_Com.DBName + ".export");
        }

        /// <summary>
        /// Imports the Database, that was Exporded previously
        /// </summary>
        /// <param name="path">The Path of the Database-File</param>
        public void ImportDatabase(string path)
        {
            if (GetUserSuperadmin())
            {
                if (File.Exists(_Com.DBName + ".export"))
                {
                    File.Copy(_Com.DBName, _Com.DBName + ".backup");
                    File.Copy(_Com.DBName + ".export", _Com.DBName);
                    File.Delete(_Com.DBName + ".backup");
                }
            }
        }

        /// <summary>
        /// Deletes the Exportfile of the Database
        /// </summary>
        public void DeleteExportFile()
        {
            if (File.Exists( _Com.DBName + ".export"))
                File.Delete( _Com.DBName + ".export");
        }

        /// <summary>
        /// Checks if a Column exists in a defined table
        /// </summary>
        /// <param name="columnname">The Columnname to check</param>
        /// <param name="tablename">The Table where the column can be located</param>
        /// <returns>yes/no</returns>
        public bool CheckColumnExists(string columnname, string tablename)
        {
            DataTable infos = _Com.GetTableInfos(tablename);
            foreach (DataRow dr in infos.Rows)
            {
                if (dr["name"].ToString() == columnname)
                {
                    return (true);
                }
            }
            return (false);
        }





        /// <summary>
        /// Adds a new Filter to the database
        /// </summary>
        /// <param name="name">Name of the new Filterset</param>
        /// <param name="filterList">All Filters that contains this Filterset</param>
        /// <returns>the ID of the new Filterset</returns>
        public long AddFilter(string name, List<FilterClass> filterList)
        {
            return (AddFilter(name, filterList, 0, false));
        }

        /// <summary>
        /// Adds a new Filter to the database
        /// </summary>
        /// <param name="name">Name of the new Filterset</param>
        /// <param name="filterList">All Filters that contains this Filterset</param>
        /// <param name="parent"></param>
        /// <param name="hide"></param>
        /// <returns>the ID of the new Filterset</returns>
        public long AddFilter(string name, List<FilterClass> filterList, long parent, bool hide)
        {
            long filterSetId = _Com.AddFilterSet(DefuseString(name), GetUserId(), false, parent, hide);

            //Add Collections aka SubFilterSet
            foreach (FilterClass aFilter in filterList)
            {
                if (aFilter.ConditionType == FilterType.Collection)
                {
                    AddFilter("Collection" + filterSetId, aFilter.SubConditions, filterSetId, hide);
                }
            }
            
            //Add Filters
            foreach (FilterClass aFilter in filterList)
            {
                _Com.AddFilter(
                    aFilter.ConditionType.ToString(),
                    aFilter.IsNot,
                    aFilter.IsOr,
                    aFilter.IsLike,
                    SerializeObject(aFilter.Value),
                    aFilter.Description,
                    filterSetId);
            }

            return (filterSetId);
        }

        /// <summary>
        /// Deletes a FilterSet, all Filters and all SubFilterSets/Collections
        /// </summary>
        /// <param name="id">ID of the filterset to delete</param>
        public void DeleteFilterSet(long id)
        {
            DataTable toDelete = _Com.GetFilterSet(id);

            //Delete SubFilterSets aka Collections
            for (int i = 0; i < toDelete.Rows.Count; i++)
            {
                if ((long)toDelete.Rows[i]["parent"] != 0)
                {
                    DeleteFilterSet((long)toDelete.Rows[i]["parent"]);
                }
            }

            //Delete this FilterSet
            _Com.DeleteFilterSet(id);
        }        

        public void ModifyFilter(long id, List<FilterClass> filterList)
        { 
            
        }

        /// <summary>
        /// Returns a Dictionary with all FilterSets (ID, Name)
        /// </summary>
        /// <returns></returns>
        public List<FilterSet> GetFilterSets()
        {
            return (GetFilterSets(0));
        }

        /// <summary>
        /// Returns a Dictionary with all FilterSets (ID, Name)
        /// </summary>
        /// <returns></returns>
        public List<FilterSet> GetFilterSets(long id)
        {
            DataTable dt = _Com.GetFilterSets(GetUserId(), id);
            List<FilterSet> ret = new List<FilterSet>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FilterSet fS = new FilterSet();
                fS.Id = Convert.ToInt64(dt.Rows[i]["id"]);
                fS.Title = EnfuseString(dt.Rows[i]["title"].ToString());
                fS.Parent = Convert.ToInt64(dt.Rows[i]["parent"]);
                fS.Hide = Convert.ToBoolean(dt.Rows[i]["hide"]);

                DataTable dtFilter = _Com.GetFilter(fS.Id);
                for (int j = 0; j < dtFilter.Rows.Count; j++)
                {
                    FilterType fT = FilterType.Name;
                    switch (dtFilter.Rows[j]["condition"].ToString())
                    {
                        case "Collection": fT = FilterType.Collection; break;
                        case "Connection": fT = FilterType.Connection; break;
                        case "Credential": fT = FilterType.Credential; break;
                        case "Description": fT = FilterType.Description; break;
                        case "Folder": fT = FilterType.Folder; break;
                        case "Host": fT = FilterType.Host; break;
                        case "Name": fT = FilterType.Name; break;
                        case "OperatingSystem": fT = FilterType.OperatingSystem; break;
                        case "Port": fT = FilterType.Port; break;
                        case "Protocol": fT = FilterType.Protocol; break;
                        case "ProtocolSetting": fT = FilterType.ProtocolSetting; break;
                        case "Public": fT = FilterType.Public; break;
                    }

                    FilterClass fC = new FilterClass(fT);
                    fC.IsNot = Convert.ToBoolean(dtFilter.Rows[j]["isnegative"]);
                    fC.IsOr = Convert.ToBoolean(dtFilter.Rows[j]["isor"]);
                    fC.IsLike = Convert.ToBoolean(dtFilter.Rows[j]["islike"]);
                    fC.Value = DeserializeObject((byte[])dtFilter.Rows[j]["phrase"]);
                    fC.Id = Convert.ToInt64(dtFilter.Rows[j]["id"]);
                    fC.Description = EnfuseString(dtFilter.Rows[j]["description"].ToString());

                    fS.Filter.Add(fC);
                }


                ret.Add(fS);
            }

            return (ret);
        }

        /// <summary>
        /// Get the ID and Name of all User-Filtersets
        /// </summary>
        /// <returns></returns>
        public List<FilterSet> GetFilterSetBasics()
        {
            DataTable dtFilter = _Com.GetFilterSetsBasic(GetUserId());
            List<FilterSet> ret = new List<FilterSet>();

            for (int i = 0; i < dtFilter.Rows.Count; i++)
            {
                FilterSet fS = new FilterSet();
                fS.Id =Convert.ToInt64(dtFilter.Rows[i]["id"]);
                fS.Title = EnfuseString(dtFilter.Rows[i]["title"].ToString());

                ret.Add(fS);
            }

            return (ret);
        }

        /// <summary>
        /// Modifies a FilterSet and its included Filters
        /// </summary>
        /// <param name="fS"></param>
        public void ModifyFilterSet(FilterSet fS)
        {
            _Com.ModifyFilterSet(fS.Id, DefuseString(fS.Title), fS.Hide);

            //Get all currently connected FilterIds to check if there are some deleted Filters
            DataTable dtFilterIds = _Com.GetFilterIds(fS.Id);
            List<long> filterIds = new List<long>();
            foreach (DataRow dR in dtFilterIds.Rows)
                filterIds.Add(Convert.ToInt64(dR["id"]));

            foreach (FilterClass fC in fS.Filter)
            { 
                //Check, if this filter is added, removed or modified
                if (fC.Id == 0) //new
                {
                    //if (fC.ConditionType == FilterType.Collection)
                    //    fC.Value = ((FilterSet)fC.Value).Id;

                    //Add new Filters
                    _Com.AddFilter(
                    fC.ConditionType.ToString(),
                    fC.IsNot,
                    fC.IsOr,
                    fC.IsLike,
                    SerializeObject(fC.Value),
                    DefuseString(fC.Description),
                    fS.Id);
                }
                else if (filterIds.Contains(fC.Id)) //It is modified
                {
                    filterIds.Remove(fC.Id); //Remove, so all Ids that remain at the end are removed filters

                    //if (fC.ConditionType == FilterType.Collection)
                    //    fC.Value = ((FilterSet)fC.Value).Id;

                    //Modify existing Filters
                    _Com.ModifyFilter(
                        fC.ConditionType.ToString(),
                        fC.IsNot,
                        fC.IsOr,
                        fC.IsLike,
                        SerializeObject(fC.Value),
                        DefuseString(fC.Description),
                        fS.Id,
                        fC.Id);
                }
            }

            //Delete removed Filters
            foreach (long delId in filterIds)
            {
                _Com.DeleteFilter(delId);
            }
        }

        /// <summary>
        /// Gets all Connections, that apply to the filterset - NEEDS OPTIMISATION
        /// </summary>
        /// <param name="fS"></param>
        /// <param name="protocolNames">A List of Protocol-Identifiers that are available.</param>
        /// <returns></returns>
        public List<ConnectionHost> GetFilterResult(FilterSet fS, List<string> protocolNames)
        {
            //Get the Results, that belongs to the ProtocolSettings
            var pCmd = new List<List<object>>();
            var subConnections = new List<ConnectionHost>(); //Contains all Results of Sub-FilterSets
            var hasSubFilterSets = false; //If there is a Collection-Filter, this will be true

            foreach (var fC in fS.Filter)
            {
                //Flags for 2nd List-Entry:
                //0 = AND
                //1 = OR
                //1048576 = NOT
                //2097152 = LIKE

                //[0] = The Value, that will be filtered for
                //[1] = The operation (see above)
                //[2] = The name of the column [0] will be applied on
                var pCmdValue = new List<object>(3) { fC.Value, (int)fC.Combination + (int)fC.Operation, "" };

                //Set the Columnname and DefuseStrings or get ItemIds to make them compareable
                switch (fC.ConditionType)
                {
                    // ########## Properties of Connections ##########
                    case FilterType.Host:                        
                        pCmdValue[0] = DefuseString(pCmdValue[0].ToString());
                        pCmdValue[2] = "host";
                        pCmd.Add(pCmdValue);
                        break;
                    case FilterType.Name:
                        pCmdValue[0] = DefuseString(pCmdValue[0].ToString());
                        pCmdValue[2] = "name";
                        pCmd.Add(pCmdValue);
                        break;
                    case FilterType.Description:
                        pCmdValue[0] = DefuseString(pCmdValue[0].ToString());
                        pCmdValue[2] = "description";
                        pCmd.Add(pCmdValue);
                        break;
                    case FilterType.Folder:
                        fC.ValueIdSource = GetFolders();
                        pCmdValue[0] = fC.ValueId;
                        pCmdValue[2] = "folder";
                        pCmd.Add(pCmdValue);
                        break;
                    case FilterType.OperatingSystem:
                        fC.ValueIdSource = GetOperatingSystemList();
                        pCmdValue[0] = fC.ValueId;
                        pCmdValue[2] = "os";
                        pCmd.Add(pCmdValue);
                        break;
                    case FilterType.Public:
                        pCmdValue[2] = "public";
                        pCmd.Add(pCmdValue);
                        break;
                    case FilterType.Connection:
                        throw new Exception("when this is used? Does it work without the ValueIdSource?");
                        //fC.ValueIdSource = GetConnections();
                        pCmdValue[0] = fC.ValueId;
                        pCmdValue[2] = "connection";
                        pCmd.Add(pCmdValue);
                        break;

                    // #######  Properties of ProtocolSettings ##########
                    case FilterType.Port:
                        //[0] = Port is already set
                        pCmdValue[2] = "port";
                        pCmd.Add(pCmdValue);
                        break;

                    case FilterType.Credential:
                        fC.ValueIdSource = GetUserCredentialsAll();
                        pCmdValue[0] = fC.ValueId;
                        pCmdValue[2] = "credential";
                        pCmd.Add(pCmdValue);
                        break;

                    case FilterType.Protocol:
                        fC.ValueIdSource = protocolNames;
                        pCmdValue[0] = fC.ValueId;
                        pCmdValue[2] = "protocol";
                        pCmd.Add(pCmdValue);
                        break;


                    // ########## Properties of ProtocolOptions ##########
                    case FilterType.ProtocolSetting:
                        throw new Exception("todo?");
                        break;


                    // ########## Options for Collection ##########
                    case FilterType.Collection:
                        //Add all Connections of this result to 
                        if (hasSubFilterSets == false) //If it is the first subFilterSet
                            subConnections = GetFilterResult(GetFilterSets(Convert.ToInt64(fC.Value))[0], protocolNames);
                        else if (subConnections.Count > 0) //Get the intersection of this two Lists, if the first filterset had any connections as a result - it isn't required to get an intersection of an emtpy list
                            subConnections = connectionHostIntersection(subConnections, GetFilterResult(GetFilterSets(Convert.ToInt64(fC.Value))[0], protocolNames));


                        //subConnections.AddRange(GetFilterResult(GetFilterSets(Convert.ToInt64(fC.Value))[0]));

                        hasSubFilterSets = true;
                        break;
                }
            }

            DataTable dT = _Com.GetFilterResults(pCmd, GetUserId());
            List<ConnectionHost> filterResult = getConnectionsHelper(dT);

            //If there was a Collection as a Filter, compare the results of this filterset and the subfiltersets
            if (hasSubFilterSets)
            {
                filterResult = connectionHostIntersection(filterResult, subConnections);
            }

            return (filterResult);
        }

        /// <summary>
        /// Gets the intersection of two ConnectionHost-Lists
        /// </summary>
        /// <param name="chList1"></param>
        /// <param name="chList2"></param>
        /// <returns></returns>
        private List<ConnectionHost> connectionHostIntersection(List<ConnectionHost> chList1, List<ConnectionHost> chList2)
        {
            //Remove every entry from filterResult, that does not appear in subConnections
            for (int i = 0; i < chList1.Count; i++)
            {
                bool exists = false;

                //Check if the subfilter also includes this filter
                foreach (ConnectionHost SubCh in chList2)
                {
                    if (SubCh.ID == chList1[i].ID)
                    {
                        exists = true;
                        break;
                    }
                }

                //If the currently checked connectionhost does not matches to the subfilters: remove
                if (exists == false)
                {
                    chList1.RemoveAt(i);
                    i--;
                }
            }

            return (chList1);
        }

        #region Schema-Management

        public List<string> SchemaGetTableNames()
        {
            var dt = _Com.SchemaGetTableNames();
            var ret = new List<string>();

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                ret.Add(dt.Rows[i][0].ToString());
            }

            return (ret);
        }

        public DataTable SchemaGetTable(string table)
        {
            //PRAGMA table_info(filter)
            return _Com.SchemaGetTableDetails(table);
        }


        #region Convert SQLite components to .Net Components

        private readonly Dictionary<string, Type> _SchemaDatatypeDictionarySqLiteToNet = new Dictionary<string, Type>
                                                                               {
                                                                                   {"SMALLINT", typeof (Int16)},
                                                                                   {"INT16", typeof (Int16)},
                                                                                   {"INTEGER",typeof (Int32) },
                                                                                   {"INT",typeof (Int32) },
                                                                                   {"MEDIUMINT",typeof (Int32) },
                                                                                   {"INT32",typeof (Int32) },
                                                                                   {"BIGINT",typeof (Int64) },
                                                                                   {"INT64",typeof (Int64) },
                                                                                   {"BYTE", typeof (Byte)  },
                                                                                   {"INT8", typeof (Byte)  },
                                                                                   {"TINYINT", typeof (Byte)  },
                                                                                   {"TEXT", typeof (String)},
                                                                                   {"STRING", typeof (String)},
                                                                                   {"BLOB",typeof (Byte[]) },
                                                                                   {"NONE",typeof (Byte[]) },
                                                                                   {"REAL",typeof (Double) },
                                                                                   {"DOUBLE",typeof (Double) },
                                                                                   {"NUMERIC",typeof (Decimal) },
                                                                                   {"DECIMAL",typeof (Decimal) },
                                                                                   {"FLOAT", typeof(Single)},
                                                                                   {"SINGLE", typeof(Single)},
                                                                                   {"BIT",typeof (Boolean) },
                                                                                   {"BOOLEAN",typeof (Boolean) },
                                                                                   {"DATETIME",typeof (DateTime) },
                                                                                   {"DATE",typeof (DateTime) },
                                                                                   {"TIME",typeof (DateTime) },
                                                                                   {"NULL",typeof (DBNull) },
                                                                               };

        /// <summary>
        /// Converts a SQLite-Datatype to .Net-Datatype
        /// </summary>
        /// <param name="sqLiteType"></param>
        /// <returns></returns>
        public Type SchemaConvertDatatype(object sqLiteType)
        {
            var strType = sqLiteType.GetType().ToString().ToUpper();
            strType = strType.Replace("SYSTEM.", "");

            return (_SchemaDatatypeDictionarySqLiteToNet[strType]);
        }

        public bool SchemaConvertBoolean(object sqLiteExpression)
        {
            return (sqLiteExpression.ToString() == "1");
        }

        #endregion

        #region Convert .Net Components to SQLite
        //Source for Dictionary: https://www.sqlite.org/datatype3.html (Part 2.2)
        private readonly Dictionary<Type, string> _SchemaDatatypeDictionaryNetToSqLite = new Dictionary<Type, string>
                                                                               {
                                                                                   {typeof (Int16), "SMALLINT"},
                                                                                   {typeof (Int32), "INTEGER"},
                                                                                   {typeof (Int64), "BIGINT"},
                                                                                   {typeof (Byte),  "BYTE"},
                                                                                   {typeof (String), "TEXT"},
                                                                                   {typeof (Byte[]), "BLOB"},
                                                                                   {typeof (Double), "REAL"},
                                                                                   {typeof (Single), "REAL"},
                                                                                   {typeof (Decimal), "NUMERIC"},
                                                                                   {typeof (Boolean), "BIT"},
                                                                                   {typeof (DateTime), "DATETIME"},
                                                                                   {typeof (DBNull), "NULL"},
                                                                               };

        /// <summary>
        /// Converts a SQLite-Datatype to .Net-Datatype
        /// </summary>
        /// <param name="dotnetType"></param>
        /// <returns></returns>
        public string SchemaConvertDatatypeToDatabase(Type dotnetType)
        {
            return (_SchemaDatatypeDictionaryNetToSqLite[dotnetType]);
        }

        /// <summary>
        /// COnverts data to the Type, that hte Database need
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SchemaConvertDataToDatabase(object data)
        {
            if (data is Int16 ||
                data is Int32 ||
                data is Int64 ||
                data is Byte ||
                data is Boolean ||
                data is Double ||
                data is Single ||
                data is Decimal)
                return (data.ToString());
            else if (data is String ||
                     data is DateTime)
                return ("'" + data + "'");
            else if (data is DBNull)
                return ("NULL");

            return ("");
        }
        #endregion

        /// <summary>
        /// Adds a table to the Database (including columns)
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool SchemaAddTable(DatabaseTableDefinition table)
        {
            try
            {
                var sqlColumns = "";

                foreach (var aColumn in table.Columns)
                {
                    //Set name and type
                    var colDefinition = string.Format("{0} {1}", aColumn.Name, _SchemaDatatypeDictionaryNetToSqLite[aColumn.Type]);
                    
                    //SQLite only allows INTEGER to use AUTOINCREMENT
                    if (aColumn.Autoincrement)
                        colDefinition = string.Format("{0} {1}", aColumn.Name, "INTEGER");

                    //set Not null, if required
                    if (aColumn.NotNull)
                        colDefinition += " NOT NULL";

                    //Set primary Key, if required
                    if (aColumn.IsPrimaryKey)
                        colDefinition += " PRIMARY KEY";

                    //Set Autoincrement, if set
                    if (aColumn.Autoincrement)
                        colDefinition += " AUTOINCREMENT ";

                    //Set default value, if required
                    if (aColumn.Default != null)
                        colDefinition += " DEFAULT " + SchemaConvertDataToDatabase(aColumn.Default);

                    sqlColumns += colDefinition + ", ";
                }

                sqlColumns = sqlColumns.Substring(0, sqlColumns.Length - 2); //remove the last ", "
                
                Logger.Log(LogEntryType.Info, "Adding table " + table.Name + " with using SQL-Command: " + sqlColumns, "StorageCore");
                _Com.SchemaAddTable(table.Name, sqlColumns);
                return (true);
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Unable to add table " + table.Name, ea, "StorageCore");
                return (false);
            }
        }

        /// <summary>
        /// Adds a Column to a table
        /// </summary>
        /// <param name="column"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool SchemaAddColumn(DatabaseColumnDefinition column, string tableName)
        {
            _Com.SchemaAddColumn(
                tableName, 
                column.Name, 
                SchemaConvertDatatypeToDatabase(column.Type), 
                column.NotNull,
                column.IsPrimaryKey, 
                column.Default != null ? SchemaConvertDataToDatabase(column.Default) : ""
                );

            return (true);
        }

        /// <summary>
        /// Adds a Row to the Database
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool SchemaAddRow(DatabaseContentDefinition content)
        {
            var colNames = "";
            var values = "";

            foreach (var aContent in content.RowContent)
            {
                colNames += aContent.Key + ", ";
                values += aContent.Value + ", ";
            }

            colNames = colNames.Substring(0, colNames.Length - 2);
            values = values.Substring(0, values.Length - 2);

            _Com.SchemaAddRow(colNames, values, content.Tablename);
            
            return (true);
        }

        /// <summary>
        /// Updates a Row to the Database. The first entry in the dictionary is the where-condition
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool SchemaUpdateRow(DatabaseContentDefinition content)
        {
            var changeStr = "";
            var whereStr = "";
            foreach (var aContent in content.RowContent)
            {
                if (whereStr == "")
                    whereStr = aContent.Key + "=" + aContent.Value;
                else
                    changeStr += aContent.Key + "=" + aContent.Value + ", ";
                
            }

            changeStr = changeStr.Substring(0, changeStr.Length - 2);

            _Com.SchemaUpdateRow(changeStr, whereStr, content.Tablename);

            return (true);
        }


        #endregion


        /// <summary>
        /// Resets the sorting-Indexes
        /// </summary>
        /// <returns></returns>
        public void ResetSorting()
        {
            try
            {
                DataTable allFolder = _Com.GetDatabaseTableContent("folder");
                foreach (DataRow dRFolder in allFolder.Rows)
                {
                    long folderId = Convert.ToInt64(dRFolder["id"]);

                    //Get all Connections in this folder already sorted by sortorder
                    DataTable allConnections = _Com.GetConnectionsInFolder(folderId);
                    for (int i = 0; i < allConnections.Rows.Count; i++)
                    {
                        _Com.ModifyConnectionSortOrder(Convert.ToInt64(allConnections.Rows[i]["id"]), i);
                    }
                }

                _Com.AddSetting("db_resetsorting", "1");
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                
            }
        }


        #region SmartStorage
        private List<string> smartStorageExistingValues = new List<string>();

        /// <summary>
        /// Creates a Table, if it doesn't exists for the SmartStorage-System
        /// </summary>
        /// <param name="tablename">The name of the new table</param>
        /// <returns></returns>
        public bool SmartStorageCreateTable(string tablename)
        {
            return(_Com.SmartStorageCreateTable(tablename));
        }

        /// <summary>
        /// Write data to the Storage.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public bool SmartStorageWriteValue(string name, byte[] data, string tablename)
        {
            //Add value if not existent
            if (smartStorageExistingValues.Contains(name) == false)
            {
                if (_Com.SmartStorageValueExists(name, tablename, _Com.GetUserId()) == false)
                {
                    _Com.SmartStorageCreateValue(name, tablename, _Com.GetUserId());                    
                }
                smartStorageExistingValues.Add(name);
            }

            return (_Com.SmartStorageWriteValue(name, data, tablename, _Com.GetUserId()));
        }

        /// <summary>
        /// Reads a value for SmartStorage-Plugin
        /// </summary>
        /// <param name="name">The Name of the value to get</param>
        /// <param name="tablename">The tablename for the SmartStorage-Plugin</param>
        /// <returns>The serialized object</returns>
        public byte[] SmartStorageReadValue(string name, string tablename)
        {
            return (_Com.SmartStorageReadValue(name, tablename, _Com.GetUserId()));
        }

        /// <summary>
        /// Deletes a already stored value of a SmartStorage-Plugins
        /// </summary>
        /// <param name="name">Name of the value</param>
        /// <param name="tablename">Table of the SmartStorage-Plugin</param>
        /// <returns></returns>
        public bool SmartStorageDeleteValue(string name, string tablename)
        {
            smartStorageExistingValues.Remove(name);
            return (_Com.SmartStorageDeleteValue(name, tablename, _Com.GetUserId()));
        }
        #endregion

        #region Helper
        #region Defusing/Enfusing String
        /// <summary>
        /// Removes Special-Signs from strings, that can manupulate SQL-Syntax
        /// </summary>
        /// <param name="toDefuse">Original String</param>
        /// <returns>Defused String</returns>
        public string DefuseString(string toDefuse)
        {
            if (toDefuse == null)
                return (null);

            //If it is just A-Z, a-z, 0-9 or space => the string is OK
            if (IsAlphaNumericString(toDefuse))
                return(toDefuse);

            string defusedString = "";

            //Check every sign
            for (int i = 0; i < toDefuse.Length; i++)
            {
                //If the sign is a specialsign
                if (IsAlphaNumericString(toDefuse.Substring(i, 1)) == false)
                {
                    defusedString += "-#" + StringToHexString(toDefuse.Substring(i, 1)) + "#";
                }
                else //If it is Alphanumeric
                {
                    defusedString += toDefuse.Substring(i, 1);
                }
            }
            return (defusedString);
        }

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

            for(int i = 0; i < toEnfuse.Length; i++)
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
        /// Checks, of a string just contains of A-Z, a-z and 0-9
        /// </summary>
        /// <param name="strAlphanum">The string to check</param>
        /// <returns>True/False</returns>
        private bool IsAlphaNumericString(string strAlphanum)
        {
            var pattern = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return pattern.IsMatch(strAlphanum.Trim());
        }


        /// <summary>
        /// Converts a String to Hex
        /// </summary>
        /// <param name="asciiString">Original String</param>
        /// <returns>Hex-String</returns>
        private string StringToHexString(string asciiString)
        {
            var sb = new StringBuilder();
            foreach (char c in asciiString)
            {                
                sb.Append(Convert.ToString(c, 16));             
            }
            return sb.ToString();
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
        
        public static byte[] SerializeObject(object toSerialize)
        {
            IFormatter formatter = new BinaryFormatter();
            var stream = new MemoryStream();                    
            formatter.Serialize(stream, toSerialize);

            byte[] ret = stream.ToArray();
            stream.Close();

            return (ret);
        }

        public static object DeserializeObject(byte[] data)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream(data);
            Object obj = formatter.Deserialize(stream);
            stream.Close();

            return (obj);
        }
        #endregion
    }
}
