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

namespace beRemote.GUI.StatusBar.Stopwatch
{
    /// <summary>
    /// Interaction logic for SbStopwatch.xaml
    /// </summary>
    public partial class SbStopwatch
    {
        public SbStopwatch()
        {
            InitializeComponent();
        }

        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) 
                return;

            cmbStopwatch.Items.Add(DateTime.Now.ToLocalTime().ToShortTimeString() + " " + cmbStopwatch.Text);
            cmbStopwatch.SelectedIndex = cmbStopwatch.Items.Count - 1;
            cmbStopwatch.IsEditable = false;
            cmbStopwatch.IsEditable = true;
        }

        private void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {
            cmbStopwatch.Items.Clear();
        }

        private void MenuItemCopy_Click(object sender, RoutedEventArgs e)
        {
            var cacheString = "";
            foreach (var anItem in cmbStopwatch.Items)
            {
                cacheString += anItem + Environment.NewLine;
            }

            Clipboard.SetText(cacheString);
        }
    }
}
