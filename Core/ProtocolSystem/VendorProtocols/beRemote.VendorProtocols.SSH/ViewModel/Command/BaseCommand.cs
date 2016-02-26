using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace beRemote.VendorProtocols.SSH.ViewModel.Command
{
    public abstract class BaseCommand : ICommand
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public abstract void Execute(object sender);

        public event EventHandler CanExecuteChanged;
    }
}
