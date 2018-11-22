using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Database
{

    public partial class TblConnectionHost
    {
        public Int64 Id { get; set; }

        public String Address { get; set; }

        public String Name { get; set; }

        public String Os { get; set; }

        public String Description { get; set; }

    }

    public partial class TblConnectionProtocols
    {
        public Int64 Id { get; set; }

        public Int64? ConnectionHostId { get; set; }

        public Int64? TypeId { get; set; }

        public Int64? Port { get; set; }

        public Int64? CredentialId { get; set; }

    }

    public partial class TblConnectionSettings
    {
        public Int64? ConnectionId { get; set; }

        public String Setting { get; set; }

        public String Value { get; set; }

    }

    public partial class TblCredentials
    {
        public Int64 Id { get; set; }

        public String Source { get; set; }

        public String SourceIdentifier { get; set; }

    }
}
