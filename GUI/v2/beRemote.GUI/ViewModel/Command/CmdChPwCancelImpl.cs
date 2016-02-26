using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdChPwCancelImpl : ICommand
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            throw new NotImplementedException("Todo");
        }

        public event EventHandler CanExecuteChanged;
    }
}
