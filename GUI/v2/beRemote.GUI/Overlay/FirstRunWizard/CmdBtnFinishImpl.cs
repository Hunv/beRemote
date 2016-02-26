using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.ViewModel.Command;

namespace beRemote.GUI.Overlay.FirstRunWizard
{
    public class CmdBtnFinishImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnFinishWizardEvent(new RoutedEventArgs());
        }

        #region FinishWizard

        public delegate void FinishWizardEventHandler(object sender, RoutedEventArgs e);

        public event FinishWizardEventHandler FinishWizard;

        protected virtual void OnFinishWizardEvent(RoutedEventArgs e)
        {
            var Handler = FinishWizard;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }


}
