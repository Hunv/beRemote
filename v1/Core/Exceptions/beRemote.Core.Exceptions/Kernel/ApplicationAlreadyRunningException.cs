using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.Exceptions.Kernel
{
    public class ApplicationAlreadyRunningException : KernelException
    {
        public ApplicationAlreadyRunningException() : base(beRemoteExInfoPackage.MinorInformationPackage, "beRemote is already running. Trying to focus it", 20000)
        {
            
        }


        //public ApplicationAlreadyRunningException(beRemoteExInfoPackage info, string message, int eventId) : base(info, message, eventId)
        //{
        //}

        //public ApplicationAlreadyRunningException(beRemoteExInfoPackage info, string message, int eventId, Exception ex) : base(info, message, eventId, ex)
        //{
        //}
    }
}
