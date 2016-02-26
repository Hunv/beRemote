namespace beRemote.Core.Definitions.Classes
{
    public class License
    {
        string _Firstname = "";
        string _Lastname = "";
        string _Email = "";
        string _Secret = "";
        int _UserId = 0;

        public License(string Firstname, string Lastname, string Email, string Secret, int UserId)
        {
            _Firstname = Firstname;
            _Lastname = Lastname;
            _Email = Email;
            _Secret = Secret;
            _UserId = UserId;
        }

        public string Firstname
        {
            get { return _Firstname; }
            set { _Firstname = value; }
        }

        public string Lastname
        {
            get { return _Lastname; }
            set { _Lastname = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string Secret
        {
            get { return _Secret; }
            set { _Secret = value; }
        }

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public string getFirstname()
        {
            return (_Firstname);
        }

        public string getLastname()
        {
            return (_Lastname);
        }

        public string getEmail()
        {
            return (_Email);
        }

        public string getSecret()
        {
            return (_Secret);
        }

        public int getUserId()
        {
            return (_UserId);
        }

    }
}
