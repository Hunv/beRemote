using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;
using beRemote.GUI.ViewModel.Command;

namespace beRemote.GUI.ViewModel.Worker
{
    public class Protocol
    {
        public CmdAddConnectionImpl CmdAddConnection { get; set; }

        /// <summary>
        /// Gets the Buttons for the Add-Button of the GUI
        /// </summary>
        /// <returns>All Buttons, that are used in the Add-Split-Button-Menu</returns>
        public List<RibbonSplitMenuItem> GetAddProtocolList()
        {
           return(null);
        }
    }
}
