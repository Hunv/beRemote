using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.VendorProtocols.RDP
{
    public class RdpError
    {
        public RdpError(int errorCode, string errorMessage, bool isEstimated)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            IsEstimated = isEstimated;
        }


        /// <summary>
        /// The Error-Code of the RDP-ActiveX-Control
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// The meaning of the Error-Code
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Is this an estimated Error (i.e. a planned disconnect)?
        /// </summary>
        public bool IsEstimated { get; set; }
    }
}
