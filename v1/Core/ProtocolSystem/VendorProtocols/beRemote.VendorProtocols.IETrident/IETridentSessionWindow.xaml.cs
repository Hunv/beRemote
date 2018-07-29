using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Security;
using beRemote.Core.ProtocolSystem.ProtocolBase;

namespace beRemote.VendorProtocols.IETrident
{
    /// <summary>
    /// Interaction logic for SampleSessionWindow.xaml
    /// </summary>
    public partial class IETridentSessionWindow : INotifyPropertyChanged, IDisposable
    {
        private string _user;
        private SecureString _pass;
        private string _address;
        private Session _Session;

        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        public IETridentSessionWindow(Session session)
        {
            InitializeComponent();
            _Session = session;

            Header = session.GetSessionServer().GetServerName();
            IconSource = session.GetSessionProtocol().ProtocolIconSmall;
            TabToolTip = session.GetSessionServer().GetServerHostName();
        }


        internal void OpenNewConnection(string domuser, SecureString password)
        {
            _user = domuser;
            _pass = password;

            string url = _Session.GetSessionServer().GetServerHostName();
            if (!url.Contains("://")) //If no Protocol is given; Trident needs this.
                url = "http://" + url;

            WebView.Navigate(url);
        }


        #region Properties
        public bool CanGoBack { get { return (WebView.CanGoBack); } }
        public bool CanGoNext { get { return (WebView.CanGoForward); } }

        private string _WebAddress = "";

        public string WebAddress
        {
            get { return (_WebAddress); }
            set
            {
                _WebAddress = value;
                RaisePropertyChanged("WebAddress");
            }
        }
        #endregion

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            WebView.GoBack();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            WebView.GoForward();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            WebView.Refresh();
        }

        private void WebView_LoadCompleted(object sender, NavigationEventArgs url)
        {
            _WebAddress = url.Uri.OriginalString;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("CanGoBack"));
                PropertyChanged(this, new PropertyChangedEventArgs("CanGoNext"));
                PropertyChanged(this, new PropertyChangedEventArgs("WebAddress"));
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == null)
                    return;

                var url = ((TextBox) sender).Text;
                if (url == "")
                    return;

                if (!url.StartsWith("http") &&
                    !url.StartsWith("https") &&
                    !url.StartsWith("ftp") &&
                    !url.StartsWith("ftps"))
                    url = "http://" + url;

                try
                {
                    WebView.Navigate(url);
                }
                catch (Exception)
                {
                    WebView.Navigate("res://ieframe.dll/dnserrordiagoff.htm#" + url);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            WebView.Dispose();
            WebView = null;
        }
    }
}
