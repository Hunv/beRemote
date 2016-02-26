using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using beRemote.Core;
using beRemote.Core.Common.LogSystem;
using beRemote.GUI.Notification;
using beRemote.GUI.ViewModel.EventArg;
using Xceed.Wpf.AvalonDock;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdMainWindowClosingImpl : ICommand, INotifyPropertyChanged
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            var rea = new MainWindowClosingEventArgs();
            rea.DockMgr = (DockingManager)sender;
            OnApplicationClosing(rea);
        }

        public event EventHandler CanExecuteChanged;


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        /// <summary>
        /// Helper for Triggering PropertyChanged
        /// </summary>
        /// <param name="triggerControl">The Name of the Property to update</param>
        private void RaisePropertyChanged(string triggerControl)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(triggerControl));
            }
        }
        #endregion

        #region Events

        public delegate void ApplicationClosingEventHandler(object sender, MainWindowClosingEventArgs e);

        public event ApplicationClosingEventHandler ApplicationClosing;

        protected virtual void OnApplicationClosing(MainWindowClosingEventArgs e)
        {
            var Handler = ApplicationClosing;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
