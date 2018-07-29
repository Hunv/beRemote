using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Definitions.Classes;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdConnectionFilterChangedImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            var evArgs = new ConnectionFilterChangedEventArgs();
            evArgs.NewFilter = (FilterSet)sender;

            OnApplyFilter(evArgs);
        }
    }
}
