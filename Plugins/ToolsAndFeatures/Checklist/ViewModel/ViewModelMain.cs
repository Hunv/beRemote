using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.VendorPlugins.Checklist.ViewModel
{
    public class ViewModelMain : INotifyPropertyChanged
    {
        private ObservableCollection<string> _ChecklistItems = new ObservableCollection<string>();
        public ObservableCollection<string> ChecklistItems
        {
            get { return _ChecklistItems; }
            set 
            { 
                ChecklistItems = value;
                RaisePropertyChanged("ChecklistItems");
            }
        }

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
