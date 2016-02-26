namespace beRemote.Core.Definitions.Classes
{
    public class OSVersion
    {
        private int _Id = 0;
        private string _Family = "";
        private string _Distribution = "";
        private string _Version = "";

        public OSVersion(int Id, string Family, string Distribution, string Version)
        {
            _Id = Id;
            _Family = Family;
            _Distribution = Distribution;
            _Version = Version;
        }

        public int getId() { return (_Id); }
        public string getFamily() { return (_Family); }
        public string getDistribution() { return (_Distribution); }
        public string getVersion() { return (_Version); }

        public string DisplayText { get { return (getFamily() + " " + getDistribution() + " " + getVersion()); } }
        public int Id { get { return (_Id); } }
        public string Family { get { return (_Family); } }
        public string Distribution { get { return (_Distribution); } }
        public string Version { get { return (_Version); } }

    }
}
