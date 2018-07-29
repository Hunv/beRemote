using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace beRemote.GUI.Tabs.ManageFolder
{
    public class CmdTabManageFolderAddFolderClickImpl : ICommand, INotifyPropertyChanged
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            if (sender == null)
                return;

            var evArg = new FolderAddEventArgs();
            evArg.View = (TabManageFolder)sender;
            OnTabManageFolderAddFolderClick(evArg);
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

        public delegate void TabManageFolderAddFolderClickEventHandler(object sender, FolderAddEventArgs e);

        public event TabManageFolderAddFolderClickEventHandler TabManageFolderAddFolderClick;

        protected virtual void OnTabManageFolderAddFolderClick(FolderAddEventArgs e)
        {
            var Handler = TabManageFolderAddFolderClick;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
