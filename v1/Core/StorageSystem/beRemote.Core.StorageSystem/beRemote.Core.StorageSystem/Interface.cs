using System;
using System.Collections.Generic;
using System.Security;
using System.Data;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.Definitions.Classes.UpdateDatabase;

namespace beRemote.Core.StorageSystem.StorageBase
{
    public interface IDbPlugin
    {
        /// <summary>
        /// Gets the Name of the Strage-Wrapper Name
        /// </summary>
        /// <returns>The name of the Wrapper</returns>
        string GetWrapperName();
        /// <summary>
        /// Gets the version of the Database
        /// </summary>
        /// <returns>Version of Databasecontent</returns>
        Int32 GetDatabaseVersion();

        /// <summary>
        /// Gets a Setting from the settings-Table
        /// </summary>
        /// <param name="setting">Name of the setting</param>
        /// <returns>The containing value to the setting</returns>
        string GetSetting(string setting);
        /// <summary>
        /// Sets a global Setting from the settings-Table
        /// </summary>        
        /// <returns>The containing value to the setting</returns>
        void SetSetting(string setting, string value);

        /// <summary>
        /// Sets the SingleUserMode. Can only be triggered once!
        /// </summary>
        /// <param name="enable">Enable SingleUsermode?</param>
        /// <param name="parameter">Parameters, if SingleUserMode is enabled</param>
        void SetSingleUserMode(bool enable, string parameter);

        /// <summary>
        /// Sets the Salt of the current User
        /// </summary>
        /// <param name="hash">The hash of the new salt</param>
        void SetUserSalt1(byte[] hash);

        /// <summary>
        /// Sets the Salt of a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="hash">Hash of the Salt</param>
        void SetUserSalt1(long userId, byte[] hash);

        /// <summary>
        /// Gets the Salt of the current User
        /// </summary>
        /// <returns></returns>
        byte[] GetUserSalt1();

        /// <summary>
        /// Gets the UserSalt of the given User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        byte[] GetUserSalt1(long userId);

        /// <summary>
        /// Sets the Salt of the current User
        /// </summary>
        /// <param name="hash">The hash of the new salt</param>
        void SetUserSalt2(byte[] hash);

        /// <summary>
        /// Sets the Salt of a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="hash">Hash of the Salt</param>
        void SetUserSalt2(long userId, byte[] hash);

        /// <summary>
        /// Gets the Salt of the current User
        /// </summary>
        /// <returns></returns>
        byte[] GetUserSalt2();

        /// <summary>
        /// Gets the UserSalt of the given User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        byte[] GetUserSalt2(long userId);

        /// <summary>
        /// Sets the Salt of the current User
        /// </summary>
        /// <param name="hash">The hash of the new salt</param>
        void SetUserSalt3(byte[] hash);

        /// <summary>
        /// Sets the Salt of a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="hash">Hash of the Salt</param>
        void SetUserSalt3(long userId, byte[] hash);

        /// <summary>
        /// Gets the Salt of the current User
        /// </summary>
        /// <returns></returns>
        byte[] GetUserSalt3();

        /// <summary>
        /// Gets the UserSalt of the given User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        byte[] GetUserSalt3(long userId);


        /// <summary>
        /// Initialisations on Load
        /// </summary>    
        bool InitPlugin();/// <summary>
        /// Sets a (new) Database GUID for this database
        /// </summary>
        /// <param name="overwriteExistingGuid">Should the GUID recreated, if it already exists?</param>
        /// <returns></returns>
        void InitDatabaseGuid(bool overwriteExistingGuid);
        /// <summary>
        /// Get the Database GUID
        /// </summary>
        /// <returns></returns>
        string GetDatabaseGuid();
        
        /// <summary>
        /// Get all DatabaseTables
        /// </summary>
        /// <returns>All Tables of the Database</returns>
        DataTable GetDatabaseTables();
        /// <summary>
        /// Gets the complete content of a Databasetable
        /// </summary>
        /// <param name="table">Tablename to query</param>
        /// <returns>Complete content of the table</returns>
        DataTable GetDatabaseTableContent(string table);
        /// <summary>
        /// Modifies a Cellvalue
        /// </summary>
        /// <param name="table">Tablename</param>
        /// <param name="column">Columnname</param>
        /// <param name="value">Value</param>
        /// <param name="isString">Is the Value a String (add ' ???)</param>
        /// <param name="currentDataRow">A Dictionary, that contains all current Row-Values. Key=columnname, value=cellvalue (with ' if nessessary)</param>
        void ModifyDatabaseTableContent(string table, string column, string value, bool isString, Dictionary<string, string> currentDataRow);



