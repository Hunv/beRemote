using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.Tabs.Help
{
    /// <summary>
    /// Interaction logic for ContentTabTesttab.xaml
    /// </summary>
    public partial class TabHelp
    {
        private BackgroundWorker bgWorker = new BackgroundWorker(); //Backgroundworker
        public bool sendSuccess = true; //Was the sending successfull?

        public TabHelp()
        {
            InitializeComponent();

            foreach (var inst in Core.Common.Helper.CLI.AbstractOptions.UsedInstances)
            {
                lvCLIParams.Items.Add(String.Join("\r\n", inst.GetHelpInfo()));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String result = "";
            foreach (var inst in Core.Common.Helper.CLI.AbstractOptions.UsedInstances)
            {
                result += String.Join("\r\n", inst.GetHelpInfo());
            }

            MessageBox.Show(result);
        }

        public override void Dispose()
        {
            base.Dispose();

            bgWorker.Dispose();
        }

    }
}
