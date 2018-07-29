using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.Exceptions.Definitions
{
    public enum ExceptionUrgency
    {
        /// <summary>
        /// Minor problem. Mostly ignoreable, only verbose logging
        /// </summary>
        MINOR = 0,
        /// <summary>
        /// Major problem. This should be logged!
        /// </summary>
        MAJOR = 1,
        /// <summary>
        /// Significant problem. Should be logged and the user should be notified
        /// </summary>
        SIGNIFICANT = 2,
        /// <summary>
        /// Application needs to be stopped. 
        /// </summary>
        STOP = 99
        
    }
}