        /// <summary>
        /// Use _UserId for private-usage! Gets the UserId of the current User
        /// </summary>
        /// <returns>The UserId</returns>
        int GetUserId();        
        /// <summary>
        /// Gets the Id of the User "username"
        /// </summary>
        /// <param name="username">The Username (Domain\Username) of the Windows-User</param>
        /// <returns>The UserId</returns>
        int GetUserId(string username);
        /// <summary>
        /// Returns a Dictionary with UserId as Key and Name as Value
        /// </summary>
        /// <returns>Returns a Dictionary with UserId as Key and Name as Value</returns>
        Dictionary<int, string> GetUserList();
        /// <summary>
        /// Changes the UserId, if the password is correct
        /// </summary>
        /// <param name="username">Username (i.e. Domain\Username)</param>
        /// <param name="hashedPassword">hashed Password</param>
        /// <returns>The ID of the User</returns>
        int ChangeUser(string username, byte[] hashedPassword);

        /// <summary>
        /// Adds a new User
        /// </summary>
        /// <param name="username">Windows-Username (i.e. Domain\Username)</param>
        /// <param name="displayname">Displayname in beRemote (i.e. Nickname)</param>
        /// <param name="hashedPassword">3x MD5 salted Password</param>
        /// <returns>UserId of the new User</returns>
        long AddUser(string username, string displayname, byte[] hashedPassword);
        /// <summary>
        /// Changes the Default Folder for a User
        /// </summary>
        /// <param name="userId">ID of the User</param>
        /// <param name="newDefaultFolder">ID of the new Default folder</param>
        void ModifyUserDefaultFolder(long userId, long newDefaultFolder);
        /// <summary>
        /// Gets the Default Folder ID for the CURRENT UserId
        /// </summary>        
        /// <returns>FolderID of the Defaultfolder</returns>
        int GetUserDefaultFolder();
        /// <summary>
        /// Gets the Default Folder ID for the given UserId
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>FolderID of the Defaultfolder</returns>
        int GetUserDefaultFolder(long userId);
        /// <summary>        
        /// Modifies a User; identified by name        
        /// </summary>
        /// <param name="username">Username (Domain\Username)</param>
        /// <param name="name">The new Displayname (i.e. Nickname)</param>
        /// <param name="hashedPassword">The new hashed Password</param>
        void ModifyUser(string username, string name, byte[] hashedPassword);        
        /// <summary>
        /// Modifes a User, identified by ID
        /// </summary>
        /// <param name="userId">The UserId of the User</param>
        /// <param name="name">The new Displayname (i.e. Nickname)</param>
        /// <param name="hashedPassword">The new hashed Password</param>
        void ModifyUser(long userId, string name, byte[] hashedPassword);
        /// <summary>
        /// Modifies the display- and username
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <param name="name">New Displayname</param>
        /// <param name="username">New Username</param>        
        void ModifyUser(long userId, string name, string username);
        /// <summary>
        /// Modifies the Password of a User
        /// </summary>
        /// <param name="userId">The UserId</param>
        /// <param name="hashedPassword">The hashed Password</param>
        void ModifyUserPassword(long userId, byte[] hashedPassword);                
        /// <summary>
        /// Should a User be Superadmin?
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="isSuperadmin">Is Superadmin?</param>
        void ModifyUserSuperadmin(long userId, bool isSuperadmin);        
        /// <summary>
        /// Gets the Usersettings of the current User
        /// </summary>
        /// <returns>All Settings; Column as Key, Value as Value</returns>
        User GetUserSettings();
        /// <summary>
        /// Gets Usersettings by Username except PasswordHash and IsSuperadmin
        /// </summary>
        /// <param name="username">Username (i.e. Domain\Username)</param>
        /// <returns>All Settings; Column as Key, Value as Value</returns>
        User GetUserSettings(string username);        
        /// <summary>
        /// Gets Usersettings by userId except PasswordHash and IsSuperadmin
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>All Settings; Column as Key, Value as Value</returns>
        User GetUserSettings(long userId);

