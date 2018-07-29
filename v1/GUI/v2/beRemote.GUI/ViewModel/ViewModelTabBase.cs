using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using beRemote.GUI.ViewModel.Enumeration;

namespace beRemote.GUI.ViewModel
{
    public class ViewModelTabBase : INotifyPropertyChanged, IDisposable
    {
        #region Constructor

        public ViewModelTabBase()
        {
            
        }

        #endregion

        #region MainViewModel Reference
        /// <summary>
        /// a Reference to the MainViewModel (i.e. to close a Tab)
        /// </summary>
        public ViewModelMain MainViewModel
        {
            get;
            set;
        }
        #endregion

        #region Title

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title == value) return;

                _Title = value;
                RaisePropertyChanged("Title");
            }
        }

        #endregion

        #region ToolTip

        private string _ToolTip;
        public string ToolTip
        {
            get { return _ToolTip; }
            set
            {
                if (_ToolTip == value) return;

                _ToolTip = value;
                RaisePropertyChanged("ToolTip");
            }
        }

        #endregion

        #region IconSource
        private ImageSource _IconSource;
        public ImageSource IconSource
        {
            get { return _IconSource; }
            set
            {
                if (_IconSource != null && _IconSource.Equals(value)) return;

                _IconSource = value;
                RaisePropertyChanged("IconSource");
            } 
        }
        #endregion

        #region ContentId

        private Guid _ContentId;
        public Guid ContentId
        {
            get { return _ContentId; }
            set
            {
                if (_ContentId == value) return;

                _ContentId = value;
                RaisePropertyChanged("ContentId");
            }
        }

        #endregion

        #region IsSelected

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected == value) return;

                _IsSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        #endregion

        #region IsActive

        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (_IsActive == value) return;

                _IsActive = value;
                RaisePropertyChanged("IsActive");
            }
        }

        #endregion

        #region IsVisible

        private bool _IsVisible;
        public bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                if (_IsVisible == value) return;

                _IsVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        #endregion

        #region Content

        private Control _Content;
        public Control Content
        {
            get { return _Content; }
            set
            {
                if (_Content == value) return;

                _Content = value;
                RaisePropertyChanged("Content");
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
        
        #region TabType

        public TabTypeDefinition TabType
        {
            get;
            set;
        }

        #endregion

        #region Dispose
        public virtual void Dispose()
        {
            Title = "";
            ToolTip = "";
            IconSource = null;            
            Content = null;
        }
        #endregion
    }
}
