using System;

namespace beRemote.Core.Definitions.Classes
{
    public class UserCredential
    {
        private int _Id;
        private string _Username = String.Empty;
        private byte[] _Password;
        private string _Domain = String.Empty;
        private int _Owner;
        private string _Description = String.Empty;

        public UserCredential(int id, string username, byte[] password, string domain, string description, int owner)
        {
            _Id = id;
            _Username = username;
            _Password = password;
            _Domain = domain;
            _Owner = owner;
            _Description = description;
        }

        [Obsolete("Use the Property instead")]
        public int getId() { return (_Id); }
        [Obsolete("Use the Property instead")]
        public string getUsername() { return (_Username); }
        [Obsolete("Use the Property instead")]
        public byte[] getPassword() { return (_Password); }
        [Obsolete("Use the Property instead")]
        public string getDomain() { return (_Domain); }
        [Obsolete("Use the Property instead")]
        public int getOwner() { return (_Owner); }
        [Obsolete("Use the Property instead")]
        public string getDescription() { return (_Description); }

        public int Id { get { return (_Id); } set { _Id = value; } }
        public string Username { get { return (_Username); } set { _Username = value; } }
        public byte[] Password { get { return (_Password); } set { _Password = value; } }
        public string Domain { get { return (_Domain); } set { _Domain = value; } }
        public int Owner { get { return (_Owner); } set { _Owner = value; } }
        public string Description { get { return (_Description); } set { _Description = value; } }
    }
}
