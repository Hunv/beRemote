using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Checklist.Usability.Controls
{
    /// <summary>
    /// Interaction logic for MultipleChoiseControl.xaml
    /// </summary>
    public partial class MultipleChoiseControl : INotifyPropertyChanged
    {
        public MultipleChoiseControl()
        {
            InitializeComponent();
        }

        private bool _IsChecked = false;
        public bool IsChecked
        {
            get { return _IsChecked; }
            private set
            {
                if (value == _IsChecked)
                    return;

                _IsChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }
        
        private string _DisplayText = "";
        public string DisplayText
        {
            get { return _DisplayText; }
            set
            {
                if (_DisplayText == value)
                    return;

                _DisplayText = value;
                RaisePropertyChanged("DisplayText");

                if (_DisplayText.Length > 0)
                {
                    IsChecked = true;
                }
            }
        }

        private ImageSource _ButtonImage;
        public ImageSource ButtonImage
        {
            get { return _ButtonImage; }
            set
            {
                _ButtonImage = value;
            }
        }

        private object _ActionLink;
        public object ActionLink
        {
            get { return _ActionLink; }
            set
            {
                if (_ActionLink == value)
                    return;

                _ActionLink = value;
                RaisePropertyChanged("ActionLink");
            }
        }

        private ObservableCollection<object> _ChoiseList;
        public ObservableCollection<object> ChoiseList
        {
            get { return _ChoiseList; }
            set
            {
                if (_ChoiseList == value)
                    return;

                _ChoiseList = value;
                RaisePropertyChanged("ChoiseList");
            }
        }

        private object _SelectedChoise;
        public object SelectedChoise
        {
            get { return _SelectedChoise; }
            private set 
            {
                if (_SelectedChoise == value)
                    return;

                _SelectedChoise = value;
                RaisePropertyChanged("SelectedChoise");
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

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
