using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.ViewModel.Command;

namespace beRemote.GUI.Overlay.FirstRunWizard
{
    public class CmdBtnNextImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnNextTabEvent(new RoutedEventArgs());
        }


        #region NextTab

        public delegate void NextTabEventHandler(object sender, RoutedEventArgs e);

        public event NextTabEventHandler NextTab;

        protected virtual void OnNextTabEvent(RoutedEventArgs e)
        {
            var Handler = NextTab;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
