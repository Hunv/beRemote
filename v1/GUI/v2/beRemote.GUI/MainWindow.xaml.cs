using System;
using System.Text;
using System.Windows;
using beRemote.Core;
using beRemote.GUI.ViewModel;
using Application = System.Windows.Application;

namespace beRemote.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            Kernel.ParsedOptions.ParseArguments(Environment.GetCommandLineArgs());

            if (Kernel.ParsedOptions.ContainsParameter("?") || Kernel.ParsedOptions.ContainsParameter("help"))
            {
                var sb = new StringBuilder("Following CLI options are present and useable:\r\n===============================================\r\n");

                foreach (var commandLineOptionAttribute in Kernel.ParsedOptions.GetParameters())
                {
                    sb.Append(String.Format("{0} - {1}\r\n", commandLineOptionAttribute.KeyCollection[0], commandLineOptionAttribute.Description));
                }

                MessageBox.Show(this, sb.ToString());

                Close();
                return;
            }

           
            InitializeComponent();

            //Required, because binding of Widht, Height, Left and Top not possible
            ((ViewModelMain)DataContext).WindowPropertiesChanged += MainWindow_WindowPropertiesChanged;
            ((ViewModelMain)DataContext).CloseApplication += MainWindow_CloseApplication;
        }

        void MainWindow_CloseApplication(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Windowappeareance Handling (not MVVM conform)
        /// <summary>
        /// Sets the Windowparameters; not MVVM conform, but Width, Height, Left and Top-Binding not possible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_WindowPropertiesChanged(object sender, GUI.ViewModel.EventArg.WindowPropertiesChangedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
                Width = e.WindowWidth;
                Height = e.WindowHeight;
                Left = e.WindowPositionX;
                Top = e.WindowPositionY;
            }));
            
        }

        private void beRemote_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((ViewModelMain)DataContext).UiWindowSizeWidth = Convert.ToInt32(e.NewSize.Width);
            ((ViewModelMain)DataContext).UiWindowSizeHeight =Convert.ToInt32( e.NewSize.Height);            
        }

        private void beRemote_LocationChanged(object sender, EventArgs e)
        {
            ((ViewModelMain)DataContext).UiWindowPositionLeft = Convert.ToInt32(Left);
            ((ViewModelMain)DataContext).UiWindowPositionTop =Convert.ToInt32( Top);
        }
        #endregion

        private void pbCreateAccountPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Not MVVM conform, but safer than doing it with MVVM
            ((ViewModelMain)DataContext).CreateAccountPassword1 = pbCreateAccountPassword.SecurePassword;
        }

        private void pbCreateAccountPassword2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Not MVVM conform, but safer than doing it with MVVM
            ((ViewModelMain)DataContext).CreateAccountPassword2 = pbCreateAccountPassword2.SecurePassword;
        }
    }
}