        /// <summary>
        /// Sets a usersetting
        /// </summary>
        /// <param name="settingname">The name of the column</param>
        /// <param name="value">the value</param>
        void SetUserSettings(string settingname, object value);
        /// <summary>
        /// Get if the current user is Superadmin
        /// </summary>        
        /// <returns>yes/no (true/false)</returns>
        bool GetUserSuperadmin();        
        /// <summary>
        /// Get if a User is Superadmin
        /// </summary>
        /// <param name="userId">UserId to check</param>
        /// <returns>yes/no (true/false)</returns>
        bool GetUserSuperadmin(long userId);        
        /// <summary>
        /// Increases the LoginCount of the current user by one
        /// </summary>        
        void UserLoggedIn();        
        /// <summary>
        /// Increases the LoginCount of userId by one
        /// </summary>
        /// <param name="userId">UserId</param>
        void UserLoggedIn(long userId);       
        /// <summary>
        /// Validates the Password for the current User(Takes at least 1 Second!)
        /// </summary>        
        /// <param name="hashedPassword">3xMD5 Password</param>
        /// <returns>matches/not matches (true/false)</returns>
        bool CheckUserPassword(byte[] hashedPassword);        
        /// <summary>
        /// Validates the Password (Takes at least 1 Second!)
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="hashedPassword">3xMD5 Password</param>
        /// <returns>matches/not matches (true/false)</returns>
        bool CheckUserPassword(long userId, byte[] hashedPassword);
        /// <summary>
        /// Modfies the selected default protocol of a user
        /// </summary>        
        /// <param name="newDefaultProtocol">internal Protocol-Name</param>
        void ModifyUserDefaultProtocol(string newDefaultProtocol);
        /// <summary>
        /// Modfies the selected default protocol of a user
        /// </summary>
        /// <param name="userid">ID of the user</param>
        /// <param name="newDefaultProtocol">internal Protocol-Name</param>
        void ModifyUserDefaultProtocol(long userid, string newDefaultProtocol);
        /// <summary>
        /// Gets the default Protocol of the current user
        /// </summary>        
        /// <returns>The internal name of the protocol</returns>
        string GetUserDefaultProtocol();
        /// <summary>
        /// Gets the default Protocol of a user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>The internal name of the protocol</returns>
        string GetUserDefaultProtocol(long userId);
        /// <summary>
        /// Updates the Userheartbeat to the current timestamp
        /// </summary>
        void UpdateUserHeartbeat();
        /// <summary>
        /// Sets the lastlogout-value to the current timestamp
        /// </summary>
        void LogoutUser();
        /// <summary>
        /// Modifies the Updatemode. 0=Stable, 1=Nightly
        /// </summary>
        /// <param name="updatemode">The Mode-ID</param>
        void ModifyUserUpdatemode(int updatemode);
        /// <summary>
        /// Modifies the Updatemode. 0=Stable, 1=Nightly
        /// </summary>
        /// <param name="userId">ID user to modify</param>
        /// <param name="updatemode">The Mode-ID</param>
        void ModifyUserUpdatemode(long userId, int updatemode);
        /// <summary>
        /// Gets a list of usernames, that were online in the heartbeatinterval and not logged out in the heartbeat interval
        /// </summary>
        /// <returns></returns>
        List<string> GetUsersOnline();
        /// <summary>
        /// Get the current users visual settings
        /// </summary>        
        /// <returns></returns>
        UserVisuals GetUserVisuals();
        /// <summary>
        /// Get the users visual settings
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns></returns>
        UserVisuals GetUserVisuals(long userId);
        /// <summary>
        /// Set the UserVisuals-Parameter of the current User
        /// </summary>        
        /// <param name="values">Dictionary with visual settings</param>
        void SetUserVisual(Dictionary<string, object> values);
        /// <summary>
        /// Set the UserVisuals-Parameter
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="values">Dictionary with visual settings</param>
        void SetUserVisual(long userId, Dictionary<string, object> values);


