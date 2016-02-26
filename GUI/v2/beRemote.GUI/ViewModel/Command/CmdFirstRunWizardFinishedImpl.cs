using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdFirstRunWizardFinishedImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnWizardFinished((WizardResultEventArgs) sender);
        }


        #region WizardFinished

        public delegate void WizardFinishedEventHandler(object sender, WizardResultEventArgs e);

        public event WizardFinishedEventHandler WizardFinished;

        protected virtual void OnWizardFinished(WizardResultEventArgs e)
        {
            var Handler = WizardFinished;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

    }
}
