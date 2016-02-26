using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Services.ServiceLib.Classes.Clients
{
    public class ClientType
    {
        public static List<ClientType> Clients = new List<ClientType>();

        static ClientType ()
        {
            Clients.Add(new ClientType("Debug", "2780692789449aba075a5a5217f16c1e", false));
            Clients.Add(new ClientType("beRemote PluginDirectory Client", "f9e42f36afd16120244e12d41ba38a2c38ec5f02e376f0543d665023667bfa9c01fb34de6b90572636ae9f95727c4a7f9cec325d616ce877d811aa2ef6393db0", false));
        }

        public String Name { get; private set; }
        public String SecretKey { get; private set; }
        [Obsolete]
        public bool Encrypted { get; private set; }

        public ClientType(String name, String secretKey, bool encrypted)
        {
            Name = name;
            SecretKey = secretKey;
            Encrypted = encrypted;
        }
    }
}
