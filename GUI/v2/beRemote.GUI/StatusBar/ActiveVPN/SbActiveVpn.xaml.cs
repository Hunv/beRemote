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

namespace beRemote.GUI.StatusBar.ActiveVPN
{
    /// <summary>
    /// Interaction logic for SbActiveVpn.xaml
    /// </summary>
    public partial class SbActiveVpn
    {
        public SbActiveVpn()
        {
            InitializeComponent();
        }

        #region UserCount
        public static readonly DependencyProperty ActiveVpnCountProperty =
            DependencyProperty.Register(
                "ActiveVpnCount",
                typeof(int),
                typeof(SbActiveVpn),
                new PropertyMetadata(0, OnActiveVpnCountChanged)
                );

        private static void OnActiveVpnCountChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
        }

        public int ActiveVpnCount
        {
            get
            {
                return ((int)GetValue(ActiveVpnCountProperty));
            }
            set
            {
                SetValue(ActiveVpnCountProperty, value);
            }
        }
        #endregion
    }
}