        /// <summary>
        /// Adds or Updates a Setting of a Plugin
        /// </summary>
        /// <param name="settingvalue">The new value of the setting</param>
        /// <param name="settingname">The name of the setting</param>
        /// <param name="connectionSettingId">The ID of the Connectionsetting</param>
        /// <returns>The new ConnectionOptionId (if added) or 0 (if updated)</returns>
        long ModifyConnectionOption(object settingvalue, string settingname, long connectionSettingId);
        /// <summary>
        /// Gets the setting, where the ConnectionSettingId is given; gets private setting, if existing - else public
        /// </summary>
        /// <param name="connectionSettingId">The ConnectionID where the Settingname belongs to</param>
        /// <param name="settingname">The Settingname to get</param>
        /// <returns>Setting</returns>
        object GetConnectionOption(long connectionSettingId, string settingname);
        /// <summary>
        /// Gets all Options related to this ConnectionSetting
        /// </summary>
        /// <param name="connectionSettingId">The ID of the ConnectionSetting</param>
        /// <returns>All Options related to the ConnectionSettingId</returns>
        List<ConnectionProtocolOption> GetConnectionOptions(long connectionSettingId);
        /// <summary>
        /// Deletes a connectionoption
        /// </summary>
        /// <param name="optionId">the OptionID to delete</param>
        void DeleteConnectionOption(long optionId);
        /// <summary>
        /// Adds a new ConnectionSetting
        /// </summary>
        /// <param name="connectionId">The ID of the Connection</param>
        /// <param name="protocol">Protocolname (Pluginname)</param>
        /// <param name="port">The Port where the Protocol connects to</param>
        /// <returns>the Id of the new ConnectionSetting</returns>
        long AddConnectionSetting(long connectionId, string protocol, int port);
        /// <summary>
        /// Adds a new ConnectionSetting
        /// </summary>
        /// <param name="connectionId">The ID of the Connection</param>
        /// <param name="protocol">Protocolname (Pluginname)</param>
        /// <param name="port">The Port where the Protocol connects to</param>
        /// <param name="credentialId">The ID of the used Credential</param>
        /// <returns>the Id of the new ConnectionSetting</returns>
        long AddConnectionSetting(long connectionId, string protocol, int port, int credentialId);
        /// <summary>
        /// Sets the ConnectionSetting; Every Parameter will be set except the ID - this is the identifier
        /// </summary>
        /// <param name="id">The ConnectionID that should get the new settings</param>        
        /// <param name="port">The Port where the Plugin should connect to</param>
        void ModifyConnectionSetting(long id, int port);
        /// <summary>
        /// Sets the ConnectionSetting; Every Parameter will be set except the ID - this is the identifier
        /// </summary>
        /// <param name="id">The ConnectionID that should get the new settings</param>        
        /// <param name="port">The Port where the Plugin should connect to</param>
        /// <param name="credentialId">ID of the used Credential</param>
        void ModifyConnectionSetting(long id, int port, int credentialId);
        /// <summary>
        /// Removes a ConnectionSettingCredential-Value
        /// </summary>
        void DeleteConnectionSettingCredential(long connectionSetting);
        /// <summary>
        /// Resets a Credential-Password. Required on password-resets
        /// </summary>
        /// <param name="id"></param>
        void ResetUserCredentialPassword(long id);
        /// <summary>
        /// Gets a ConnectionSetting
        /// </summary>
        /// <param name="id">The Id of the Connectionsetting</param>
        /// <returns>The Setting with id of a Connection</returns>
        ConnectionProtocol GetConnectionSetting(long id);
        /// <summary>
        /// Gets all Settings(Plugins) for the current user
        /// </summary>        
        /// <returns>List of all ConnectionSettings releated to the ConnectionId</returns>
        List<ConnectionProtocol> GetConnectionSettings();
        /// <summary>
        /// Gets all Settings(Plugins) for a Connection
        /// </summary>
        /// <param name="connectionId">The ID of the Connection</param>
        /// <returns>List of all ConnectionSettings releated to the ConnectionId</returns>
        List<ConnectionProtocol> GetConnectionSettings(long connectionId);
        /// <summary>
        /// Deletes a connectionsetting
        /// </summary>
        /// <param name="settingId">ID of the Setting</param>
        void DeleteConnectionSetting(long settingId);
        /// <summary>
        /// Modifies an existing connection; identified by the ID
        /// </summary>
        /// <param name="id">The ID of the connection to modify</param>
        /// <param name="host">Hostname or IP of the Host</param>
        /// <param name="name">Displayname of the Host</param>
        /// <param name="description">Discription of the host</param>
        /// <param name="os"></param>
        /// <param name="owner"></param>
        /// <param name="isPublic"></param>
        void ModifyConnection(long id, string host, string name, string description, int os, int owner, bool isPublic, int vpn);
        /// <summary>
        /// Modifies the ParentFolder of a connection
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentFolder"></param>
        /// <returns></returns>
        bool ModifyConnection(long id, long parentFolder);
        /// <summary>
        /// Modifies the ParentFolder of a connection
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortOrder"></param>
        /// <param name="sortUp"></param>
        /// <returns></returns>
        bool ModifyConnection(long id, long sortOrder, bool sortUp);
        /// <summary>
        /// Adds a new Connection
        /// </summary>
        /// <param name="host">Hostname or IP-Address of the Host</param>
        /// <param name="name">Displayname of the Host</param>
        /// <param name="description">Description of the Host</param>
        /// <param name="os"></param>
        /// <param name="folderId"></param>
        /// <param name="owner"></param>
        /// <param name="isPublic"></param>
        /// <returns>The id of the new connection</returns>
        long AddConnection(string host, string name, string description, int os, long folderId, int owner, bool isPublic, int vpn);
        /// <summary>
        /// Gets a connection
        /// </summary>
        /// <param name="id">The ID of the Connection</param>
        /// <returns>The Connection Parameters</returns>
        ConnectionHost GetConnection(long id);
        /// <summary>
        /// Returns a Datatable with all Connections
        /// </summary>
        /// <returns>All Connections</returns>
        List<ConnectionHost> GetConnections();
        /// <summary>
        /// Returns a Datatable with all Connections that matches to the given displayname
        /// </summary>
        /// <returns>All Connections</returns>
        List<ConnectionHost> GetConnections(string displayname);
        /// <summary>
        /// Get all Connections, that matching the filters
        /// </summary>
        /// <param name="filterList"></param>
        /// <returns></returns>
        List<ConnectionHost> GetConnections(List<FilterClass> filterList);
        /// <summary>
        /// Returns a Datatable with all Connections
        /// </summary>        
        /// <param name="folderId"></param>
        /// <returns>All Connections</returns>
        List<ConnectionHost> GetConnectionsInFolder(long folderId);
        /// <summary>
        /// Deletes a connection
        /// </summary>
        /// <param name="id">ConnectionId to delete</param>
        void DeleteConnection(long id);


