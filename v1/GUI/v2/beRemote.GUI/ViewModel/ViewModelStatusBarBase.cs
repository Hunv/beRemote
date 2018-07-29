using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Definitions.Classes;

namespace beRemote.GUI.ViewModel
{
    public class ViewModelStatusBarBase : INotifyPropertyChanged
    {
        #region Element
        private StatusBarBase _Element;
        public StatusBarBase Element 
        {
            get { return _Element; }
            set
            {
                _Element = value;
                RaisePropertyChanged("Element");
            }
        }
        #endregion

        #region PropertyChanged
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
