using System;

namespace beRemote.Core.Definitions.Classes
{
    public class User
    {
        private int _Id;
        private string _Name = "";
        private string _Winname = "";
        private string _LastMachine = "";
        private DateTime _LastLogin;
        private int _LoginCount;
        private DateTime _LastLogout;
        private int _DefaultFolder;
        private string _DefaultProtocol = "";
        private DateTime _Heartbeat;
        private int _Updatemode;
        private UserVisuals _VisualSettings;
        private bool _DeleteQuickConnect = false;

        public User() { }
        public User(int id, string name, string winname, string lastMachine, DateTime lastLogin, int loginCount, DateTime lastLogout, int defaultFolder, string defaultProtocol, DateTime heartbeat, int updatemode, bool deletequickconnect)
        {
            setSettings(id, name, winname, lastMachine, lastLogin, loginCount, lastLogout, defaultFolder, defaultProtocol, heartbeat, updatemode, null, deletequickconnect);
        }
        public User(int id, string name, string winname, string lastMachine, DateTime lastLogin, int loginCount, DateTime lastLogout, int defaultFolder, string defaultProtocol, DateTime heartbeat, int updatemode, UserVisuals visualSettings, bool deletequickconnect)
        {
            setSettings(id, name, winname, lastMachine, lastLogin, loginCount, lastLogout, defaultFolder, defaultProtocol, heartbeat, updatemode, visualSettings, deletequickconnect);
        }

        private void setSettings(int id, string name, string winname, string lastMachine, DateTime lastLogin, int loginCount, DateTime lastLogout, int defaultFolder, string defaultProtocol, DateTime heartbeat, int updatemode, UserVisuals visualSettings, bool deletequickconnect)
        {
            _Id = id;
            _Name = name;
            _Winname = winname;
            _LastMachine = lastMachine;
            _LastLogin = lastLogin;
            _LoginCount = loginCount;
            _LastLogout = lastLogout;
            _DefaultFolder = defaultFolder;
            _DefaultProtocol = defaultProtocol;
            _Heartbeat = heartbeat;
            _Updatemode = updatemode;
            _VisualSettings = visualSettings;
            _DeleteQuickConnect = deletequickconnect;
        }

        [Obsolete("Use Property instead!")]
        public int getId() { return (_Id); }
        [Obsolete("Use Property instead!")]
        public string getName() { return (_Name); }
        [Obsolete("Use Property instead!")]
        public string getWinname() { return (_Winname); }
        [Obsolete("Use Property instead!")]
        public string getLastMachine() { return (_LastMachine); }
        [Obsolete("Use Property instead!")]
        public DateTime getLastLogin() { return (_LastLogin); }
        [Obsolete("Use Property instead!")]
        public int getLoginCount() { return (_LoginCount); }
        [Obsolete("Use Property instead!")]
        public DateTime getLastLogout() { return (_LastLogout); }
        [Obsolete("Use Property instead!")]
        public int getDefaultFolder() { return (_DefaultFolder); }
        [Obsolete("Use Property instead!")]
        public string getDefaultProtocol() { return (_DefaultProtocol); }
        [Obsolete("Use Property instead!")]
        public DateTime getHeartbeat() { return (_Heartbeat); }
        [Obsolete("Use Property instead!")]        
        public int getUpdatemode() { return (_Updatemode); }
        [Obsolete("Use Property instead!")]
        public UserVisuals getVisualSettings()
        {
            if (_Id == 0) return (null);

            return (_VisualSettings);
        }


        #region Properties
        public int Id { get { return _Id; } set { _Id = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Winname { get { return _Winname; } set { _Winname = value; } }
        public string LastMachine { get { return _LastMachine; } set { _LastMachine = value; } }
        public DateTime LastLogin { get { return _LastLogin; } set { _LastLogin = value; } }
        public int LoginCount { get { return _LoginCount; } set { _LoginCount = value; } }
        public DateTime LastLogout { get { return _LastLogout; } set { _LastLogout = value; } }
        public int DefaultFolder { get { return _DefaultFolder; } set { _DefaultFolder = value; } }
        public string DefaultProtocol { get { return _DefaultProtocol; } set { _DefaultProtocol = value; } }
        public DateTime Heartbeat { get { return _Heartbeat; } set { _Heartbeat = value; } }
        public int Updatemode { get { return _Updatemode; } set { _Updatemode = value; } }
        public Boolean DeleteQuickConnect { get { return _DeleteQuickConnect; } set { _DeleteQuickConnect = value; } }
        public UserVisuals VisualSettings
        {
            get { return (_VisualSettings); }
            set { _VisualSettings = value; }
        }
        #endregion

    }
}