        /// <summary>
        /// Gets a Folder
        /// </summary>
        /// <param name="folderId">ID of the Folder to get</param>
        /// <returns></returns>
        Folder GetFolder(long folderId);
        /// <summary>
        /// Gets all Folders of a user
        /// </summary>        
        /// <returns></returns>
        List<Folder> GetFolders();
        /// <summary>
        /// Checks if a Folder with the given Parameter exists for the current user
        /// </summary>
        /// <param name="foldername">Name of the Folder</param>        
        /// <returns></returns>
        bool GetFolderExists(string foldername);
        /// <summary>
        /// Checks if a Folder with the given Parameter exists
        /// </summary>
        /// <param name="foldername">Name of the Folder</param>
        /// <param name="userId">ID of the Owner</param>
        /// <returns></returns>
        bool GetFolderExists(string foldername, long userId);
        /// <summary>
        /// Gets all Folders in a List for a Text-Based-List
        /// </summary>
        /// <returns>A List of all Folders so long</returns>
        List<Folder> GetSubfolders(List<Folder> returnValue, long parentId, string prefix);
        /// <summary>
        /// Gets all Subfolders from parentId
        /// </summary>
        /// <param name="parentId">The Parent-Folder-Id</param>
        /// <returns>A Datatable with the containing informations</returns>
        List<Folder> GetSubfolders(long parentId);
        /// <summary>
        /// Deletes a Folder
        /// </summary>
        /// <param name="folderId"></param>
        void DeleteFolder(long folderId);
        /// <summary>
        /// Adds a Folder
        /// </summary>
        /// <param name="name">Name of the Folder</param>
        /// <param name="parentId">ID of the parent</param>        
        /// <param name="ispublic">Is it a public folder?</param>
        /// <returns>Added ID</returns>
        long AddFolder(string name, long parentId, bool ispublic);
        /// <summary>
        /// Adds a Folder
        /// </summary>
        /// <param name="name">Name of the Folder</param>
        /// <param name="parentId">ID of the parent</param>
        /// <param name="owner">OwnerID</param>
        /// <param name="ispublic">Is it a public folder?</param>
        /// <returns>Added ID</returns>
        long AddFolder(string name, long parentId, int owner, bool ispublic);
        /// <summary>
        /// Gets the FolderId of a Folder by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetFolderId(string name);
        /// <summary>
        /// Gets the FolderId of a Folder by Name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetFolderId(string name, long userId);
        /// <summary>
        /// Returns if a Folder in an explicific folder exists
        /// </summary>
        /// <param name="foldername"></param>
        /// <param name="userId"></param>
        /// <param name="parentFolderId"></param>
        /// <returns></returns>
        bool GetFolderExists(string foldername, long userId, long parentFolderId);
        /// <summary>
        /// Moves a folder to another folder
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="newParentFolder"></param>
        /// <returns>success/fail</returns>
        bool ModifyFolderParent(long folderId, long newParentFolder);
        /// <summary>
        /// Resort a Folder
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="sortOrder"></param>
        /// <param name="sortUp"></param>
        /// <returns>success/fail</returns>
        bool ModifyFolderSortOrder(long folderId, long sortOrder, bool sortUp);

