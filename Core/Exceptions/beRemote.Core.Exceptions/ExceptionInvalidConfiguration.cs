using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.Exceptions
{
    public class ExceptionInvalidConfiguration : beRemoteException
    {
        public ExceptionInvalidConfiguration(beRemoteExInfoPackage infoPackage, String message)
            : base(infoPackage, message)
        {

        }

        public ExceptionInvalidConfiguration(beRemoteExInfoPackage infoPackage, String message, Exception innerEx)
            : base(infoPackage, message, innerEx)
        {

        }

        public override int EventId
        {
            get { return 101; }
        }
    }
}
