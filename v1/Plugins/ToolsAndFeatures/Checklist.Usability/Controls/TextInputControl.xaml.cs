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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Checklist.Usability.Controls
{
    /// <summary>
    /// Interaction logic for TextInputControl.xaml
    /// </summary>
    public partial class TextInputControl : INotifyPropertyChanged
    {
        public TextInputControl()
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

        private string _TextValue = "";
        public string TextValue 
        {
            get { return _TextValue; }
            set
            {
                if (_TextValue == value)
                    return;

                _TextValue = value;
                RaisePropertyChanged("TextValue");

                if (_TextValue.Length > 0)
                {
                    IsChecked = true;
                }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
