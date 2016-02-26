using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security;
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
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.VendorProtocols.HyperVManager.HyperV;
using beRemote.Core.Common.Helper;

namespace beRemote.VendorProtocols.HyperVManager
{
    /// <summary>
    /// Interaction logic for HyperVManagerSessionWindow.xaml
    /// </summary>
    public partial class HyperVManagerSessionWindow : INotifyPropertyChanged
    {
        private Session _Session; //Contains the current Session
        private ObservableCollection<HyperVMachine> _MachineList = new ObservableCollection<HyperVMachine>(); //The List of HyperV-Machines on the Hypervisor
        private Dictionary<string, bool> _ColumnsVisibility = new Dictionary<string, bool>(); //The Dictionary, that tells us, if a Column is marked as visible or not
        private string[] _ColumnOrder = new string[0]; //The Order of the Columns; used only on startup

        #region Events
        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form
        #endregion

        #region WindowsManagement
        public HyperVManagerSessionWindow(Session session)
        {
            InitializeComponent();

            _Session = session;


            Header = session.GetSessionServer().GetServerName();
            IconSource = session.GetSessionProtocol().ProtocolIconSmall;
            TabToolTip = session.GetSessionServer().GetServerHostName();
        }

        internal void OpenNewConnection(string username, SecureString password, string domain)
        {
            //Check, if beRemote has to run with elevated pevileges to access local WMI
            if (_Session.GetSessionServer().GetRemoteIP().ToString() == "::1" ||
                _Session.GetSessionServer().GetRemoteIP().ToString() == "127.0.0.1" ||
                _Session.GetSessionServer().GetServerHostName() == "localhost" ||
                _Session.GetSessionServer().GetServerHostName().ToLower() == System.Environment.MachineName.ToLower())
            {
                if (UacHelper.IsProcessElevated == false)
                {
                    MessageBox.Show("You have to run beRemote \"as Administrator\" if you want to manage a local HyperV-Server", "Elevated permissions required", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _Session.CloseConnection();
                }
            }

            //Get all possible Columns for the DataGrid except the other Types
            foreach (System.Reflection.PropertyInfo propInfo in typeof(HyperVMachine).GetProperties())
            {
                _ColumnsVisibility.Add(propInfo.Name, false);
            }
            //Default visible Columns
            _ColumnsVisibility["Machinename"] = true;
            _ColumnsVisibility["EnabledState"] = true;
            _ColumnsVisibility["OnTimeInDays"] = true;            

            //Load visibility-Settings
            try
            {
                _ColumnsVisibility = _Session.SmartStorage.Load<Dictionary<string, bool>>("ColumnsVisibility", _ColumnsVisibility);
                _ColumnOrder = _Session.SmartStorage.Load<string>("ColumnOrder", _ColumnOrder).Split(';');
            }
            catch { }
            
            //Load Machinelist
            HyperVWMI hyperVcom = new HyperVWMI(_Session.GetSessionServer().GetRemoteIP().ToString(), username, domain, password);
            _MachineList = hyperVcom.GetData();

            //Update GUI
            PropertyChanged(this, new PropertyChangedEventArgs("MachineList"));
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns the current Content of MachineList
        /// </summary>
        public ObservableCollection<HyperVMachine> MachineList
        {
            get { return _MachineList; }
        }

        /// <summary>
        /// Returns a Dictionary of all Columns and their visibility-State
        /// </summary>
        public Dictionary<string, bool> ColumnsVisibility
        {
            get { return _ColumnsVisibility; }
            set 
            {                
                _ColumnsVisibility = value;
                //PropertyChanged(this, new PropertyChangedEventArgs("ColumnsVisibility"));
            }
        }
        #endregion

        private Dictionary<string, string> _ColumnNameDictionary = new Dictionary<string, string>(); //Is the Dictionary to translate the localized Columnheader to the internal Name
        private void dgMachineList_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            try
            {
                //Set the visibility
                if (ColumnsVisibility.ContainsKey(e.Column.Header.ToString()))
                    e.Column.Visibility = (ColumnsVisibility[e.Column.Header.ToString()] ? Visibility.Visible : Visibility.Collapsed);

                //Fill the dictionary to translate from localized to system-Header
                _ColumnNameDictionary.Add(this.Resources["Men" + e.Column.Header.ToString()].ToString(), e.Column.Header.ToString());

                //Set the localized string
                e.Column.Header = this.Resources["Men" + e.Column.Header.ToString()].ToString();
            }
            catch
            {
                e.Column.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private bool isDataTypeVisible(Type data)
        {
            if (data == typeof(HyperVMemory) || data == typeof(HyperVBios) ||
                    data == typeof(HyperVIntegration) || data == typeof(List<HyperVProcessor>) ||
                    data == typeof(List<HyperVNetworkAdapter>) || data == typeof(List<HyperVIdeController>) ||
                    data == typeof(List<HyperVScsiController>) || data == typeof(List<HyperVStorageDevice>) ||
                    data == typeof(List<HyperVSerialController>))
                return false;
            else
                return true;
        }

        #region Context-Menu handling
        #region Properties for Contextmenu-Controling
        private Visibility _ContextVisible = Visibility.Collapsed; //The Visibility-Status of the Datagrid-Contextmenu
        private Thickness _ContextMargin; //The Position of the Datagrid-Contextmenu

        /// <summary>
        /// Public Property for the Datagrid-Contextmenu Visibility
        /// </summary>
        public Visibility ContextVisible
        {
            get { return _ContextVisible; }
            set
            {
                _ContextVisible = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ContextVisible"));
                }
            }
        }

        /// <summary>
        /// Public Property for the Position of the Datagrid-Contextmenu
        /// </summary>
        public Thickness ContextMargin { get { return _ContextMargin; } }
        #endregion

        #region Methods for show/hide 
        private void dgMachineList_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.GetPosition(this).Y > 26 + 24) //26 = height of the toolbar + 24 = height of the columnheader
                return;

            _ContextVisible = System.Windows.Visibility.Visible;
            _ContextMargin = new Thickness(Mouse.GetPosition(this).X, Mouse.GetPosition(this).Y - 26, 0, 0);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ContextMargin"));
                PropertyChanged(this, new PropertyChangedEventArgs("ContextVisible"));
            }
        }

