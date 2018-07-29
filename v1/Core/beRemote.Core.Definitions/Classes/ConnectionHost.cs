namespace beRemote.Core.Definitions.Classes
{
    public class ConnectionHost
    {
        private long _Id = 0;
        private string _Host = "";
        private string _Name = "";
        private string _Description = "";
        private long _Folder = 0;
        private int _SortOrder = 0;
        private int _OS = 0;
        private int _Owner = 0;
        private bool _IsPublic = false;
        private int _Vpn = 0;

        public ConnectionHost()
        {
        }

        public ConnectionHost(long Id, string Host, string Name, string Description, long Folder, int SortOrder, int OS, int Owner, bool IsPublic, int vpn)
        {
            _Id = Id;
            _Host = Host;
            _Name = Name;
            _Description = Description;
            _Folder = Folder;
            _SortOrder = SortOrder;
            _OS = OS;
            _Owner = Owner;
            _IsPublic = IsPublic;
            _Vpn = vpn;
        }

        public long ID
        {
            get { return (_Id); }
            set { _Id = value; }
        }

        public string Host
        {
            get { return (_Host); }
            set { _Host = value; }
        }

        public string Name
        {
            get { return (_Name); }
            set { _Name = value; }
        }

        public string Description
        {
            get { return (_Description); }
            set { _Description = value; }
        }

        public long Folder
        {
            get { return (_Folder); }
            set { _Folder = value; }
        }

        public int SortOrder
        {
            get { return (_SortOrder); }
            set { _SortOrder = value; }
        }

        public int OS
        {
            get { return (_OS); }
            set { _OS = value; }
        }

        public int Owner
        {
            get { return (_Owner); }
            set { _Owner = value; }
        }

        public bool IsPublic
        {
            get { return (_IsPublic); }
            set { _IsPublic = value; }
        }

        public int Vpn
        {
            get { return (_Vpn); }
            set { _Vpn = value; }
        }
    }
}
