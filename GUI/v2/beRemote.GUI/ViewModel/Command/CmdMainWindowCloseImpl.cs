using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using beRemote.GUI;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdMainWindowCloseImpl : ICommand, INotifyPropertyChanged
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            var evArgs = new MainWindowCloseEventArgs();
            
            //evArgs.View = (MainWindow)sender;
            evArgs.View = (MainWindow) App.Current.MainWindow;
            
            OnApplicationClose(evArgs);
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

        public delegate void ApplicationCloseEventHandler(object sender, MainWindowCloseEventArgs e);

        public event ApplicationCloseEventHandler ApplicationClose;

        protected virtual void OnApplicationClose(MainWindowCloseEventArgs e)
        {
            var Handler = ApplicationClose;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
