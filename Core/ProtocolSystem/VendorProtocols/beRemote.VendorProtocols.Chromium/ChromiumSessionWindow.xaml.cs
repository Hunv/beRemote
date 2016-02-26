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
using CefSharp;
using System.Security;
using beRemote.Core.ProtocolSystem.ProtocolBase;

namespace beRemote.VendorProtocols.Chromium
{
    /// <summary>
    /// Interaction logic for SampleSessionWindow.xaml
    /// </summary>
    public partial class ChromiumSessionWindow : INotifyPropertyChanged
    {
        private string _user;
        private SecureString _pass;
        private string _address;
        private Session _Session;

        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        public ChromiumSessionWindow(Session session)
        {
            InitializeComponent();
            _Session = session;

            WebView.PropertyChanged += OnWebViewPropertyChanged;

            CEF.Initialize(new Settings());

            Header = session.GetSessionServer().GetServerName();
            IconSource = session.GetSessionProtocol().ProtocolIconSmall;
            TabToolTip = session.GetSessionServer().GetServerHostName();
        }


        internal void OpenNewConnection(string domuser, SecureString password)
        {
            _user = domuser;
            _pass = password;            
        }

        private void OnWebViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsBrowserInitialized":
                    if (WebView.IsBrowserInitialized)
                    {
                        string address = _Session.GetSessionServer().GetServerHostName();
                        WebView.Load(address);
                    }

                    break;
            }
        }

        #region Properties        
        public bool CanGoBack { get { return (WebView.CanGoBack); } }
        public bool CanGoNext { get { return (WebView.CanGoForward); } }

        public string WebAddress
        {
            get { return (WebView.Address != null ? WebView.Address : ""); }
            set
            {
                WebView.Address = value;
            }
        }
        #endregion

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            WebView.Back();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            WebView.Forward();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            WebView.Reload();
        }

        private void WebView_LoadCompleted(object sender, LoadCompletedEventArgs url)
        {
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

                var url = ((TextBox)sender).Text;
                if (url == "")
                    return;

                if (!url.StartsWith("http") &&
                    !url.StartsWith("https") &&
                    !url.StartsWith("ftp") &&
                    !url.StartsWith("ftps"))
                    url = "http://" + url;

                try
                {
                    WebView.Address = url;
                    WebView.Load(url);
                }
                catch (Exception)
                {
                    //WebView.Navigate("res://ieframe.dll/dnserrordiagoff.htm#" + url);
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
