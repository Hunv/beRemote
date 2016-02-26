using System;
using System.Collections.Generic;
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

namespace beRemote.GUI.StatusBar.ConnectTime
{
    /// <summary>
    /// Interaction logic for SbConnectTime.xaml
    /// </summary>
    public partial class SbConnectTime
    {
        public SbConnectTime()
        {
            InitializeComponent();
        }

        #region ConnectTime
        private string _ConnectTime = "";
        public string ConnectTime
        {
            get { return _ConnectTime; }
            set
            {
                _ConnectTime = value;

                if (value == "")
                    ShowConnectTime = false;

                RaisePropertyChanged("ConnectTime");
            }
        }
        #endregion

        #region ShowConnectTime
        private bool _ShowConnectTime = true;
        public bool ShowConnectTime
        {
            get { return _ShowConnectTime; }
            set
            {
                _ShowConnectTime = value;
                RaisePropertyChanged("ShowConnectTime");
            }
        }
        #endregion
    }
}