        /// <summary>
        /// Gets the Information for a specific versionId
        /// </summary>
        /// <param name="versionId">The ID of the versionentry</param>
        /// <returns>Versioninformation</returns>
        OSVersion GetVersion(int versionId);
        /// <summary>
        /// Gets a list of all Versions
        /// </summary>
        /// <returns></returns>
        List<OSVersion> GetVersionlist();


        /// <summary>
        /// Adds new Credentials to the Database for a specific user
        /// </summary>
        /// <param name="username">The Username to login (i.e. Windows-Username)</param>
        /// <param name="password">The Password as a SecureString</param>
        /// <param name="domain">The Domain of the Username, if used</param>
        /// <param name="description"></param>
        /// <returns>The ID of the Added Credentials</returns>
        long AddUserCredentials(string username, byte[] password, string domain, string description);
        /// <summary>
        /// Adds new Credentials to the Database for a specific user
        /// </summary>
        /// <param name="username">The Username to login (i.e. Windows-Username)</param>
        /// <param name="password">The Password as a SecureString</param>
        /// <param name="domain">The Domain of the Username, if used</param>
        /// <param name="description"></param>
        /// <param name="owner">The ID of the User, that owns this credentials</param>
        /// <returns>The ID of the Added Credentials</returns>
        long AddUserCredentials(string username, byte[] password, string domain, string description, int owner);
        /// <summary>
        /// Get the Credentials of a specific ID
        /// </summary>
        /// <param name="id">The ID of the Credential</param>
        /// <returns>Returns the Credentials</returns>
        UserCredential GetUserCredentials(int id);
        /// <summary>
        /// Get the Credentials of current User
        /// </summary>        
        /// <returns>Returns the Credentials</returns>
        List<UserCredential> GetUserCredentialsAll();
        /// <summary>
        /// Get the Credentials of a specific ID
        /// </summary>
        /// <returns>Returns the Credentials</returns>
        List<UserCredential> GetUserCredentialsAll(long userid);
        /// <summary>
        /// Deletes a UserCredential
        /// </summary>
        /// <param name="id">Id of the Credential to delete</param>
        void DeleteUserCredential(int id);
        /// <summary>
        /// Modifies a Credential
        /// </summary>
        /// <param name="id">ID of the Credential</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="domain">Domain</param>
        /// <param name="owner">Owner</param>
        /// <param name="description">Description</param>
        void ModifyUserCredential(int id, string username, byte[] password, string domain, int owner, string description);
        /// <summary>
        /// Modifies a Credential
        /// </summary>
        /// <param name="id">ID of the Credential</param>
        /// <param name="username">Username</param>
        /// <param name="domain">Domain</param>
        /// <param name="owner">Owner</param>
        /// <param name="description">Description</param>
        void ModifyUserCredential(int id, string username, string domain, int owner, string description);
        

        /// <summary>
        /// Enable or Disable the Proxy for a user
        /// </summary>
        /// <param name="enabled"></param>
        void SetUserProxySettings(bool enabled);
        /// <summary>
        /// Enable a Proxy by using System Settings
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="useSystemSettings"></param>
        void SetUserProxySettings(bool enabled, bool useSystemSettings);
        /// <summary>
        /// Set Proxysettings of a User
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="bypasslocal"></param>
        void SetUserProxySettings(string host, int port, bool bypasslocal);
        /// <summary>
        /// Set Proxysettings of a User
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="credentialid"></param>
        /// <param name="bypasslocal"></param>
        void SetUserProxySettings(string host, int port, int credentialid, bool bypasslocal);
        /// <summary>
        /// Get UserProxy-Settings
        /// </summary>
        /// <returns></returns>
        UserProxySetting GetUserProxySettings();


