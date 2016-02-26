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

namespace beRemote.GUI.Notification
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        public NotificationWindow(String message)
        {
            InitializeComponent();

            rtbStack.AppendText(message);
        }

        private void Continue_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
