using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace beRemote.Core.ProtocolSystem.ProtocolBase.GUI
{
    /// <summary>
    /// Interaction logic for UserCredentialsWindow.xaml
    /// </summary>
    public partial class UserCredentialsWindow : Window
    {
        public UserCredentialsWindow()
        {
            InitializeComponent();

            txtUsername.Focus();
        }
        
        private void cmdContinue_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
                txtPassword.Focus();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
                cmdContinue_Click(cmdContinue, new RoutedEventArgs());
        }
    }
}
