using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ProtocolSystem.ProtocolBase.Declaration
{
    public enum IconType
    {
        /// <summary>
        /// Small icon (16x16)
        /// </summary>
        SMALL,
        /// <summary>
        /// Medium icon (32x32)
        /// </summary>
        MEDIUM,
        /// <summary>
        /// Large icon (64x64)
        /// </summary>
        LARGE,
        /// <summary>
        /// High definition icon
        /// Will return 128x128 or 256x256 wich ever ist available (256 is prio)
        /// </summary>
        HIGHDEF
    }
}
