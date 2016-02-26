using System;
using System.Collections.Generic;
using System.Text;

namespace beRemote.Core.Exceptions.Licensing
{
    public class LicenseException : beRemoteException
    {
        public LicenseException(beRemoteExInfoPackage infoPack, String message) : base(infoPack, message) { }

        public LicenseException(beRemoteExInfoPackage infoPack, String message, Exception innerEx) : base(infoPack, message, innerEx) { }

        public override int EventId
        {
            get { return 101; }
        }
    }
}
