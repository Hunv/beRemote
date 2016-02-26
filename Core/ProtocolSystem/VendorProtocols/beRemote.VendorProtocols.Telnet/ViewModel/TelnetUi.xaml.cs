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
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.VendorProtocols.Telnet.EventArgs;

namespace beRemote.VendorProtocols.Telnet.ViewModel
{
    /// <summary>
    /// Interaction logic for TelnetUi.xaml
    /// </summary>
    public partial class TelnetUi
    {
        public TelnetUi(string title, ImageSource icon, string tooltip, bool textWrap)
        {
            InitializeComponent();

            ((ViewModelMain)DataContext).SendInput += TelnetUi_SendInput;
            ((ViewModelMain) DataContext).TextWrap = textWrap;

            Header = title;
            IconSource = icon;
            TabToolTip = tooltip;
        }

        void TelnetUi_SendInput(object sender, SendInputEventArgs e)
        {
            if (SendInput != null)
                SendInput(sender, e);
        }

        #region SendInput

        public delegate void SendInputEventHandler(object sender, SendInputEventArgs e);

        public event ViewModelMain.SendInputEventHandler SendInput;

        public bool IsSendInputRegistered
        {
            get { return (SendInput != null); }
        }

        #endregion

        #region DisplayText
        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register(
            "DisplayText", 
            typeof(string), 
            typeof(TelnetUi), 
            new FrameworkPropertyMetadata(
                "",
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisplayTextChanged));

        private static void OnDisplayTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ViewModelMain) ((TelnetUi) d).DataContext).DisplayText = e.NewValue.ToString();
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {
            base.Dispose();

            ((ViewModelMain)DataContext).SendInput -= TelnetUi_SendInput;
            ((ViewModelMain) DataContext).Dispose();

            Header = "";
            IconSource = null;
            TabToolTip = "";
            DisplayText = "";
        }
        #endregion
    }
}
