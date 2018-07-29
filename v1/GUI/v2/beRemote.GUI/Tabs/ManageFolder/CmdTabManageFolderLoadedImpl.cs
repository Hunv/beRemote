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

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdTabManageFolderLoadedImpl : ICommand, INotifyPropertyChanged
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            OnTabManageFolderLoaded(new RoutedEventArgs());
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

        #region Event

        public delegate void TabManageFolderLoadedEventHandler(object sender, RoutedEventArgs e);

        public event TabManageFolderLoadedEventHandler TabManageFolderLoaded;

        protected virtual void OnTabManageFolderLoaded(RoutedEventArgs e)
        {
            var Handler = TabManageFolderLoaded;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
