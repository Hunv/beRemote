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
using System.Windows.Threading;

namespace beRemote.GUI.StatusBar.Time
{
    /// <summary>
    /// Interaction logic for SbTime.xaml
    /// </summary>
    public partial class SbTime
    {
        public SbTime()
        {
            InitializeComponent();

            _TmrStatusbarTime.Interval = new TimeSpan(0,0,0,0,950);
            _TmrStatusbarTime.Tick += tmrStatusbarTime_Elapsed;
            _TmrStatusbarTime.Start();
        }


        #region Properties
        #region DisplayTime
        private string _DisplayTime;
        public string DisplayTime 
        {
            get { return _DisplayTime; }
            set 
            {
                _DisplayTime = value;
                RaisePropertyChanged("DisplayTime");
            }
        }
        #endregion
        #endregion

        #region private variables

        private readonly DispatcherTimer _TmrStatusbarTime = new DispatcherTimer();
        void tmrStatusbarTime_Elapsed(object sender, EventArgs args)
        {
            DisplayTime = DateTime.Now.ToLocalTime().ToShortTimeString();
        }
        #endregion
    }
}