        /// <summary>
        /// Gets a List of all available OperatingSystems
        /// </summary>
        /// <returns></returns>
        List<OSVersion> GetOperatingSystemList();
        /// <summary>
        /// Gets the OSVersion-Element of the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OSVersion GetOperatingSystem(int id);
        /// <summary>
        /// Adds a new Operatingsystem to the list
        /// </summary>
        /// <param name="id">The ID of the OS</param>
        /// <param name="family">The Family</param>
        /// <param name="distribution">The Distribution</param>
        /// <param name="version">The Version </param>
        void AddOperatingSystem(int id, string family, string distribution, string version);
        

        /// <summary>
        /// Get a List of all Licenses
        /// </summary>
        /// <returns></returns>
        List<License> GetLicenses();
        /// <summary>
        /// Get all Licenses
        /// </summary>
        /// <returns>A Table with all licenses</returns>
        List<License> GetUserLicenses(long userId);
        /// <summary>
        /// Adds a License to the Database
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        void AddUserLicense(License license);
        /// <summary>
        /// Delete a License
        /// </summary>
        /// <param name="license"></param>
        void DeleteUserLicense(License license);


                /// <summary>
        /// Adds an Entry to the history
        /// </summary>
        /// <param name="connectionSettingId"></param>
        void AddUserHistoryEntry(long connectionSettingId);
        /// <summary>
        /// Adds an Entry to the history
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionSettingId"></param>
        void AddUserHistoryEntry(long userId, long connectionSettingId);
        /// <summary>
        /// Cleanup Old Userhistoryinformation
        /// </summary>
        /// <param name="historyLimit">The maximum number of history-entries for this user</param>
        void TidyUpUserHistory(int historyLimit);
        /// <summary>
        /// Cleanup Old Userhistoryinformation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="historyLimit">The maximum number of history-entries for this user</param>
        void TidyUpUserHistory(long userId, int historyLimit);
        /// <summary>
        /// Get the complete UserHistory of the current User
        /// </summary>
        /// <returns></returns>
        List<UserHistoryEntry> GetUserHistory();
        /// <summary>
        /// Get the complete UserHistory of a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserHistoryEntry> GetUserHistory(long userId);
        /// <summary>
        /// Get a specified number of History-Entries of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        List<UserHistoryEntry> GetUserHistory(long userId, int count, int offset);
        /// <summary>
        /// Returns all History-Entries of the given Date
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        List<UserHistoryEntry> GetHistoryDate(long userId, DateTime date);
        /// <summary>
        /// Get a specified History-Entry
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        UserHistoryEntry GetUserHistoryEntry(int entryId);

        /// <summary>
        /// Deletes the given connectionId from the history
        /// </summary>
        /// <param name="connectionSettingId"></param>
        void DeleteUserHistoryEntry(long connectionSettingId);


        /// <summary>
        /// Gets all VPNs of a User
        /// </summary>
        /// <returns></returns>
        DataTable GetUserVpnConnections();

        /// <summary>
        /// Get name and ID of VPn Connections
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetUserVpnConnectionsShort();

        /// <summary>
        /// Gets the Definde VPN
        /// </summary>
        /// <param name="id">ID of the VPN</param>
        /// <returns></returns>
        DataTable GetUserVpnConnection(int id);

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
        long AddUserVpnConnection(int type, string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10, string name);

        /// <summary>
        /// Deletes the VPN with the given ID
        /// </summary>
        /// <param name="id"></param>
        void DeleteUserVpnConnection(int id);

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
        void EditUserVpnConnection(int id, int type, string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10, string name);


        /// <summary>
        /// Sets data to the database
        /// </summary>
        long AddData(byte[] data);
        /// <summary>
        /// Sets data to the database
        /// </summary>
        void SetData(byte[] data, int currentId);
        /// <summary>
        /// Sets data to the database
        /// </summary>
        byte[] GetData(int id);




        /// <summary>
        /// Exports the Database in a singe file
        /// </summary>
        /// <returns>Returns the absolute path of the Database</returns>
        string ExportDatabase();
        /// <summary>
        /// Imports the Database, that is Exported previously
        /// </summary>
        /// <returns></returns>
        void ImportDatabase();
        /// <summary>
        /// Imports the Database, that was Exporded previously
        /// </summary>
        /// <param name="path">The Path of the Database-File</param>
        void ImportDatabase(string path);
        /// <summary>
        /// Deletes the Exportfile of the Database
        /// </summary>
        void DeleteExportFile();
        /// <summary>
        /// Checks if a Column exists in a defined table
        /// </summary>
        /// <param name="columnname"></param>
        /// <param name="tablename"></param>
        /// <returns>yes/no</returns>
        bool CheckColumnExists(string columnname, string tablename);




