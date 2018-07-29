using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.ViewModel.Command;

namespace beRemote.GUI.Tabs.ManageProfile.Commands
{
    public class CmdSaveClickImpl : BaseCommand
    {
        public override void Execute(object parameter)
        {
            OnSave(new RoutedEventArgs());
        }


        #region Events

        #region Save

        public delegate void SaveEventHandler(object sender, RoutedEventArgs e);

        public event SaveEventHandler Save;

        protected virtual void OnSave(RoutedEventArgs e)
        {
            var Handler = Save;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion
        #endregion
    }
}
