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

namespace beRemote.Core.ExceptionSystem.ExceptionBase.GUI
{
    /// <summary>
    /// Interaktionslogik für ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionWindow : Window
    {
        public ExceptionWindow(Exceptions.BERemoteGUIException exception, Window owner)
        {
            InitializeComponent();
            //if(owner != null)
            //    owner.Dispatcher.Invoke(new Action(() => this.Owner = owner));
            this.Owner = owner;
            //this.Dispatcher.Invoke(new Action(() => this.Owner = owner));

            this.ShowInTaskbar = false;
            this.Topmost = true;

            this.Tag = exception;
            this.tbMessage.Text = exception.GetMessage();
            if (exception.InnerException != null)
                this.tbMessage.Text += "\r\n" + exception.InnerException.Message;
            this.tbDetail.Text = exception.ToString();

            
        }

        private void cmdStop_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(88);
        }
    }
}
