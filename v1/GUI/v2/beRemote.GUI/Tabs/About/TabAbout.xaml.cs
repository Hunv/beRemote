using System;

namespace beRemote.GUI.Tabs.About
{
    /// <summary>
    /// Interaction logic for ContentTabAbout.xaml
    /// </summary>
    public partial class TabAbout
    {
        public TabAbout()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            tbTeam.Text = "";

            var rnd = new Random(DateTime.Now.Second);
            var firstmember = rnd.Next(0, 2);
            switch (firstmember)
            {
                case 0:
                    tbTeam.Text += "Kristian Reukauff (Development, Design, Publishing) (2012-2015)" + Environment.NewLine;
                    tbTeam.Text += "Benedikt Kröning (Development, Design, Publishing) (2012-2015)" + Environment.NewLine;
                    break;
                case 1:
                    tbTeam.Text += "Benedikt Kröning (Development, Design, Publishing) (2012-2015)" + Environment.NewLine;
                    tbTeam.Text += "Kristian Reukauff (Development, Design, Publishing) (2012-2015)" + Environment.NewLine;
                    break;
            }

        }

        private void ViewModelTabGeneral_Initialized(object sender, EventArgs e)
        {
            UserControl_Loaded(sender, null);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
