using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Definitions.Enums.Filter;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdAddFilterImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            if (sender == null)
                return;

            int filterId;
            int.TryParse(sender.ToString(), out filterId);

            //If no defined number was returned: return
            if (filterId == 0)
                return;

            var evArgs = new FilterAddEventArgs();
            evArgs.FilterType = (FilterType)filterId;
            OnFilterAdd(evArgs);
        }
    }
}
