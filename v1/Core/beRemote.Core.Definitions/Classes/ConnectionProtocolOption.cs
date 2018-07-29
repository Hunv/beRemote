namespace beRemote.Core.Definitions.Classes
{
    public class ConnectionProtocolOption
    {
        private long _Id = 0;
        private long _ConnectionSettingId = 0;
        private string _Settingname = "";
        private object _Settingvalue = "";

        public ConnectionProtocolOption(long Id, long ConnectionSettingId, string Settingname, object Settingvalue)
        {
            _Id = Id;
            _ConnectionSettingId = ConnectionSettingId;
            _Settingname = Settingname;
            _Settingvalue = Settingvalue;
        }

        public long getId() { return (_Id); }
        public long getConnectionSettingId() { return (_ConnectionSettingId); }
        public string getSettingname() { return (_Settingname); }
        public object getSettingvalue() { return (_Settingvalue); }

    }
}
