using beRemote.Database;
using Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
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

namespace beRemote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private CompositionContainer _container;
        private NLog.Logger _logger;

        public MainWindow()
        {
            //Initialize the Logger
            _logger = NLog.LogManager.GetCurrentClassLogger();

            //Initialize Database
            using (brDbContext dbContext = new brDbContext())
            {
                
            }

            //Use Windows Setting for Fluent Ribbon Theme
            ThemeManager.IsAutomaticWindowsAppModeSettingSyncEnabled = true;
            ThemeManager.SyncAppThemeWithWindowsAppModeSetting();

            // Set the current user interface culture to the specific culture
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");

            InitializeComponent();
        }
    }
}