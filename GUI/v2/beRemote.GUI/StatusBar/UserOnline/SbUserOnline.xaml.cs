using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace beRemote.GUI.StatusBar.UserOnline
{
    /// <summary>
    /// Interaction logic for UserOnline.xaml
    /// </summary>
    public partial class SbUserOnline
    {
        public SbUserOnline()
        {
            InitializeComponent();
        }

        #region UserCount
        public static readonly DependencyProperty UserCountProperty =
            DependencyProperty.Register(
                "UserCount",
                typeof(int),
                typeof(SbUserOnline),
                new PropertyMetadata(0, OnUserCountChanged)
                );

        private static void OnUserCountChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is int)) return;
            if (!(target is SbUserOnline)) return;

            ((SbUserOnline)target).IsUserCountVisible = (int)e.NewValue > 0;
        }

        public int UserCount
        {
            get
            {
                return ((int)GetValue(UserCountProperty));
            }
            set
            {
                SetValue(UserCountProperty, value);
            }
        }
        #endregion

        #region UserName
        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register(
                "UserName",
                typeof(string),
                typeof(SbUserOnline)
                );

        /// <summary>
        /// Gets the Item, that is right-clicked, if not right clicked, selected
        /// </summary>
        /// <returns></returns>
        public string UserName
        {
            get
            {
                return ((string)GetValue(UserNameProperty));
            }
            set
            {
                SetValue(UserNameProperty, value);
            }
        }
        #endregion

        #region IsUserCountVisible
        public static readonly DependencyProperty IsUserCountVisibleProperty =
            DependencyProperty.Register(
                "IsUserCountVisible",
                typeof(bool),
                typeof(SbUserOnline)
                );

        public bool IsUserCountVisible
        {
            get
            {
                return ((bool)GetValue(IsUserCountVisibleProperty));
            }
            set
            {
                SetValue(IsUserCountVisibleProperty, value);
            }
        }
        #endregion
    }
}
