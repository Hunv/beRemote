namespace beRemote.Core.Definitions.Classes
{
    public class UserProxySetting
    {
        private System.Net.WebProxy _ConfProxy;
        private int _UserCredentialID = 0;
        private bool _UseSystemSettings = false;

        public System.Net.WebProxy ConfiguredProxy
        {
            get { return _ConfProxy; }
            set { _ConfProxy = value; }
        }

        public int UserCredentialId
        {
            get { return _UserCredentialID; }
            set { _UserCredentialID = value; }
        }

        public bool UseSystemSettings
        {
            get { return _UseSystemSettings; }
            set { _UseSystemSettings = value; }
        }
    }
}