        /// <summary>
        /// Adds a new Filter to the database
        /// </summary>
        /// <param name="name">Name of the new Filterset</param>
        /// <param name="filterList">All Filters that contains this Filterset</param>
        /// <returns>the ID of the new Filterset</returns>
        long AddFilter(string name, List<FilterClass> filterList);

        /// <summary>
        /// Adds a new Filter to the database
        /// </summary>
        /// <param name="name">Name of the new Filterset</param>
        /// <param name="filterList">All Filters that contains this Filterset</param>
        /// <param name="parent"></param>
        /// <param name="hide"></param>
        /// <returns>the ID of the new Filterset</returns>
        long AddFilter(string name, List<FilterClass> filterList, long parent, bool hide);

        /// <summary>
        /// Deletes a FilterSet and all Filters
        /// </summary>
        /// <param name="id">ID of the filterset to delete</param>
        void DeleteFilterSet(long id);

        /// <summary>
        /// Returns a Dictionary with all FilterSets (ID, Name)
        /// </summary>
        /// <returns></returns>
        List<FilterSet> GetFilterSets();

        /// <summary>
        /// Returns a Dictionary with all FilterSets (ID, Name)
        /// </summary>
        /// <returns></returns>
        List<FilterSet> GetFilterSets(long id);

        /// <summary>
        /// Get the ID and Name of all User-Filtersets
        /// </summary>
        /// <returns></returns>
        List<FilterSet> GetFilterSetBasics();

        /// <summary>
        /// Modifies a FilterSet and its included Filters
        /// </summary>
        /// <param name="fS"></param>
        void ModifyFilterSet(FilterSet fS);

        /// <summary>
        /// Gets all Connections, that apply to the filterset
        /// </summary>
        /// <param name="fS"></param>
        /// <param name="protocolNames"></param>
        /// <returns></returns>
        List<ConnectionHost> GetFilterResult(FilterSet fS, List<string> protocolNames);




        List<string> SchemaGetTableNames();

        DataTable SchemaGetTable(string table);

        /// <summary>
        /// Converts a SQLite-Datatype to .Net-Datatype
        /// </summary>
        /// <param name="sqLiteType"></param>
        /// <returns></returns>
        Type SchemaConvertDatatype(object sqLiteType);


        bool SchemaConvertBoolean(object sqLiteExpression);

        bool SchemaAddTable(DatabaseTableDefinition table);

        bool SchemaAddColumn(DatabaseColumnDefinition column, string tableName);

        bool SchemaAddRow(DatabaseContentDefinition content);

        bool SchemaUpdateRow(DatabaseContentDefinition content);


        /// <summary>
        /// Creates a Table, if it doesn't exists for the SmartStorage-System
        /// </summary>
        /// <param name="tablename">The name of the new table</param>
        // <returns></returns>
        bool SmartStorageCreateTable(string tablename);
        /// <summary>
        /// Write data to the Storage.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        bool SmartStorageWriteValue(string name, byte[] data, string tablename);
        /// <summary>
        /// Reads a value for SmartStorage-Plugin
        /// </summary>
        /// <param name="name">The Name of the value to get</param>
        /// <param name="tablename">The tablename for the SmartStorage-Plugin</param>
        /// <returns>The serialized object</returns>
        byte[] SmartStorageReadValue(string name, string tablename);
        /// <summary>
        /// Deletes a already stored value of a SmartStorage-Plugins
        /// </summary>
        /// <param name="name">Name of the value</param>
        /// <param name="tablename">Table of the SmartStorage-Plugin</param>
        /// <returns></returns>
        bool SmartStorageDeleteValue(string name, string tablename);



        /// <summary>
        /// Removes Special-Signs from strings, that can manupulate SQL-Syntax
        /// </summary>
        /// <param name="toDefuse">Original String</param>
        /// <returns>Defused String</returns>
        string DefuseString(string toDefuse);
        /// <summary>
        /// Replaces Special-Signs in strings, that had been removed
        /// </summary>
        /// <param name="toEnfuse">Defused String</param>
        /// <returns>Original String</returns>
        string EnfuseString(string toEnfuse);

        /// <summary>
        /// Resets the sorting-Indexes
        /// </summary>
        /// <returns></returns>
        void ResetSorting();
    }
}
