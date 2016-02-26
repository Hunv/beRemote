using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ProtocolSystem.ProtocolBase
{
    public class UserCredentials
    {
        private String _domain;
        private String _username;
        private String _password;

        public UserCredentials(String username, String password)
        {
            this._username = username;
            this._password = password;
        }

        public UserCredentials(String domain, String username, String password)
        {
            _domain = domain;
            this._username = username;
            this._password = password;
        }


        public String GetDomain()
        {
            return _domain;
        }
        public String GetUsername()
        {
            return _username;
        }

        public String GetPassword()
        {
            return _password;
        }
    }
}
