using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdWizardMessageImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            OnWizardMessage((WizardMessageEventArgs)sender);
        }


        #region WizardMessage

        public delegate void WizardMessageEventHandler(object sender, WizardMessageEventArgs e);

        public event WizardMessageEventHandler WizardMessage;

        protected virtual void OnWizardMessage(WizardMessageEventArgs e)
        {
            var Handler = WizardMessage;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

    }
}
