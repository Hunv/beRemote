using System;

namespace beRemote.Core.Definitions.Classes
{
    public class UserHistoryEntry
    {
        int _Id = 0;
        int _UserId = 0;
        int _ConnectionSettingId = 0;
        DateTime _PointOfTime = DateTime.Now;
        string _Name = "unknown";
        string _Protocol = "";
        int _ConnectionId = 0;
        int _Port = 0;
        string _Host = "unknown";

        public int Id
        {
            get { return (_Id); }
            set { _Id = value; }
        }

        public int UserId
        {
            get { return (_UserId); }
            set { _UserId = value; }
        }

        public int ConnectionSettingId
        {
            get { return (_ConnectionSettingId); }
            set { _ConnectionSettingId = value; }
        }

        public int ConnectionId
        {
            get { return (_ConnectionId); }
            set { _ConnectionId = value; }
        }

        public DateTime PointOfTime
        {
            get { return (_PointOfTime); }
            set { _PointOfTime = value; }
        }

        public string Name
        {
            get { return (_Name); }
            set { _Name = value; }
        }

        public string Protocol
        {
            get { return (_Protocol); }
            set { _Protocol = value; }
        }

        public int Port
        {
            get { return (_Port); }
            set { _Port = value; }
        }

        public string Host
        {
            get { return (_Host); }
            set { _Host = value; }
        }
    }
}
