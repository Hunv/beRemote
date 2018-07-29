using System.Security;

namespace beRemote.Core.Definitions.Classes
{
    public class UserCredentialGridInformation
    {
        readonly UserCredential uC;
        private readonly string _PasswordStr = "";
        private string _KeyImage = "";

        public UserCredentialGridInformation(int id, string username, SecureString password, string domain, string description, int owner)
        {
            _PasswordStr = password.Length == 0 ? "no" : "yes";
            uC = new UserCredential(id, username, new byte[0], domain, description, owner);
        }

        public UserCredentialGridInformation(UserCredential credential)
        {
            _PasswordStr = credential.Password.Length == 0 ? "no" : "yes";
            credential.Password = new byte[0];
            uC = credential;
        }

        public string getPassword() { return (_PasswordStr); }
        public string PasswordStatus { get { return (_PasswordStr); } }

        public string getKeyImage() { return (_KeyImage); }
        public string KeyImage { get { return (_KeyImage); } set { _KeyImage = value; } }


        public int getId() { return (uC.Id); }
        public string getUsername() { return (uC.Username); }
        public string getDomain() { return (uC.Domain); }
        public int getOwner() { return (uC.Owner); }
        public string getDescription() { return (uC.Description); }

        public int Id { get { return (uC.Id); } set { uC.Id = value; } }
        public string Username { get { return (uC.Username); } set { uC.Username = value; } }
        public string Domain { get { return (uC.Domain); } set { uC.Domain = value; } }
        public int Owner { get { return (uC.Owner); } set { uC.Owner = value; } }
        public string Description { get { return (uC.Description); } set { uC.Description = value; } }
    }
}
