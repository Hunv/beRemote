using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using beRemote.Core.Definitions.Classes;

namespace beRemote.GUI.Tabs.Import.ImportWorker
{
    /// <summary>
    /// Interaction logic for BeRemoteImportDialog.xaml
    /// </summary>
    public partial class BeRemoteImportDialog : INotifyPropertyChanged
    {
        public BeRemoteImportDialog(List<User> userList)
        {
            InitializeComponent();

            UserList = userList;
            RaisePropertyChanged("UserList");
        }

        /// <summary>
        /// The Selected UserId
        /// </summary>
        public int SelectedUserId { get; set; }

        /// <summary>
        /// Available Users in the beRemote Database
        /// </summary>
        public List<User> UserList { get; set; }


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

    }
}
