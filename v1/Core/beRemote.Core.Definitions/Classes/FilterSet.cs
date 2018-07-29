using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace beRemote.Core.Definitions.Classes
{
    [Serializable]
    public class FilterSet : INotifyPropertyChanged
    {
        long _Id = 0;
        string _Title = "";
        List<FilterClass> _Filter = new List<FilterClass>();
        private long _Parent = 0;
        private bool _Hide = false;

        //To Update Properties in the Listbox
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id
        {
            get { return (_Id); }
            set { _Id = value; }
        }

        public string Title
        {
            get { return (_Title); }
            set
            {
                _Title = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
            }
        }

        public List<FilterClass> Filter
        {
            get { return (_Filter); }
            set { _Filter = value; }
        }

        public long Parent
        {
            get { return (_Parent); }
            set { _Parent = value; }
        }

        public bool Hide
        {
            get { return (_Hide); }
            set
            {
                _Hide = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayColor"));
            }
        }

        public System.Windows.Media.SolidColorBrush DisplayColor
        {
            get
            {
                if (Hide == true)
                {
                    return (System.Windows.Media.Brushes.Gray);
                }
                else
                {
                    return (System.Windows.Media.Brushes.Black);
                }
            }
        }
    }
}
