using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace beRemote.VendorProtocols.SSH.ViewModel.Command
{
    public class CmdSendCommandImpl : BaseCommand
    {
        public override void Execute(object parameter)
        {
            OnSendCommand(new RoutedEventArgs());
        }


        #region SendCommand

        public delegate void SendCommandEventHandler(object sender, RoutedEventArgs e);

        public event SendCommandEventHandler SendCommand;

        protected virtual void OnSendCommand(RoutedEventArgs e)
        {
            var Handler = SendCommand;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion
    }
}
