using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace beRemote.Core.Exceptions
{
    /// <summary>
    /// Interaction logic for UIExceptionWindow.xaml
    /// </summary>
    public partial class UIExceptionWindow : Window
    {
        public enum brDialogResult
        {
            STOP,
            CONTINUE,
            INVALID
        }

        public brDialogResult DialogResult = brDialogResult.INVALID;
        private beRemoteException exception;
        private beRemoteException beRemoteException;
       
        /// <summary>
        /// SHow a windows with exception inforamtion.
        /// </summary>
        /// <param name="exc">The exception</param>
        /// <param name="terminating">Indicates if the exception will kill the app</param>
        public UIExceptionWindow(beRemoteException exc, bool terminating)
        {
            // TODO: Complete member initialization
            this.exception = exc;

            InitializeComponent();

            rtbStack.AppendText(String.Format("{0}", exception));

            if (terminating == false)
            {
                DialogResult = brDialogResult.CONTINUE;
                cmdStop.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                DialogResult = brDialogResult.STOP;
                cmdContinue.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = brDialogResult.STOP;
            this.Close();
        }

        private void Continue_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = brDialogResult.CONTINUE;
            this.Close();
        }

     
    }
}
