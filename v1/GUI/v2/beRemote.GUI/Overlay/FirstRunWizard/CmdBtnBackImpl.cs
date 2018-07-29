using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.ViewModel.Command;

namespace beRemote.GUI.Overlay.FirstRunWizard
{
    public class CmdBtnBackImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnPrevTabEvent(new RoutedEventArgs());
        }

        #region PrevTab

        public delegate void PrevTabEventHandler(object sender, RoutedEventArgs e);

        public event PrevTabEventHandler PrevTab;

        protected virtual void OnPrevTabEvent(RoutedEventArgs e)
        {
            var Handler = PrevTab;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