        private void dgMachineList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContextVisible = System.Windows.Visibility.Collapsed;
        }        
        #endregion

        #region Controling check/uncheck of the Columns
        private List<string> uncheckedColumns = new List<string>() { };

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ColumnsVisibility[((CheckBox)sender).Tag.ToString()] = (bool)((CheckBox)sender).IsChecked;

            //Not MVVM
            foreach (DataGridColumn dgc in dgMachineListRef.Columns)
            {
                if (dgc.Header.ToString() == ((CheckBox)sender).Content.ToString())
                {
                    dgc.Visibility = (bool)((CheckBox)sender).IsChecked ? Visibility.Visible : Visibility.Collapsed;
                    break;
                }
            }
        }
        #endregion

        private void hvcontrol_Unloaded(object sender, RoutedEventArgs e)
        {
            //save the visibilestate of the columns
            _Session.SmartStorage.Save("ColumnsVisibility", _ColumnsVisibility);
        }
        #endregion
    }

    #region Converter

    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class VisibilityToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility vis = (Visibility)value;
            switch (vis)
            {
                case Visibility.Collapsed:
                    return false;
                case Visibility.Hidden:
                    return false;
                case Visibility.Visible:
                    return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        #endregion
    }

    [ValueConversion(typeof(HyperVEnabledState), typeof(ImageSource))]
    public class EnabledStateToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            HyperVEnabledState state = (HyperVEnabledState)value;

            BitmapImage bI = new BitmapImage();
            bI.BeginInit();

            switch (state)
            { 
                default:
                case HyperVEnabledState.Unknown:
                case HyperVEnabledState.Other:
                case HyperVEnabledState.NotApplicable:
                bI.UriSource = new Uri("GUI/redstate.png", UriKind.Relative);
                    break;

                case HyperVEnabledState.Enabled:
                    bI.UriSource = new Uri("GUI/greenstate.png", UriKind.Relative);
                    break;
                
                case HyperVEnabledState.Disabled:
                    bI.UriSource = new Uri("GUI/graystate.png", UriKind.Relative);
                    break;
                                
                case HyperVEnabledState.Paused:
                case HyperVEnabledState.Suspended:
                case HyperVEnabledState.Quiesce: //"Paused"-State
                case HyperVEnabledState.EnabledButOffline: //"Saved"-State
                    bI.UriSource = new Uri("GUI/bluestate.png", UriKind.Relative);
                    break;
                                    
                case HyperVEnabledState.Deferred:
                case HyperVEnabledState.InTest:
                case HyperVEnabledState.ShuttingDown:
                case HyperVEnabledState.Starting:
                case HyperVEnabledState.Snapshotting:
                case HyperVEnabledState.Saving:
                case HyperVEnabledState.Stopping:
                case HyperVEnabledState.Pausing:
                case HyperVEnabledState.Resuming:
                    bI.UriSource = new Uri("GUI/yellowstate.png", UriKind.Relative);
                    break;
                
            }

            bI.EndInit();
            return (bI);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (HyperVEnabledState.Unknown);
        }
    }



    #endregion

    #region Freezable for Context-Menu-Data-Transmition
    public class BindingProxy : Freezable
    {
        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
    #endregion

}
